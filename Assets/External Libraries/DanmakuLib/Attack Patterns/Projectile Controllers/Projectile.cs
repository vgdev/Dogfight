using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Projectile.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : PooledObject<ProjectilePrefab> {
	/// <summary>
	/// The player death mask.
	/// </summary>
	private static int playerDeathMask = 1 << 8;

	/// <summary>
	/// The enemy damage mask.
	/// </summary>
	private static int enemyDamageMask = 1 << 13;

	/// <summary>
	/// The combo mask.
	/// </summary>
	private static int comboMask = playerDeathMask | enemyDamageMask;

	/// <summary>
	/// Angles the between 2d.
	/// </summary>
	/// <returns>The between2 d.</returns>
	/// <param name="v1">V1.</param>
	/// <param name="v2">V2.</param>
	public static float AngleBetween2D(Vector2 v1, Vector2 v2) {
		Vector2 diff = v2 - v1;
		return Mathf.Atan2 (diff.y, diff.x) * 180f / Mathf.PI - 90f; 
	}

	/// <summary>
	/// Rotations the between2 d.
	/// </summary>
	/// <returns>The between2 d.</returns>
	/// <param name="v1">V1.</param>
	/// <param name="v2">V2.</param>
	public static Quaternion RotationBetween2D(Vector2 v1, Vector2 v2) {
		return Quaternion.Euler (0f, 0f, AngleBetween2D (v1, v2));
	}

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
	private List<ProjectileController> controllers;

	/// <summary>
	/// The properties.
	/// </summary>
	private Dictionary<string, object> properties;

	/// <summary>
	/// The circle check.
	/// </summary>
	private bool circleCheck = false;

	/// <summary>
	/// The box check.
	/// </summary>
	private bool boxCheck = false;

	/// <summary>
	/// The circle center.
	/// </summary>
	private Vector2 circleCenter = Vector2.zero; 

	/// <summary>
	/// The circle raidus.
	/// </summary>
	private float circleRaidus = 1f;

	/// <summary>
	/// The box center.
	/// </summary>
	private Vector2 boxCenter = Vector2.zero;

	/// <summary>
	/// The size of the box.
	/// </summary>
	private Vector2 boxSize = Vector2.one;

	/// <summary>
	/// The sprite renderer.
	/// </summary>
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private float fireTimer;

	public float BulletTime {
		get {
			return fireTimer;
		}
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	public override void Awake() {
		properties = new Dictionary<string, object> ();
		controllers = new List<ProjectileController> ();
		if(spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate() {
		float dt = Time.fixedDeltaTime;
		fireTimer += dt;
		//Rotate
		Transform.rotation = Quaternion.Slerp (Transform.rotation, Transform.rotation * angularVelocity, dt);
		float movementDistance = linearVelocity * dt;
		Vector3 movementVector = Transform.up * movementDistance;

		RaycastHit2D hit = default(RaycastHit2D);
		if (circleCheck) {//circleCollider.enabled) {
			Vector2 offset = Util.ComponentProduct2(Transform.lossyScale, circleCenter);
			float radius = circleRaidus * Util.MaxComponent2(Util.To2D(Transform.lossyScale));
			hit = Physics2D.CircleCast(Transform.position + Util.To3D(offset), 
			                           radius, 
			                           movementVector, 
			                           movementDistance, 
			                           comboMask);
		}
		if (boxCheck) {//boxCollider.enabled) {
			Vector2 offset = Util.ComponentProduct2(Transform.lossyScale, boxCenter);
			hit = Physics2D.BoxCast(Transform.position + Util.To3D(offset), 
			                        Util.ComponentProduct2(boxSize, Util.To2D(Transform.lossyScale)), 
			                        Transform.rotation.eulerAngles.z, 
			                        movementVector, 
			                        movementDistance,
			                        comboMask);
		}
		Debug.DrawRay (Transform.position, movementVector);

		//Translate
		Transform.position += movementVector;

		if (hit.collider != null) {
			GameObject other = hit.collider.gameObject;
			if(other.layer == playerDeathMask && CompareTag("Bullet")) {
				Transform.position = hit.point;
				AbstractPlayableCharacter avatar = other.GetComponentInParent<AbstractPlayableCharacter>();
				Debug.Log(avatar);
				if(avatar != null) {
					Deactivate();
					avatar.Hit(this);
				}
			} else if(other.layer == enemyDamageMask && CompareTag("Player Shot")) {
				Transform.position = hit.point;
				AbstractEnemy enemy = other.GetComponent<AbstractEnemy>();
				if(enemy != null) {
					enemy.Hit (damage);
					Deactivate();
				}
			}
		}

		for(int i = 0; i < controllers.Count; i++)
			if(controllers[i] != null)
				controllers[i].UpdateBullet(this, dt);
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
		BoxCollider2D bc = prefab.BoxCollider;
		CircleCollider2D cc = prefab.CircleCollider;
		SpriteRenderer sr = prefab.SpriteRenderer;

		Transform.localScale = prefab.Transform.localScale;
		gameObject.tag = prefab.GameObject.tag;
		gameObject.layer = prefab.GameObject.layer;

		if(spriteRenderer != null) {
			spriteRenderer.sprite = sr.sprite;
			spriteRenderer.color = sr.color;
			//spriteRenderer.material = spriteTest.material;
			spriteRenderer.sortingOrder = sr.sortingOrder;
			spriteRenderer.sortingLayerID = sr.sortingLayerID;
		}
		else
			Debug.LogError("The provided prefab should have a SpriteRenderer!");

		if(boxCheck = (bc != null)) {//boxCollider.enabled = bc != null) {
//			boxCollider.center = bc.center;
//			boxCollider.size = bc.size;
			boxCenter = bc.center;
			boxSize = bc.size;
		}

		if(circleCheck = (cc != null)) {//circleCollider.enabled = cc != null) {
//			circleCollider.center = cc.center;
//			circleCollider.radius = cc.radius;
			circleCenter = cc.center;
			circleRaidus = cc.radius;
		}
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
	public void AddController(ProjectileController controller) {
		controllers.Add (controller);
		controller.OnControllerAdd (this);
	}

	/// <summary>
	/// Removes the controller.
	/// </summary>
	/// <param name="controller">Controller.</param>
	public void RemoveController (ProjectileController controller) {
		if(controllers.Remove(controller))
			controller.OnControllerRemove(this);
	}

	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	public override void Deactivate()  {
		base.Deactivate ();
		properties.Clear ();
		controllers.Clear ();
		linearVelocity = 0f;
		fireTimer = 0f;
		damage = 0;
		angularVelocity = Quaternion.identity;
	}
}
