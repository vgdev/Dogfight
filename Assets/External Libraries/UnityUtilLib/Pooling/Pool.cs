using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtilLib.Pooling {
	public abstract class Pool<T> : IPool<T> where T : IPooledObject {
		private Queue<T> inactiveObjs;
		private HashSet<T> activeObjs;
		private HashSet<T> all;

		private int spawnCount;

		public IEnumerable<T> Active {
			get {
				return activeObjs as IEnumerable<T>;
			}
		}

		public IEnumerable<T> Inactive {
			get {
				return inactiveObjs as IEnumerable<T>;
			}
		}

		public IEnumerable<T> All {
			get {
				return all as IEnumerable<T>;
			}
		}

		public int ActiveCount {
			get {
				return totalCount - ((inactiveObjs == null) ? 0 : inactiveObjs.Count);
			}
		}

		private int InactiveCount {
			get {
				return inactiveObjs.Count;
			}
		}
		
		private int totalCount = 0;
		public int TotalCount {
			get {
				return totalCount;
			}
		}

		public Pool(int initial, int spawn) {
			spawnCount = spawn;
			inactiveObjs = new Queue<T> ();
			activeObjs = new HashSet<T> ();
			all = new HashSet<T> ();
			Spawn (initial);
		}

		public void Return(T po) {
			inactiveObjs.Enqueue (po);
			activeObjs.Remove (po);
			//Debug.Log(activeCount);
		}

		public T Get() {
			if(InactiveCount <= 0) {
				Spawn (spawnCount);
			}
			T po = inactiveObjs.Dequeue();
			activeObjs.Add (po);
			OnGet (po);
			//Debug.Log(active);
			return po;
		}

		protected void Spawn(int count) {
			for(int i = 0; i < count; i++) {
				T newPO = CreateNew();
				newPO.Pool = this;
				inactiveObjs.Enqueue(newPO);
				all.Add(newPO);
				totalCount++;
			}
		}

		protected abstract T CreateNew ();
		protected virtual void OnGet(T obj) {
		}


		#region IPool implementation
		object IPool.Get () {
			throw new System.NotImplementedException ();
		}
		void IPool.Return (object obj) {
			Return ((T)obj);
		}
		#endregion
	}
}