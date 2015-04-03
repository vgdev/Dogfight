using System;
using UnityEngine;

namespace Danmaku2D {

	public abstract class DanmakuControlBehavior : MonoBehaviour, IDanmakuController {
		
		public DanmakuGroup DanmakuGroup {
			get;
			set;
		}

		public virtual DanmakuController Controller {
			get {
				return UpdateDanmaku;
			}
		}
		
		public virtual void Awake() {
			DanmakuGroup = new DanmakuGroup ();
			DanmakuGroup.AddController(this);
		}

		#region IDanmakuController implementation

		public abstract void UpdateDanmaku (Danmaku danmaku, float dt);

		#endregion
	}
}

