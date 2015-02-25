using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Projectile boundary.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ProjectileBoundary : MonoBehaviour {

	/// <summary>
	/// The tag filter.
	/// </summary>
	[SerializeField]
	private string tagFilter;

	private List<string> validTags;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		if(tagFilter == null)
			tagFilter = "";
		validTags = new List<string> ();
		validTags.AddRange(tagFilter.Split ('|'));
	}

	void OnProjectileCollision(Projectile proj) {
		if(proj != null) {
			ProcessProjectile(proj);
			proj.Deactivate();
		}
	}

	protected virtual void ProcessProjectile(Projectile proj) {
		proj.Deactivate();
	}
}
