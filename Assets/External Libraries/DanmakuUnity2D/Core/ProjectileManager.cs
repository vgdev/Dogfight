using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityUtilLib;
using UnityUtilLib.Pooling;

namespace Danmaku2D {
	public class ProjectileManager : SingletonBehavior<ProjectileManager>, IPausable {

		private static BasicPool<Projectile> projectilePool;
		private static HashSet<Projectile> toReturn;

		[SerializeField]
		private int initialCount = 1000;

		[SerializeField]
		private int spawnOnEmpty = 1000;

		public void Start () {
			if(projectilePool == null) {
				projectilePool = new BasicPool<Projectile> (initialCount, spawnOnEmpty);
				toReturn = new HashSet<Projectile>();
			}
		}

		public int TotalCount {
			get {
				return (projectilePool != null) ? projectilePool.TotalCount : 0;
			}
		}

		public int ActiveCount {
			get {
				return (projectilePool != null) ? projectilePool.ActiveCount : 0;
			}
		}

		public void Update() {
			if (!Paused)
				NormalUpdate ();
		}

		public virtual void NormalUpdate () {
			foreach(Projectile proj in projectilePool.Active) {
				proj.Update();
			}
			foreach (Projectile proj in toReturn) {
				projectilePool.Return(proj);
			}
			toReturn.Clear ();
		}

		public static void DeactivateAll() {
			foreach(Projectile proj in projectilePool.Active) {
				proj.DeactivateImmediate();
			}
		}

		internal static void Return(Projectile proj) {
			toReturn.Add (proj);
		}

		internal static Projectile Get (ProjectilePrefab projectileType) {
			Projectile proj = projectilePool.Get ();
			proj.MatchPrefab (projectileType);
			return proj;
		}

		#region IPausable implementation

		public bool Paused {
			get;
			set;
		}

		#endregion
	}
}