using UnityEngine;
using System;
using System.Collections.Generic;

namespace BaseLib {
	public abstract class PooledObject<T> : CachedObject where T : MonoBehaviour {
		private IPool parentPool;

		private T prefab;
		public T Prefab {
			get { 
				return prefab; 
			}
			set {
				prefab = value;
				MatchPrefab(prefab);
			}
		}

		private bool is_active = false;
		public bool Active {
			get { 
				return Active; 
			}
			set {
				//Debug.Log(value);
				is_active = value;
				if(value){
					Activate();
				} else {
					Deactivate();
					parentPool.Return(this);
				}
				GameObject.SetActive (value);
			}
		}

		void Start() {
			GameObject.SetActive (false);
		}

		public void Initialize(IPool pool) {
			//Debug.Log ("initlaized");
			parentPool = pool;
		}

		public abstract void MatchPrefab (T gameObj);

		protected virtual void Activate() {
		}

		protected virtual void Deactivate() {
		}
	}
}