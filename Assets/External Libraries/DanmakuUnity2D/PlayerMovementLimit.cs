using UnityEngine;
using System.Collections;

namespace Danmaku2D {
	[RequireComponent(typeof(ScreenBoundary))]
	public class PlayerMovementLimit : MonoBehaviour {

		[SerializeField]
		private string tagCheck;

		[SerializeField]
		private Vector2 lockedMovementVector;

		void OnTriggerEnter2D(Collider2D other) {
			if(other.CompareTag(tagCheck)) {
				DanmakuPlayer player = other.gameObject.GetComponent<DanmakuPlayer> ();
				if(player != null) {
					player.ForbidMovement(lockedMovementVector);
				}
			}
		}

		void OnTriggerExit2D(Collider2D other) {
			if(other.CompareTag(tagCheck)) {
				DanmakuPlayer player = other.gameObject.GetComponent<DanmakuPlayer> ();
				if(player != null) {
					player.AllowMovement(lockedMovementVector);
				}
			}
		}
	}
}