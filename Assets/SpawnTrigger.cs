using UnityEngine;
using System.Collections;

namespace Danmaku2D.NoScript {
	
	public class SpawnTrigger : DanmakuTrigger {

		[SerializeField]
		private bool DestroyAfter;

		[SerializeField]
		private bool DestroyGameObject;

		void Start() {
			Trigger ();
			if (DestroyAfter)
				Destroy ((DestroyGameObject) ? gameObject as Object : this as Object);
		}

	}
}
