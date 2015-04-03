using UnityEngine;
using UnityUtilLib;
using System.Collections;

namespace Danmaku2D {
	public class PlayerDeathHitbox : MonoBehaviour, IDanmakuCollider {

		private DanmakuPlayer player;

		void Start() {
			player = GetComponentInParent<DanmakuPlayer> ();
			if (player == null) {
				Debug.LogError("PlayerDeathHitbox should be on a child object of a GameObject with an Avatar sublcass script");
			}
		}

		public void OnDanmakuCollision(Danmaku danmaku) {
			if (player != null) {
				player.Hit (danmaku);
				danmaku.Deactivate();
			}
		}
	}
}