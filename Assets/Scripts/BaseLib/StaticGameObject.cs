using UnityEngine;
using System;

namespace BaseLib {
	[Serializable]
	public abstract class StaticGameObject<T> : CachedObject where T : StaticGameObject<T>
	{
		private static T instance;
		
		public static T Instance
		{
			get 
			{ 
				if(instance == null)
				{
					instance = FindObjectOfType<T>();
				}
				return instance; 
			}
		}
		
		public bool keepBetweenScenes;
		public bool destroyNewInstances;
		
		public override void Awake ()
		{
			base.Awake ();
			if(instance != null)
			{
				if(instance.destroyNewInstances)
				{
					Destroy (gameObject);
					return;
				}
				else
				{
					Destroy (instance.GameObject);
				}
			}
			
			instance = (T)this;
			
			if(keepBetweenScenes)
			{
				DontDestroyOnLoad (gameObject);
			}
		}
	}
}