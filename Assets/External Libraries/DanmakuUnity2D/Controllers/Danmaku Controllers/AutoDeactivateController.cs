using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.Controllers {

	[System.Serializable]
	public class AutoDeactivateController : IDanmakuController {

		[SerializeField]
		private bool useTime;

		[SerializeField]
		private int frames;

		public int Frames {
			get {
				return frames;
			}
			set {
				frames = value;
			}
		}

		public float Time {
			get {
				return Util.FramesToTime(frames);
			}
			set {
				frames = Util.TimeToFrames(value);
			}
		}

		public AutoDeactivateController () {
			frames = -1;
		}

		public AutoDeactivateController(int frames) {
			this.frames = frames;
		}

		public AutoDeactivateController(float time) {
			frames = Util.TimeToFrames (time);
		}

		#region IProjectileController implementation
		public void UpdateProjectile (Danmaku projectile, float dt) {
			if (projectile.frames > frames && frames >= 0) {
				projectile.Deactivate();
			}
		}
		#endregion
		
	}

	namespace Wrapper {
		
		[AddComponentMenu("Danmaku 2D/Controllers/Auto Deactivate Controller")]
		internal class AutoDeactivateController : ControllerWrapperBehavior<Danmaku2D.Controllers.AutoDeactivateController> {
		}

	}

}