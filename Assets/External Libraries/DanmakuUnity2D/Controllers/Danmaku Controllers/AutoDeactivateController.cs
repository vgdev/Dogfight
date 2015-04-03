using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	[System.Serializable]
	public class AutoDeactivateController : IDanmakuController {
		
		public bool useTime;
		public int frames;

		#region IDanmakuController implementation
		public void UpdateDanmaku (Danmaku danmaku, float dt) {
			if (danmaku.frames > frames) {
				danmaku.Deactivate();
			}
		}
		#endregion
		
	}

}