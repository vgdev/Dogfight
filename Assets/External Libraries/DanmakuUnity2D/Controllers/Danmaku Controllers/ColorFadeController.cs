using UnityEngine;
using System.Collections;

namespace Danmaku2D.DanmakuControllers {

	public class ColorFadeController : DanmakuControlBehavior {

		[SerializeField]
		private Color endColor;

		[SerializeField]
		private float startTime;

		[SerializeField]
		private float endTime;

		public override void UpdateDanmaku (Danmaku danmaku, float dt) {
			float bulletTime = danmaku.Time;
			Color startColor = danmaku.Prefab.cachedColor;
//			Debug.Log (bulletTime);
			if (bulletTime < startTime)
				danmaku.Color = startColor;
			else if (bulletTime > endTime)
				danmaku.Color = endColor;
			else {
				if(endTime <= startTime)
					danmaku.color = endColor;
				else
					danmaku.Color = Color.Lerp (startColor, endColor, (bulletTime - startTime) / (endTime - startTime));
			}
		}

	}
}