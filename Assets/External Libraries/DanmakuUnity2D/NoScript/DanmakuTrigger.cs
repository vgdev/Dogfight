using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	public abstract class DanmakuTriggerReciever : CachedObject {

		[SerializeField]
		private DanmakuTrigger[] triggers;

		public override void Awake () {
			base.Awake ();
			for(int i = 0; i < triggers.Length; i++) {
				if(triggers[i] != null) {
					triggers[i].triggerCallback += Trigger;
				}
			}
		}

		public void OnDestroy() {
			for(int i = 0; i < triggers.Length; i++) {
				if(triggers[i] != null) {
					triggers[i].triggerCallback -= Trigger;
				}
			}
		}

		public abstract void Trigger ();
	}

	[AddComponentMenu("Danmaku 2D/Danmaku Trigger")]
	public class DanmakuTrigger : CachedObject {

		public delegate void TriggerCallback ();
		
		internal TriggerCallback triggerCallback;
		
		public void Trigger() {
			if(triggerCallback != null)
				triggerCallback();
		}

	}

}
