using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Projectile.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : AbstractPrefabedPooledObject<ProjectilePrefab> {

	private static int[] collisionMask;
	private bool to_deactivate;

	private GameObject gameObject;
	private Transform transform;
	private SpriteRenderer renderer;

	/// <summary>
	/// The damage.
	/// </summary>
	private int damage;
	public int Damage {
		get {
			return damage;
		}
		set {
			damage = value;
		}
	}

	/// <summary>
	/// The linear velocity.
	/// </summary>
	private float linearVelocity = 0f;
	public float Velocity {
		get {
			return linearVelocity;
		}
		set {
			linearVelocity = value;
		}
	}

	/// <summary>
	/// The angular velocity.
	/// </summary>
	private Quaternion angularVelocity = Quaternion.identity;
	public float AngularVelocity {
		get {
			return angularVelocity.eulerAngles.z;
		}
		set {
			angularVelocity = Quaternion.Euler(new Vector3(0f, 0f, value));
		}
	}

	/// <summary>
	/// Gets or sets the angular velocity radians.
	/// </summary>
	/// <value>The angular velocity radians.</value>
	public float AngularVelocityRadians
	{
		get {
			return AngularVelocity * Util.Degree2Rad;
		}
		set {
			AngularVelocity = value * Util.Rad2Degree;
		}
	}

	/// <summary>
	/// The controllers.
	/// </summary>
	private List<AbstractProjectileController> controllers;

	/// <summary>
	/// The properties.
	/// </summary>
	private Dictionary<string, object> properties;

	/// <summary>
	/// The circle center.
	/// </summary>
	private Vector2 circleCenter = Vector2.zero; 

	/// <summary>
	/// The circle raidus.
	/// </summary>
	private float circleRaidus = 1f;

	public SpriteRenderer SpriteRenderer {
		get {
			return renderer;
		}
	}

	public Transform Transform {
		get {
			return transform;
		}
	}

	private float fireTimer;

	public float BulletTime {
		get {
			return fireTimer;
		}
	}

	private RaycastHit2D[] hits;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	public Projectile() {
		properties = new Dictionary<string, object> ();
		controllers = new List<AbstractProjectileController> ();
		if(collisionMask == null)
			collisionMask = Util.CollisionLayers2D();
		gameObject = new GameObject ("Projectile");
		renderer = gameObject.AddComponent<SpriteRenderer> ();
		transform = gameObject.transform;
		hits = new RaycastHit2D[10];
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	public void Update(float dt) {
		fireTimer += dt;
		//Rotate
		if(angularVelocity != Quaternion.identity) {
			transform.rotation = Quaternion.Slerp (transform.rotation, transform.rotation * angularVelocity, dt);
		}
		float movementDistance = linearVelocity * dt;
		
		Vector3 movementVector = transform.up * movementDistance;
		//Debug.DrawRay (Transform.position, movementVector);
		int count = Physics2D.CircleCastNonAlloc(transform.position + Util.To3D(circleCenter), 
						                           circleRaidus,
							                       Transform.up,
			                                       hits,
						                           movementDistance,
						                           collisionMask[gameObject.layer]);
		transform.position += movementVector;

		//Translate
		if(count > 0) {
			int i;
			for (i = 0; i < count; i++) {
				RaycastHit2D hit = hits[i];
				if (hit.collider != null) {
					hit.collider.SendMessage("OnProjectileCollision", this, SendMessageOptions.DontRequireReceiver);
				}
				if(to_deactivate){
					Transform.position = hits[i].point;
					break;
				}
			}
		}

		if(controllers.Count > 0)
			for(int i = 0; i < controllers.Count; i++)
				if(controllers[i] != null)
					controllers[i].UpdateBullet(this, dt);

		if (to_deactivate) {
			DeactivateImmediate();
		}
	}

	/// <summary>
	/// Transfer the specified currentController and targetField.
	/// </summary>
	/// <param name="currentController">Current controller.</param>
	/// <param name="targetField">Target field.</param>
	public void Transfer(PhantasmagoriaField currentController, PhantasmagoriaField targetField) {
		Vector2 relativePos = currentController.FieldPoint (Transform.position);
		Transform.position = targetField.WorldPoint (relativePos);
	}

	/// <summary>
	/// Matchs the prefab.
	/// </summary>
	/// <param name="prefab">Prefab.</param>
	public override void MatchPrefab(ProjectilePrefab prefab) {
		CircleCollider2D cc = prefab.CircleCollider;
		SpriteRenderer sr = prefab.SpriteRenderer;

		transform.localScale = prefab.Transform.localScale;
		gameObject.tag = prefab.GameObject.tag;
		gameObject.layer = prefab.GameObject.layer;

		if(sr != null) {
			renderer.sprite = sr.sprite;
			renderer.color = sr.color;
			renderer.sharedMaterial = sr.sharedMaterial;
			renderer.sortingOrder = sr.sortingOrder;
			renderer.sortingLayerID = sr.sortingLayerID;
		}
		else
			Debug.LogError("The provided prefab should have a SpriteRenderer!");

		if(cc != null) {//circleCollider.enabled = cc != null) {
//			circleCollider.center = cc.center;
//			circleCollider.radius = cc.radius;
			circleCenter = Util.ComponentProduct2(Transform.lossyScale, cc.center);
			circleRaidus = cc.radius * Util.MaxComponent2(Util.To2D(Transform.lossyScale));
		}
		else
			Debug.LogError("The provided prefab should a CircleCollider2D!");
	}

	/// <summary>
	/// Determines whether this instance has property the specified key.
	/// </summary>
	/// <returns><c>true</c> if this instance has property the specified key; otherwise, <c>false</c>.</returns>
	/// <param name="key">Key.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public bool HasProperty<T>(string key) {
		return (properties.ContainsKey(key)) && (properties[key] is T);
	}

	/// <summary>
	/// Gets the property.
	/// </summary>
	/// <returns>The property.</returns>
	/// <param name="key">Key.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public T GetProperty<T>(string key) {
		if(properties.ContainsKey(key))
			return (T)properties[key];
		else 
			return default(T);
	}
	/// <summary>
	/// Sets the property.
	/// </summary>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public void SetProperty<T>(string key, T value) {
		properties [key] = value;
	}

	/// <summary>
	/// Adds the controller.
	/// </summary>
	/// <param name="controller">Controller.</param>
	public void AddController(AbstractProjectileController controller) {
		controllers.Add (controller);
		controller.OnControllerAdd (this);
	}

	/// <summary>
	/// Removes the controller.
	/// </summary>
	/// <param name="controller">Controller.</param>
	public void RemoveController (AbstractProjectileController controller) {
		if(controllers.Remove(controller))
			controller.OnControllerRemove(this);
	}

	public override void Activate () {
		to_deactivate = false;
		gameObject.SetActive (true);
	}

	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	public override void Deactivate()  {
		to_deactivate = true;
	}

	public void DeactivateImmediate() {
		properties.Clear ();
		controllers.Clear ();
		linearVelocity = 0f;
		fireTimer = 0f;
		damage = 0;
		angularVelocity = Quaternion.identity;
		gameObject.SetActive (false);
		base.Deactivate ();
	}
}
