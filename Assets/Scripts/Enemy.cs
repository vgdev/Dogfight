using UnityEngine;
using BaseLib;
using System.Collections;

/// <summary>
/// Enemy.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(FieldMovementPattern))]
public class Enemy : CachedObject {

	/// <summary>
	/// The death reflect radius.
	/// </summary>
	[SerializeField]
	private float deathReflectRadius;

	/// <summary>
	/// The max health.
	/// </summary>
	[SerializeField]
	private int maxHealth;
	private int health;

	private AttackPattern attacks;
	private FieldMovementPattern fmp;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		health = maxHealth;
		EnemyManager.RegisterEnemy (this);
		fmp = GetComponent<FieldMovementPattern> ();
		attacks = GetComponent<AttackPattern> ();
		if (attacks != null) {
			attacks.TargetField = fmp.field;
			attacks.Fire ();
		}
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player Shot")) {
			Projectile proj = other.GetComponent<Projectile>();
			if(proj != null) {
				Hit (proj);
			}
		}
	}

	/// <summary>
	/// Hit the specified proj.
	/// </summary>
	/// <param name="proj">Proj.</param>
	public void Hit(Projectile proj) {
		health -= proj.Damage;
		proj.Active = false;
		if(health <= 0) {
			Die();
		}
	}

	/// <summary>
	/// Die this instance.
	/// </summary>
	private void Die() {
		float radius = Util.MaxComponent3 (Transform.lossyScale) * deathReflectRadius;
		//TODO: FINISH
		EnemyManager.UnregisterEnemy (this);
		Destroy (GameObject);
	}
}
