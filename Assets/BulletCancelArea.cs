using UnityEngine;
using System.Collections;
using BaseLib;

/// <summary>
/// Bullet cancel area.
/// </summary>
[RequireComponent(typeof(ProjectileBoundary))]
[RequireComponent(typeof(Collider2D))]
public class BulletCancelArea : CachedObject {

	//TODO: Finish this class

	/// <summary>
	/// The max scale.
	/// </summary>
	[SerializeField]
	private float maxScale = 8f;
	public float MaxScale {
		get {
			return maxScale;
		}
		set {
			maxScale = value;
		}
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		StartCoroutine (Execute ());
	}

	/// <summary>
	/// Execute this instance.
	/// </summary>
	private IEnumerator Execute() {
		throw new System.NotImplementedException();
	}
}
