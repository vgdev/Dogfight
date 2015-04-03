using UnityEngine;

namespace Danmaku2D.Controllers {

	[System.Serializable]
	public class AnimationCurveController : IDanmakuController {

		[SerializeField]
		private AnimationCurve velocityCurve;

		#region IProjectileController implementation
		public virtual void UpdateProjectile (Danmaku projectile, float dt) {
			float velocity = velocityCurve.Evaluate (projectile.Time);
			if (velocity != 0)
				projectile.Position += projectile.Direction * velocity * dt;
		}
		#endregion
	}

	namespace Wrapper {
		
		[AddComponentMenu("Danmaku 2D/Controllers/Animation Curve Controller")]
		internal class AnimationCurveController : ControllerWrapperBehavior<AnimationCurveController> {
		}

	}
}

