using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class BulletController 
{
	private string unique_id;
	public string ID
	{
		get 
		{
			if(unique_id == null)
			{
				unique_id = this.GetType().ToString() + "_" + Guid.NewGuid().ToString();
				Debug.Log(unique_id);
			}
			return unique_id;
		}
	}

	public abstract bool IsFinite { get; }

	public virtual void UpdateBullet (Bullet bullet, float dt)
	{
	}

	public virtual void OnCollision(Bullet bullet, Collider2D other)
	{
	}

	public virtual void OnControllerAdd(Bullet bullet)
	{
	}

	public virtual void OnControllerRemove(Bullet bullet)
	{
	}

	public virtual bool CheckDone(Bullet bullet)
	{
		return false;
	}



	protected void AddNecessaryKey<T>(Bullet bullet, string key, T value, bool overrideOld)
	{
		if(overrideOld || !bullet.HasProperty<T>(key))
		{
			bullet.SetProperty<T>(key, value);
		}
	}
}
