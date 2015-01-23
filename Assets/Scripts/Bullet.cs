using UnityEngine;
using BaseLib;
using System.Collections.Generic;
using System.Collections;

public class Bullet : CachedObject
{
	private BulletController controller;
	private Dictionary<string, object> properties;

	public override void Awake()
	{
		properties = new Dictionary<string, object> ();
	}

	void FixedUpdate()
	{
		if(controller != null)
			controller.UpdateBullet (this, Time.fixedDeltaTime);
	}



	public T GetProperty<T>(string key)
	{
		if(properties.ContainsKey(key))
		{
			return (T)properties[key];
		}
		else
		{
			return default(T);
		}
	}

	public void SetProperty<T>(string key, T value)
	{
		properties [key] = value;
	}

	public bool HasProperty<T>(string key)
	{
		return (properties.ContainsKey(key)) && (properties[key] is T);
	}

	public BulletController Controller
	{
		get { return controller; }
		set 
		{ 
			controller.OnControllerRemove(this);
			controller = value;
			controller.OnControllerAdd(this);
		}
	}

	public void Deactivate()
	{

	}
}
