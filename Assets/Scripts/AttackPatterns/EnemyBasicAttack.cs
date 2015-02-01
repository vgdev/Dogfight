using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy basic attack.
/// </summary>
public class EnemyBasicAttack : AttackPattern {

	/// <summary>
	/// The fire delay.
	/// </summary>
	public float fireDelay;

	/// <summary>
	/// The velocity.
	/// </summary>
	public float velocity;

	/// <summary>
	/// The ang v.
	/// </summary>
	public float angV;

	/// <summary>
	/// The current delay.
	/// </summary>
	private float currentDelay;

	/// <summary>
	/// The general range.
	/// </summary>
	[SerializeField]
	private float generalRange;

	/// <summary>
	/// The basic prefab.
	/// </summary>
	public ProjectilePrefab basicPrefab;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		currentDelay = fireDelay;
	}

	/// <summary>
	/// Mains the loop.
	/// </summary>
	/// <param name="dt">Dt.</param>
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
