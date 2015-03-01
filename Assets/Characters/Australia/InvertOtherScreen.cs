using UnityEngine;
using System.Collections;
using Danmaku2D;
using Danmaku2D.Phantasmagoria;

public class InvertOtherScreen : TimedAttackPattern {

	[SerializeField]
	private float flipDuration;

	protected override void OnExecutionStart () {
		base.OnExecutionStart ();
		StartCoroutine (FlipScreen ());
	}

	protected override void OnExecutionFinish () {
		StartCoroutine (FlipScreen ());
	}

	private IEnumerator FlipScreen() {
		Quaternion rot = TargetField.CameraTransform.rotation;
		Vector3 euler = rot.eulerAngles;
		euler.z += 180f;
		Quaternion altRot = Quaternion.Euler (euler);
		float t = 0;
		Time.timeScale = 0f;
		while (t < 1f) {
			TargetField.CameraTransform.rotation = Quaternion.Slerp(rot, altRot, t);
			yield return new WaitForEndOfFrame();
			t += Time.unscaledDeltaTime / flipDuration;
		}
		TargetField.CameraTransform.rotation = altRot;
		Time.timeScale = 1f;

	}
}
