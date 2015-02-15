using UnityEngine;
using System.Collections;

/// <summary>
/// Player movement limit.
/// </summary>
[RequireComponent(typeof(ScreenBoundary))]
public class PlayerMovementLimit : MonoBehaviour {

	/// <summary>
	/// The tag check.
	/// </summary>
	[SerializeField]
	private string tagCheck;

	/// <summary>
	/// The locked movement vector.
	/// </summary>
	[SerializeField]
	private Vector2 lockedMovementVector;

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag(tagCheck)) {
			AbstractPlayableCharacter player = other.gameObject.GetComponent<AbstractPlayableCharacter> ();
			if(player != null) {
				player.ForbidMovement(lockedMovementVector);
			}
		}
	}

	/// <summary>
	/// Raises the trigger exit2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit2D(Collider2D other) {
		if(other.CompareTag(tagCheck)) {
			AbstractPlayableCharacter player = other.gameObject.GetComponent<AbstractPlayableCharacter> ();
			if(player != null) {
				player.AllowMovement(lockedMovementVector);
			}
		}
	}
}
