using UnityEngine;
using System;
using System.Collections.Generic;

namespace BaseLib {
	public abstract class PooledObject : CachedObject {
		private GameObjectPool parentPool;

		private GameObject prefab;
		public GameObject Prefab {
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
				if(is_active != value) {
					is_active = value;
					if(is_active) {
						Deactivate();
						parentPool.Return(this);
					}
					else
						Activate();
					GameObject.SetActive (!is_active);
				}
			}
		}

		public void Initialize(GameObjectPool pool) {
			parentPool = pool;
		}

		public abstract void MatchPrefab (GameObject gameObj);

		protected virtual void Activate() {
		}

		protected virtual void Deactivate() {
		}
	}
}