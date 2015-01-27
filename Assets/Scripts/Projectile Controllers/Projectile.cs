using UnityEngine;
using BaseLib;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : PooledObject<ProjectilePrefab> {

	private float linearVelocity = 0f;
	public float Velocity
	{
		get {
			return linearVelocity;
		}
		set {
			linearVelocity = value;
		}
	}

	private Quaternion angularVelocity = Quaternion.identity;
	public float AngularVelocity
	{
		get {
			return angularVelocity.eulerAngles.z;
		}
		set {
			angularVelocity = Quaternion.Euler(new Vector3(0f, 0f, value));
		}
	}

	public float AngularVelocityRadians
	{
		get {
			return AngularVelocity * Util.Degree2Rad;
		}
		set {
			AngularVelocity = value * Util.Rad2Degree;
		}
	}

	private List<ProjectileController> controllers;
	private Dictionary<string, object> properties;

	[SerializeField]
	private CircleCollider2D circleCollider;
	[SerializeField]
	private BoxCollider2D boxCollider;
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public override void Awake() {
		properties = new Dictionary<string, object> ();
		controllers = new List<ProjectileController> ();
		if(boxCollider == null)
			boxCollider = GetComponent<BoxCollider2D> ();
		if(circleCollider == null)
			circleCollider = GetComponent<CircleCollider2D> ();
		if(spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void FixedUpdate() {
		float dt = Time.fixedDeltaTime;

		//Rotate
		Transform.rotation = Quaternion.Slerp (Transform.rotation, Transform.rotation * angularVelocity, dt);
		//Translate
		Transform.position += linearVelocity * Transform.up * dt;

		for(int i = 0; i < controllers.Count; i++)
			if(controllers[i] != null)
				controllers[i].UpdateBullet(this, dt);
	}

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

		if(boxCollider.enabled = bc != null) {
			boxCollider.center = bc.center;
			boxCollider.size = bc.size;
		}

		if(circleCollider.enabled = cc != null) {
			circleCollider.center = cc.center;
			circleCollider.radius = cc.radius;
		}
	}

	public bool HasProperty<T>(string key) {
		return (properties.ContainsKey(key)) && (properties[key] is T);
	}
	
	public T GetProperty<T>(string key) {
		if(properties.ContainsKey(key))
			return (T)properties[key];
		else 
			return default(T);
	}
	
	public void SetProperty<T>(string key, T value) {
		properties [key] = value;
	}

	public void AddController(ProjectileController controller) {
		controllers.Add (controller);
		controller.OnControllerAdd (this);
	}

	public void RemoveController (ProjectileController controller) {
		if(controllers.Remove(controller))
			controller.OnControllerRemove(this);
	}

	protected override void Deactivate() {
		properties.Clear ();
		controllers.Clear ();
	}
}
