using UnityEngine;
using System.Collections;

public class Deactivate : ProjectileController
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

	public override void UpdateBullet(Projectile bullet, float dt)
	{
		bullet.Deactivate ();
	}
}
