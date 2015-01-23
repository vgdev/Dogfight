using UnityEngine;
using System.Collections;

public class Sequential : BulletController
{
	private bool is_finite;
	public override bool IsFinite
	{
		get { return is_finite; }
	}

	private BulletController[] controllerSet;

	public Sequential(BulletController[] controllers)
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
		is_finite = finite_count >= controllerSet.Length;
	}
}
