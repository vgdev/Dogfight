using UnityEngine;
using System;
using System.Collections.Generic;

namespace BaseLib {
	public interface IPool {
		void Return (object obj);
	}

	public class GameObjectPool<T, P> : CachedObject, IPool where T : PooledObject<P> where P : MonoBehaviour{
		private Queue<T> inactive;
		private bool valid = true;
		[SerializeField]
		private int initialSpawnCount;
		[SerializeField]
		private int spawnCount;
		[SerializeField]
		private GameObject basePrefab;
		[SerializeField]
		private GameObject container;
		private int active = 0;
		
		public override void Awake() {
			base.Awake ();
			T[] po = basePrefab.GetComponents<T> ();
			if(po == null || po.Length <= 0) {
				Debug.LogError("The provided prefab must have a subclass of PooledObject attached");
				valid = false;
			} else {
				inactive = new Queue<T>();
				Spawn (initialSpawnCount);
			}
		}
		
		public void Return(T po) {
			if(valid) {
				po.Active = false;
				inactive.Enqueue (po);
				active--;
				Debug.Log(active);

			}
		}

		public void Return (object obj)
		{
			Return ((T)obj);
		}
		
		public PooledObject<P> Get(P prefab = null) {
			if(valid) {
				if(inactive.Count <= 0)
					Spawn (spawnCount);
				T po = inactive.Dequeue ();
				if(prefab != default(P))
					po.Prefab = prefab;
				active++;
				Debug.Log(active);
				return po;
			}
			return null;
		}
		
		private void Spawn(int count) {
			Transform parentTrans = container.transform;
			for(int i = 0; i < count; i++) {
				T newPO = ((GameObject)Instantiate(basePrefab)).GetComponent<T>();
				newPO.Transform.parent = parentTrans;
				newPO.Initialize(this);
				inactive.Enqueue(newPO);
			}
		}
	}
}