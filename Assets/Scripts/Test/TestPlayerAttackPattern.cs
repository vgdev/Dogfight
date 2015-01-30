using UnityEngine;
using System.Collections;

public class TestPlayerAttackPattern : AttackPattern {

	public ProjectilePrefab prefab;
	public Vector2 spawnLocation;
	public int number;
	public float velocity;
	public float angV;


	protected override void MainLoop (float dt) {
		/*time -= dt;
		if(time < 0f)
		{*/

			for(int i = 0; i < number; i++)
			{
				Projectile bullet = TargetField.SpawnProjectile(prefab, spawnLocation, 360f / (float) number * (float)i);
				bullet.Velocity = velocity;
				bullet.AngularVelocity = angV;
			}
			Deactivate ();
		/*	time = timer;
		}*/
	}	
}
