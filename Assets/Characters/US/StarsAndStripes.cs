using UnityEngine;
using System.Collections;
using UnityUtilLib;

/// <summary>
/// An example attack pattern for those trying to make new bullet patterns for new characters or enemies.
/// </summary>
public class StarsAndStripes : AbstractTimedAttackPattern {

	[SerializeField]
	private CountdownDelay stripesDelay;

	[SerializeField]
	private ProjectilePrefab stripesBullet;

	[SerializeField]
	private int stripeCount;

	[SerializeField]
	private float stripeOffset = 0.1f;

	[SerializeField]
	private float stripeVelocity;

	[SerializeField]
	private bool horizontal;

	[SerializeField]
	private int burstCount;

	[SerializeField]
	private ProjectilePrefab burstBullet;

	[SerializeField]
	private Vector2 burstSpawnLocation = 0.5f * Vector2.one;

	[SerializeField]
	private Vector2 burstSpawnArea;

	[SerializeField]
	private float burstVelocity = 0f;

	// this is called every time the attack pattern starts
	protected override void OnExecutionStart () {
		base.OnExecutionStart ();
		stripesDelay.Reset ();
		Vector2 burstSource = burstSpawnLocation - 0.5f * burstSpawnArea + Util.RandomVect2 (burstSpawnArea);
		for(int i = 0; i < burstCount; i++) {
			FireLinearBullet(burstBullet, burstSource, 360f / (float) burstCount * (float)i, burstVelocity);
		}
	}

	protected override void MainLoop (float dt) {
		base.MainLoop (dt);
		if (stripesDelay.Tick (dt)) {
			Vector2 origin = Vector2.up + stripeOffset * ((horizontal) ? -Vector2.up : Vector2.right);
			Vector2 dif = (1f - 2 * stripeOffset) / ((float) stripeCount - 1) * ((horizontal) ? -Vector2.up : Vector2.right);
			for(int i = 0; i < stripeCount; i++) {
				FireLinearBullet(stripesBullet, origin + (i * dif), (horizontal) ? 270f : 180f, stripeVelocity);
			}
		}
	}
}

public class RedirectAtPLayer : ProjectileController {

	public override bool IsFinite {
		get {
			return true;
		}
	}

	public override void UpdateBullet (Projectile bullet, float dt) {
		base.UpdateBullet (bullet, dt);
	}


}
