using UnityEngine;
using System.Collections;
using Danmaku2D;
using Danmaku2D.Phantasmagoria;

public class InvertOtherScreen : TimedAttackPattern {

	[SerializeField]
	private float flipDuration;

	protected override void OnInitialize () {
		base.OnInitialize ();
		StartCoroutine (FlipScreen ());
	}

	protected override void OnFinalize () {
		StartCoroutine (FlipScreen ());
	}

	private IEnumerator FlipScreen() {
		float rot = TargetField.Camera2DRotation;
		float altRot = rot + 180f;
		float t = 0;
		Time.timeScale = 0f;
		while (t < 1f) {
			TargetField.Camera2DRotation = Mathf.Lerp(rot, altRot, t);
			yield return new WaitForEndOfFrame();
			t += Time.unscaledDeltaTime / flipDuration;
		}
		TargetField.Camera2DRotation = altRot;
		Time.timeScale = 1f;

	}
}
