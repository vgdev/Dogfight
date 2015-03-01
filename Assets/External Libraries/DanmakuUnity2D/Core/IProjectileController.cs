using UnityEngine;
using System;
using System.Collections.Generic;

namespace Danmaku2D {
	public interface IProjectileController  {
		ProjectileGroup ProjectileGroup { get; }
		Vector2 UpdateProjectile(Projectile projectile, float dt);
	}

	public abstract class ProjectileController : IProjectileController {

		private ProjectileGroup projectileGroup;
		public ProjectileGroup ProjectileGroup {
			get {
				return projectileGroup;
			}
		}

		public ProjectileController() {
			projectileGroup = new ProjectileGroup ();
		}

		public virtual Vector2 UpdateProjectile(Projectile projectile, float dt) {
			ProjectileGroup.Add (projectile);
			return Vector2.zero;
		}
	}

	public abstract class ProjectileControlBehavior : MonoBehaviour, IProjectileController {

		private ProjectileGroup projectileGroup;
		public ProjectileGroup ProjectileGroup {
			get {
				return projectileGroup;
			}
		}

		public virtual void Awake () {
			projectileGroup = new ProjectileGroup ();
		}

		public virtual Vector2 UpdateProjectile(Projectile projectile, float dt) {
			ProjectileGroup.Add (projectile);
			return Vector2.zero;
		}
	}
}