using UnityEngine;

namespace UnityUtilLib {
	[RequireComponent(typeof(Camera))]
	public class FixedScreenAreaCamera : MonoBehaviour {

		private Camera camera;
		private float currentAspectRatio;
		private float offset;

		[SerializeField]
		private float anchorPoint = 0.5f;

		[SerializeField]
		private float nativeAspectRatio;

		[SerializeField]
		private Rect nativeBounds;

		void Awake() {
			camera = GetComponent<Camera> ();
			currentAspectRatio = (float)Screen.width / (float)Screen.height;
			offset = nativeBounds.x + 0.5f * nativeBounds.width - anchorPoint;
			Resize ();
		}

		void Update() {
			float newAspectRatio = (float)Screen.width / (float)Screen.height;
			if (currentAspectRatio != newAspectRatio) {
				currentAspectRatio = newAspectRatio;
				Resize ();
			}
		}

		private void Resize() {
			float changeRatio = nativeAspectRatio / currentAspectRatio;
			float targetWidth = changeRatio * nativeBounds.width;
			float center = anchorPoint + changeRatio * offset;
			Rect cameraRect = camera.rect;
			cameraRect.x = center - targetWidth / 2;
			cameraRect.width = targetWidth;
			camera.rect = cameraRect;
		}
	}
}