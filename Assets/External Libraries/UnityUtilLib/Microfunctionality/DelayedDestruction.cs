using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	
	internal class DelayedDestruction : PausableGameObject {

		public Object target;
		public FrameCounter delay;
		public bool destroySelf = true; 

		public override void NormalUpdate () {
			if (delay.Tick ()) {
				if(target != null) {
					Destroy (target);
				}
				if(destroySelf) {
					Destroy (this);
				}
			}
		}

	}

}
