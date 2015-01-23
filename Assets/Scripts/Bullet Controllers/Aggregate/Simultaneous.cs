using UnityEngine;
using BaseLib;
using System.Collections;

public class Simultaneous : BulletController 
{
	private bool is_finite;
	public override bool IsFinite
	{
		get { return is_finite; }
	}

	private BulletController[] controllerSet;

	public Simultaneous(BulletController[] controllers, bool waitForAll)
	{
		controllerSet = controllers;
		int finite_count = 0;
		for(int i = 0; i < controllerSet.Length; i++)
		{
			if(controllerSet[i].IsFinite)
			{
				finite_count++;
			}
		}
		is_finite = (!waitForAll && finite_count >= 1) || (waitForAll && finite_count >= controllerSet.Length);
	}

	public override void UpdateBullet (Bullet bullet, float dt)
	{
		for(int i = 0; i < controllerSet.Length; i++)
		{
			controllerSet[i].UpdateBullet(bullet, dt);
		}
	}

	public override void OnControllerAdd (Bullet bullet)
	{
		base.OnControllerAdd (bullet);
		for(int i = 0; i < controllerSet.Length; i++)
		{
			controllerSet[i].OnControllerAdd(bullet);
		}
	}
}
