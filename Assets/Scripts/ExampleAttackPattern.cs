using UnityEngine;
using System.Collections;

/// <summary>
/// An example attack pattern for those trying to make new bullet patterns for new characters or enemies.
/// </summary>
public class ExampleAttackPattern : AbstractAttackPattern {

	/// <summary>
	/// Gets a value indicating whether the current attack pattern execution is finished.
	/// </summary>
	/// <value><c>true</c> if this instance is finished; otherwise, <c>false</c>.</value>
	protected override bool IsFinished {
		get {
			return false;
		}
	}

	
	[SerializeField] // This forces Unity to serialize fireDelay (or any field that's private), it's good programming practice not to use public variables
	private float fireDelay; // Total Time between shots

	private float currentDelay; //how much time is left in this current delay

	[SerializeField]
	private ProjectilePrefab bullet; // the prefab definition of the bullet used by this attack pattern;

	// this is called every time the attack pattern starts
	protected override void OnExecutionStart () {
		currentDelay = fireDelay; // set the initial delay to the defined delay
	}

	// this is the most important part of the attack pattern
	// this is called every time the game as a whole is updated
	// dt is the total amount of time since the last update
	protected override void MainLoop (float dt) {

		currentDelay -= dt; // tick down the delay
		if (currentDelay < 0f) { // if the delay is up
			// fire a bullet, or do something!


			// Fire Linear Bullet is your basic way of shooting bullets. It creates a bullet that travels at a constant speed in a forward direction
			// Argumets are:
			// 1. The ProjectilePrefab that defines what this bullet looks like and its hitbox
			// 2. The Relative Location in the Field that this Attack Pattern targets
			//    0.0f, 0.0f = bottom left, 1.0f, 1.0f = top right
			// 3. Fire Angle, this is the direction that the bullet will fly in, in degrees
			//    0 = Straight up, 90 = to the right, 180 = Straight down, 270 = to the left
			// 4. The Velocity in absolute world coordinates this bullet travles in
			Projectile proj = FireLinearBullet(bullet, new Vector2(0.5f, 0.5f), 0f, 20);

			//It returns a projectile where we can change many of these values
			proj.Velocity = 30;

			//Fire curved bullet is the same as the first one, but it also has an Angular Velocity, which is the degree measure the rotation of the bullet
			// will change by each second
			Projectile proj2 = FireCurvedBullet(bullet, new Vector2(0.5f, 0.5f), 0f, 20, 20);

			//Aso changable after firing
			proj2.AngularVelocity = 30;

			// the AngleToPlayer parameter holds the current value needed to fire directly at the player
			// if the player does not move after this gets fired, they are assured to get hit
			Projectile proj3 = FireLinearBullet(bullet, new Vector2(0.5f, 0.5f), AngleToPlayer, 20);
		}
	}


}
