using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {
	public class ProjectilePool : AbstractPrefabedPoolBehavior<Projectile, ProjectilePrefab> {
		#region implemented abstract members of AbstractPoolBehavior

		protected override Projectile CreateNew () {
			return new Projectile ();
		}

		#endregion

		public override void NormalUpdate () {
			Projectile[] active = Active;
			for(int i = 0; i < active.Length; i++) {
				active[i].Update();
			}
		}

		public void DeactivateAll() {
			Projectile[] active = Active;
			for(int i = 0; i < active.Length; i++) {
				active[i].DeactivateImmediate();
			}
		}
	}
}