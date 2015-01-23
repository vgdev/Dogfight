using UnityEngine;
using BaseLib;
using System.Collections;

public class LinearMove : BulletController
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

	public override void UpdateBullet(Bullet bullet, float dt)
	{
		Vector3 forward = bullet.Transform.up;
		bullet.Transform.position += initialVelocity * forward;
	}

	public override void OnControllerAdd (Bullet bullet)
	{
		base.OnControllerAdd (bullet);
		AddNecessaryKey (bullet, "velocity", initialVelocity, overrideVelocity);
	}
}
