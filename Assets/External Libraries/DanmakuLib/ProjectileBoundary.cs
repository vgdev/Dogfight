using UnityEngine;
using System.Collections;

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

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake() {
		if(tagFilter == null)
			tagFilter = "";
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Entered");
		if(other.CompareTag(tagFilter)) {
			Projectile proj = other.GetComponent<Projectile>();
			if(proj != null) {
				proj.Deactivate();
			}
		}
	}
}
