using UnityEngine;
using System.Collections;

public class TestProjectile : TestScript {
	public Projectile testBullet;
	public ProjectilePrefab testPrefab;
	public float vel;
	public float angVel;
	public float resetTime;
	private float time;
	private Vector3 startLoc;

	void Start () {
		testBullet.Prefab = testPrefab;
		startLoc = testBullet.Transform.position;
		time = 0f;
	}

	void Update () {
		float dt = Time.deltaTime;
		testBullet.Velocity = vel;
		testBullet.AngularVelocity = angVel;
		time += dt;
		Debug.Log (time);
		if (time >= resetTime)
		{
			testBullet.Transform.position = startLoc;
			time = 0f;
		}
	}
}
