using UnityEngine;
using System.Collections;
using UnityUtilLib;
using UnityUtilLib.Pooling;

namespace Danmaku2D {
	public class ProjectileManager : SingletonBehavior<ProjectileManager>, IPausable {

		private static BasicPool<Projectile> projectilePool;

		[SerializeField]
		private int initialCount = 1000;

		[SerializeField]
		private int spawnOnEmpty = 1000;

		public override void Awake () {
			base.Awake ();
			projectilePool = new BasicPool<Projectile> (initialCount, spawnOnEmpty);
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
			Projectile[] active = projectilePool.Active;
			for(int i = 0; i < active.Length; i++) {
				active[i].Update();
			}
		}

		public static void DeactivateAll() {
			Projectile[] active = projectilePool.Active;
			for(int i = 0; i < active.Length; i++) {
				active[i].DeactivateImmediate();
			}
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