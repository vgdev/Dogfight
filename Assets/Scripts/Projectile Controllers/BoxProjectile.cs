using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxProjectile : Projectile {
	private BoxCollider2D boxCollider;

	public override void Awake ()
	{
		base.Awake ();
		boxCollider = GetComponent<BoxCollider2D> ();
	}
}
