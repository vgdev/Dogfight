using UnityEngine;
using System.Collections;
using BaseLib;

[RequireComponent(typeof(ProjectileBoundary))]
[RequireComponent(typeof(Collider2D))]
public class BulletCancelArea : CachedObject {

	//TODO: Finish this class


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

	void Start() {
		StartCoroutine (Execute ());
	}

	private IEnumerator Execute() {
		throw new System.NotImplementedException();
	}
}
