using UnityEngine;
using System.Collections;

public class Deactivate : BulletController
{
	public override bool IsFinite 
	{
		get 
		{
			return true;
		}
	}

	public Deactivate()
	{
	}

	public override void UpdateBullet(Bullet bullet, float dt)
	{
		bullet.Deactivate ();
	}
}
