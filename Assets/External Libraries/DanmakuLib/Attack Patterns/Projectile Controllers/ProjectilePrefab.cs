using UnityEngine;
using System.Collections;
using UnityUtilLib;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ProjectilePrefab : CachedObject {
	[SerializeField]
	private BoxCollider2D boxCollider;

	public BoxCollider2D BoxCollider {
		get {
			return boxCollider;
		}
	}

	[SerializeField]
	private CircleCollider2D circleCollider;

	public CircleCollider2D CircleCollider {
		get {
			return circleCollider;
		}
	}

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public SpriteRenderer SpriteRenderer {
		get {
			return spriteRenderer;
		}
	}

	public override void Awake()
	{
		base.Awake ();
		if(boxCollider == null)
			boxCollider = GetComponent<BoxCollider2D> ();
		if(circleCollider == null)
			circleCollider = GetComponent<CircleCollider2D> ();
		if(spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer> ();
#if UNITY_EDITOR
		if(boxCollider == null && circleCollider == null)
			Debug.Log("Need box or circle collider on projectile prefab");
#endif
	}
}
