using UnityEngine;
using System;
using System.Collections.Generic;

namespace BaseLib {
	public class GameObjectPool : CachedObject {
		private Queue<PooledObject> inactive;
		private bool valid = true;
		[SerializeField]
		private int initialSpawnCount;
		[SerializeField]
		private int spawnCount;
		[SerializeField]
		private GameObject basePrefab;
		[SerializeField]
		private GameObject container;
		
		public override void Awake() {
			base.Awake ();
			PooledObject[] po = basePrefab.GetComponents<PooledObject> ();
			if(po == null || po.Length <= 0) {
				Debug.LogError("The provided prefab must have a subclass of PooledObject attached");
				valid = false;
			} else {
				inactive = new Queue<PooledObject>();
				Spawn (initialSpawnCount);
			}
		}
		
		public void Return(PooledObject po) {
			if(valid) {
				po.Active = false;
				inactive.Enqueue (po);
			}
		}
		
		public PooledObject Get(GameObject prefab = null) {
			if(valid) {
				if(inactive.Count <= 0)
					Spawn (spawnCount);
				PooledObject po = inactive.Dequeue ();
				if(prefab != null)
					po.MatchPrefab(prefab);
				return po;
			}
			return null;
		}
		
		private void Spawn(int count) {
			Transform parentTrans = container.transform;
			for(int i = 0; i < count; i++) {
				PooledObject newPO = ((GameObject)Instantiate(basePrefab)).GetComponent<PooledObject>();
				newPO.Transform.parent = parentTrans;
				inactive.Enqueue(newPO);
			}
		}
	}
}