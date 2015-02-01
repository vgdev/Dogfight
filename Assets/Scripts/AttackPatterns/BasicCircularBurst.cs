using UnityEngine;
using System.Collections;

/// <summary>
/// Basic circular burst.
/// </summary>
public class BasicCircularBurst : AttackPattern {

	/// <summary>
	/// The prefab.
	/// </summary>
	public ProjectilePrefab prefab;

	/// <summary>
	/// The spawn location.
	/// </summary>
	public Vector2 spawnLocation;

	/// <summary>
	/// The bullet count.
	/// </summary>
	public int bulletCount;

	/// <summary>
	/// The velocity.
	/// </summary>
	public float velocity;

	/// <summary>
	/// The ang v.
	/// </summary>
	public float angV;

	/// <summary>
	/// Mains the loop.
	/// </summary>
	/// <param name="dt">Dt.</param>
	protected override void MainLoop (float dt) {
		for(int i = 0; i < bulletCount; i++) {
			FireCurvedBullet(prefab, spawnLocation, 360f / (float) bulletCount * (float)i, velocity, angV);
		}
		Terminate ();
	}
}
