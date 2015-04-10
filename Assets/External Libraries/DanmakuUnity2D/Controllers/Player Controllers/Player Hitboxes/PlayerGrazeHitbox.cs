using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D {
	public class PlayerGrazeHitbox : MonoBehaviour, IDanmakuCollider{

		private DanmakuPlayer player;

		void Start() {
			player = GetComponentInParent<DanmakuPlayer> ();
			if (player == null) {
				Debug.LogError("PlayerGrazeHitbox should be on a child object of a GameObject with an Avatar sublcass script");
			}
		}

		#region IDanmakuCollider implementation
		public void OnDanmakuCollision (Danmaku proj) {
			//throw new System.NotImplementedException ();
		}
		#endregion

		//TODO: FIX
	}
}