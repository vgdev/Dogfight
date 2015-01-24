using UnityEngine;
using System.Collections;

public class Sequential : ProjectileController
{
	private bool is_finite;
	public override bool IsFinite
	{
		get { return is_finite; }
	}

	private ProjectileController[] controllerSet;
	private string indexKey;

	public Sequential(ProjectileController[] controllers)
	{
		indexKey = UniqueKey ("index");
		controllerSet = controllers;
		int finite_count = 0;
		for(int i = 0; i < controllerSet.Length; i++)
		{
			if(controllerSet[i].IsFinite)
			{
				finite_count++;
			}
		}
		is_finite = finite_count >= controllerSet.Length;
	}

	public override void UpdateBullet(Projectile bullet, float dt)
	{
		int currentIndex = bullet.GetProperty<int> (indexKey);
		controllerSet [currentIndex].UpdateBullet (bullet, dt);
		if(controllerSet[currentIndex].CheckDone(bullet))
			bullet.SetProperty<int>(indexKey, currentIndex + 1);
	}
}
