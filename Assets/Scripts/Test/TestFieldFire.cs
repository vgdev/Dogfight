using UnityEngine;
using System.Collections;

public class TestFieldFire : TestScript {
	public PlayerFieldController field;
	public ProjectilePrefab prefab;
	public Vector2 spawnLocation;
	public int number;
	public float timer;
	private float time = 0f;
	public float velocity;
	public float angV;

	void FixedUpdate()
	{
		float dt = Time.fixedDeltaTime;
		time -= dt;
		if(time < 0f)
		{
			for(int i = 0; i < number; i++)
			{
				Quaternion rot = Quaternion.Euler(0f, 0f, 360f / (float) number * (float)i);
				Projectile bullet = field.SpawnProjectile(prefab, spawnLocation, rot);
				bullet.Velocity = velocity;
				bullet.AngularVelocity = angV;
			}
			time = timer;
		}
	}
}
