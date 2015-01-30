using UnityEngine;
using System.Collections;

public class EnemyBasicAttack : AttackPattern {

	public float fireDelay;
	public float velocity;
	public float angV;
	private float currentDelay;
	[SerializeField]
	private float generalRange;

	public ProjectilePrefab basicPrefab;

	void Start() {
		currentDelay = fireDelay;
	}

	protected override void MainLoop (float dt) {
		currentDelay -= dt;
		if (currentDelay <= 0f) {
			currentDelay = fireDelay;
			float angle = TargetField.AngleTowardPlayer(transform.position) + Random.Range(-generalRange, generalRange);
			Projectile proj = TargetField.SpawnProjectile(basicPrefab, Transform.position,
			                            angle, 
			                            PlayerFieldController.CoordinateSystem.AbsoluteWorld);
			proj.Velocity = velocity;
			proj.AngularVelocity = angV;
		}
	}
}
