using System;
using BaseLib;
using UnityEngine;

public class Rotate : ProjectileController
{
	public override bool IsFinite 
	{
		get 
		{
			return false;
		}
	}

	private AbstractDynamicValue<float> initialAngularVelocity;
	private bool overrideAngularVelocity;

	public Rotate (AbstractDynamicValue<float> angV, bool overrideAngV)
	{
		initialAngularVelocity = angV;
		overrideAngularVelocity = overrideAngV;
	}

	public override void UpdateBullet(Projectile bullet, float dt)
	{
		bullet.Transform.Rotate (0f, 0f, bullet.GetProperty<float> ("angular velocity") * dt);
	}

	public override void OnControllerAdd (Projectile bullet)
	{
		base.OnControllerAdd (bullet);
		AddUniversalKey (bullet, "angular velocity", initialAngularVelocity, overrideAngularVelocity);
	}
}