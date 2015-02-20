using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// Bullet cancel area.
/// </summary>
[RequireComponent(typeof(ProjectileBoundary))]
[RequireComponent(typeof(Collider2D))]
public class BulletCancelArea : CachedObject {

	/// <summary>
	/// Execute this instance.
	/// </summary>
	public IEnumerator Execute(float duration, float maxScale) {
		Vector3 maxScaleV = Vector3.one * maxScale;
		Vector3 startScale = Transform.localScale;
		float t = 0;
		while (t < 1f) {
			Transform.localScale = Vector3.Lerp(startScale, maxScaleV, t);
			yield return new WaitForFixedUpdate();
			t += Time.fixedDeltaTime / duration;
		}
	}
}
