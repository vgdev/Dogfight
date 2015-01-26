using UnityEngine;
using System.Collections;

public class TestProjectile : MonoBehaviour {
	public Projectile testBullet;
	public GameObject testPrefab;
	public float vel;
	public float angVel;
	public float resetTime;
	private float time;
	private Vector3 startLoc;
	
	// Use this for initialization
	void Start () {
		testBullet.MatchPrefab (testPrefab);
		startLoc = testBullet.Transform.position;
		time = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		float dt = Time.deltaTime;
		testBullet.Velocity = vel;
		testBullet.AngularVelocity = angVel;
		time += dt;
		if (dt >= resetTime)
			testBullet.Transform.position = startLoc;
	}
}
