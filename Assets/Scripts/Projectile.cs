using UnityEngine;
using BaseLib;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : PooledObject {

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

	private BoxCollider2D boxCollider;
	private CircleCollider2D circleCollider;
	private SpriteRenderer spriteRenderer;

	public override void Awake() {
		properties = new Dictionary<string, object> ();
		controllers = new List<ProjectileController> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		circleCollider = GetComponent<CircleCollider2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void FixedUpdate() {
		float dt = Time.fixedTime;

		//Rotate
		Transform.rotation *= Quaternion.Slerp (Transform.rotation, Transform.rotation * angularVelocity, dt);
		//Translate
		Transform.position += linearVelocity * Transform.up * dt;


		for(int i = 0; i < controllers.Count; i++)
			if(controllers[i] != null)
				controllers[i].UpdateBullet(this, dt);
	}

	public override void MatchPrefab(GameObject prefab) {
		BoxCollider2D boxTest = prefab.GetComponent<BoxCollider2D>();
		CircleCollider2D circleTest = prefab.GetComponent<CircleCollider2D>();
		SpriteRenderer spriteTest = prefab.GetComponent<SpriteRenderer>();

		Transform.localScale = prefab.transform.localScale;
		gameObject.tag = prefab.tag;
		gameObject.layer = prefab.layer;

		if(spriteRenderer != null) {
			spriteRenderer.sprite = spriteTest.sprite;
			spriteRenderer.color = spriteTest.color;
			spriteRenderer.material = spriteTest.material;
			spriteRenderer.sortingOrder = spriteTest.sortingOrder;
			spriteRenderer.sortingLayerID = spriteTest.sortingLayerID;
		}
		else
			Debug.LogError("The provided prefab should have a SpriteRenderer!");

		if(boxCollider.enabled = boxTest != null) {
			boxCollider.center = boxTest.center;
			boxCollider.size = boxTest.size;
		}

		if(circleCollider.enabled = circleTest != null) {
			circleCollider.center = circleTest.center;
			circleCollider.radius = circleTest.radius;
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
