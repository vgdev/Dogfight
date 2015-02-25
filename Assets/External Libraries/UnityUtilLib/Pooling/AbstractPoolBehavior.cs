using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtilLib {
	public abstract class AbstractPoolBehavior<T> : CachedObject, IPool<T> where T : IPooledObject {
		private Queue<T> inactive;
		private HashSet<T> active;
		private HashSet<T> all;

		/// <summary>
		/// The initial spawn count.
		/// </summary>
		[SerializeField]
		private int initialSpawnCount;
		
		/// <summary>
		/// The spawn count.
		/// </summary>
		[SerializeField]
		private int spawnCount;

		public T[] Active {
			get {
				T[] array = new T[active.Count];
				active.CopyTo(array);
				return array;
			}
		}

		public T[] Inactive {
			get {
				return inactive.ToArray();
			}
		}

		public T[] All {
			get {
				T[] array = new T[all.Count];
				all.CopyTo(array);
				return array;
			}
		}

		public int ActiveCount {
			get {
				try {
					return totalCount - inactive.Count;
				} catch (System.NullReferenceException nre) {
					return totalCount;
				}
			}
		}

		private int InactiveCount {
			get {
				return inactive.Count;
			}
		}
		
		private int totalCount = 0;
		public int TotalCount {
			get {
				return totalCount;
			}
		}

		public override void Awake () {
			base.Awake ();
			inactive = new Queue<T> ();
			active = new HashSet<T> ();
			all = new HashSet<T> ();
			Spawn (initialSpawnCount);
		}

		/// <summary>
		/// Return the specified po.
		/// </summary>
		/// <param name="po">Po.</param>
		public void Return(T po) {
			inactive.Enqueue (po);
			active.Remove (po);
			//Debug.Log(activeCount);
		}
		
		/// <summary>
		/// Get the specified prefab.
		/// </summary>
		/// <param name="prefab">Prefab.</param>
		public T Get() {
			if(InactiveCount <= 0) {
				Spawn (spawnCount);
			}
			T po = inactive.Dequeue();
			active.Add (po);
			OnGet (po);
			//Debug.Log(active);
			return po;
		}
		
		/// <summary>
		/// Spawn the specified count.
		/// </summary>
		/// <param name="count">Count.</param>
		protected void Spawn(int count) {
			for(int i = 0; i < count; i++) {
				T newPO = CreateNew();
				newPO.Pool = this;
				inactive.Enqueue(newPO);
				all.Add(newPO);
				OnSpawn(newPO);
				totalCount++;
			}
		}

		protected abstract T CreateNew ();
		protected virtual void OnGet(T obj) {
		}
		protected virtual void OnSpawn(T obj) {
		}

		#region IPool implementation
	
		object IPool.Get () {
			return Get ();
		}

		/// <summary>
		/// Return the specified obj.
		/// </summary>
		/// <param name="obj">Object.</param>
		void IPool.Return (object obj) {
			Return ((T)obj);
		}
		#endregion
	}
}