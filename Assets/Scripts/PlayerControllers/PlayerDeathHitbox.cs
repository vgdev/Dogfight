using UnityEngine;
using BaseLib;
using System.Collections;

/// <summary>
/// Player death hitbox.
/// </summary>
public class PlayerDeathHitbox : CachedObject {

	private Avatar player;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		player = GetComponentInParent<Avatar> ();
		if (player == null) {
			Debug.LogError("PlayerDeathHitbox should be on a child object of a GameObject with an Avatar sublcass script");
		}
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("Hit");
		if (player != null) {
			player.Hit();
			Projectile proj = other.GetComponent<Projectile>();
			if(proj != null) {
				proj.Deactivate();
			}
		}
	}

	/// <summary>
	/// Raises the bullet collision event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnBulletCollision(ProjectileData other) {
		if (player != null) {
			player.Hit ();
		}
	}
}
