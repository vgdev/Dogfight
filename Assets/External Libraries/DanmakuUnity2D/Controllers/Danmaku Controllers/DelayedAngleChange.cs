using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.Controllers {

	public class DelayedAngleChange : IDanmakuController {

		private enum RotationMode { Absolute, Relative, Player }

		[SerializeField]
		private RotationMode rotationMode;

		[SerializeField]
		private float delay;

		[SerializeField]
		private DynamicFloat angle;

		public void UpdateProjectile (Danmaku projectile, float dt) {
			float time = projectile.Time;
			if(time >= delay && time - dt <= delay) {
				float baseAngle = angle.Value;
				switch(rotationMode) {
					case RotationMode.Relative:
						baseAngle += projectile.Rotation;
						break;
					case RotationMode.Player:
						baseAngle += projectile.Field.AngleTowardPlayer(projectile.Position);
						break;
					case RotationMode.Absolute:
						break;
				}
				projectile.Rotation = baseAngle;
			}
		}
	}

	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Controllers/Delayed Angle Change")]
		public class DelayedAngleChange : ControllerWrapperBehavior<Danmaku2D.Controllers.DelayedAngleChange> {
		}

	}
}