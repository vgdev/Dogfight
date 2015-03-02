using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Danmaku2D {
	public class ProjectileGroup : ICollection<Projectile> {

		private HashSet<Projectile> set;

		private IProjectileGroupController controller;
		public IProjectileGroupController Controller {
			get {
				return controller;
			}
			set {
				controller = value;
				controller.ProjectileGroup = this;
			}
		}

		internal ProjectileGroup() {
			set = new HashSet<Projectile> ();
		}

		internal Vector2 UpdateProjectile(Projectile projectile, float dt) {
			if(Controller != null && Controller != projectile.Controller)
				return Controller.UpdateProjectile(projectile, dt);
			else
				return Vector2.zero;
		}

		#region ICollection implementation

		public void Add (Projectile item) {
			item.Groups.Add (this);
			set.Add (item);
		}

		public void Clear () {
			foreach(Projectile proj in set) {
				proj.Groups.Remove(this);
			}
			set.Clear ();
		}

		public bool Contains (Projectile item) {
			return set.Contains (item);
		}

		public void CopyTo (Projectile[] array, int arrayIndex) {
			set.CopyTo (array, arrayIndex);
		}

		public bool Remove (Projectile item) {
			item.Groups.Remove (this);
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

