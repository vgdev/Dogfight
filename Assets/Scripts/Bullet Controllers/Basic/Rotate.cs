using System;
using BaseLib;
using UnityEngine;

public class Rotate : BulletController
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

	public override void UpdateBullet(Bullet bullet, float dt)
	{
		bullet.Transform.Rotate (0f, 0f, bullet.GetProperty<float> ("angular velocity") * dt);
	}

	public override void OnControllerAdd (Bullet bullet)
	{
		base.OnControllerAdd (bullet);
		AddNecessaryKey (bullet, "angular velocity", initialAngularVelocity, overrideAngularVelocity);
	}
}