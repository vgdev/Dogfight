using UnityEngine;
using BaseLib;
using System.Collections;

public class PlayerDeathHitbox : CachedObject {

	private Avatar player;
	
	void Start() {
		player = Transform.parent.GetComponent<Avatar> ();
		if (player == null) {
			Debug.LogError("PlayerDeathHitbox should be on a child object of a GameObject with an Avatar sublcass script");
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (player != null) {
			player.Hit();
		}
	}
	
	void OnBulletCollision(ProjectileData other) {
		if (player != null) {
			player.Hit ();
		}
	}
}
