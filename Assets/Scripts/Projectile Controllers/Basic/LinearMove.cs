using UnityEngine;
using BaseLib;
using System.Collections;

public class LinearMove : ProjectileController
{
	public override bool IsFinite
	{
		get { return false; }
	}

	private AbstractDynamicValue<float> initialVelocity;
	private bool overrideVelocity;

	public LinearMove(AbstractDynamicValue<float> initialVelocity, bool overrideVelocity = false)
	{
		this.initialVelocity = initialVelocity;
		this.overrideVelocity = overrideVelocity;
	}

	public override void UpdateBullet(Projectile bullet, float dt)
	{
		Vector3 forward = bullet.Transform.up;
		bullet.Transform.position += initialVelocity * forward;
	}

	public override void OnControllerAdd (Projectile bullet)
	{
		base.OnControllerAdd (bullet);
		AddUniversalKey (bullet, "velocity", initialVelocity, overrideVelocity);
	}
}
