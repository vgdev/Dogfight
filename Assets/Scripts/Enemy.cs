using UnityEngine;
using BaseLib;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(FieldMovementPattern))]
[RequireComponent(typeof(AttackPattern))]
public class Enemy : CachedObject {

	[SerializeField]
	private float deathReflectRadius;

	[SerializeField]
	private float maxHealth;
	private float health;

	private AttackPattern attacks;
	private FieldMovementPattern fmp;

	void Start() {
		health = maxHealth;
		EnemyManager.RegisterEnemy (this);
		attacks = GetComponent<AttackPattern> ();
		fmp = GetComponent<FieldMovementPattern> ();
		attacks.Initialize (fmp.field);
		attacks.Fire ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Player Shot")) {
			Projectile proj = other.GetComponent<Projectile>();
			if(proj != null) {
				Hit (proj);
			}
		}
	}

	public void Hit(Projectile proj) {
		health -= proj.Damage;
		proj.Active = false;
		if(health <= 0) {
			Die();
		}
	}

	private void Die() {
		EnemyManager.UnregisterEnemy (this);
		Destroy (GameObject);
	}
}
