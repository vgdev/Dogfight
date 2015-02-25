using UnityEngine;
using UnityUtilLib;
using System.Collections;

/// <summary>
/// Player death hitbox.
/// </summary>
public class PlayerDeathHitbox : CachedObject {

	private AbstractPlayableCharacter player;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		player = GetComponentInParent<AbstractPlayableCharacter> ();
		if (player == null) {
			Debug.LogError("PlayerDeathHitbox should be on a child object of a GameObject with an Avatar sublcass script");
		}
	}

	void OnProjectileCollision(Projectile proj) {
		if (player != null) {
			player.Hit (proj);
			proj.Deactivate();
		}
	}
}
