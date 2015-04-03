using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D.DanmakuControllers {
	public class DelayedAngleChange : IDanmakuController {

		private enum RotationMode { Absolute, Relative, Player }

		[SerializeField]
		private RotationMode rotationMode;

		[SerializeField]
		private float delay;

		[SerializeField]
		private DynamicFloat angle;

		#region implemented abstract members of IDanmakuController

		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			float time = danmaku.Time;
			if(time >= delay && time - dt <= delay) {
				float baseAngle = angle.Value;
				switch(rotationMode) {
					case RotationMode.Relative:
						baseAngle += danmaku.Rotation;
						break;
					case RotationMode.Player:
						baseAngle += danmaku.Field.AngleTowardPlayer(danmaku.Position);
						break;
					case RotationMode.Absolute:
						break;
				}
				danmaku.Rotation = baseAngle;
			}
		}

		#endregion
	}

	namespace Wrapper {

		[AddComponentMenu("Danmaku 2D/Controllers/Delayed Angle Change")]
		public class DelayedAngleChange : ControllerWrapperBehavior<Danmaku2D.DanmakuControllers.DelayedAngleChange> {
		}

	}
}