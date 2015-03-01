using System;
using System.Collections;
using System.Collections.Generic;

namespace Danmaku2D {
	public class ProjectileGroup : ICollection<Projectile> {

		private HashSet<Projectile> set;

		#region ICollection implementation

		public void Add (Projectile item) {
			set.Add (item);
		}

		public void Clear () {
			set.Clear ();
		}

		public bool Contains (Projectile item) {
			return set.Contains (item);
		}

		public void CopyTo (Projectile[] array, int arrayIndex) {
			set.CopyTo (array, arrayIndex);
		}

		public bool Remove (Projectile item) {
			return set.Remove (item);
		}

		public int Count {
			get {
				return set.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<Projectile> GetEnumerator () {
			return set.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator () {
			return set.GetEnumerator ();
		}

		#endregion




	}
}

