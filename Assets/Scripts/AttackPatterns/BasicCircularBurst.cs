using UnityEngine;
using System.Collections;
using BaseLib;

/// <summary>
/// Basic circular burst.
/// </summary>
public class BasicCircularBurst : AttackPattern {

	/// <summary>
	/// The prefab.
	/// </summary>
	[SerializeField]
	private ProjectilePrefab prefab;

	/// <summary>
	/// The spawn location.
	/// </summary>
	[SerializeField]
	private Vector2 spawnLocation;

	/// <summary>
	/// The bullet count.
	/// </summary>
	[SerializeField]
	private int bulletCount;

	/// <summary>
	/// The velocity.
	/// </summary>
	[SerializeField]
	private float velocity;

	/// <summary>
	/// The ang v.
	/// </summary>
	[SerializeField]
	private float angV;

	/// <summary>
	/// The burst count.
	/// </summary>
	[SerializeField]
	private int burstCount;

	/// <summary>
	/// The burst fire delay.
	/// </summary>
	[SerializeField]
	private CountdownDelay burstDelay;

	/// <summary>
	/// The burst initial rotation.
	/// </summary>
	[SerializeField]
	private float burstInitialRotation;

	/// <summary>
	/// The burst rotation delta.
	/// </summary>
	[SerializeField]
	private float burstRotationDelta;

	/// <summary>
	/// Mains the loop.
	/// </summary>
	/// <param name="dt">Dt.</param>
	protected override void MainLoop (float dt) {
		for(int i = 0; i < bulletCount; i++) {
			FireCurvedBullet(prefab, spawnLocation, 360f / (float) bulletCount * (float)i, velocity, angV);
		}
	}
}
