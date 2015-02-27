using UnityEngine;
using System.Collections;

namespace UnityUtilLib {
	[RequireComponent(typeof(StaticGameObject))]
	public abstract class PausableSingleton<T> : PausableGameObject where T : PausableSingleton<T> {
		private static T instance;
		
		public static T Instance {
			get { 
				if(instance == null) {
					instance = FindObjectOfType<T>();
				}
				return instance; 
			}
		}
		
		public bool destroyNewInstances;
		
		public override void Awake () {
			base.Awake ();
			if(instance != null) {
				if(instance.destroyNewInstances) {
					Destroy (gameObject);
					return;
				} else {
					Destroy (instance.GameObject);
				}
			}
			
			instance = (T)this;
		}
	}
}