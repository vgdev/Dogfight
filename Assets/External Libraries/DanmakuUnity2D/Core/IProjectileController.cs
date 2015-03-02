using UnityEngine;
using System;
using System.Collections.Generic;

namespace Danmaku2D {

	public interface IProjectileController {
		Projectile Projectile { get; }
		Vector2 UpdateProjectile (Projectile projectile, float dt);
	}

	public interface IProjectileGroupController {
		ProjectileGroup ProjectileGroup { get; set; }
		Vector2 UpdateProjectile (Projectile projectile, float dt);
	}

	public abstract class ProjectileController : IProjectileController {

		private Projectile projectile;

		#region IProjectileController implementation

		public virtual Vector2 UpdateProjectile (Projectile projectile, float dt) {
			this.projectile = projectile;
			return Vector2.zero;
		}

		public Projectile Projectile {
			get {
				return projectile;
			}
		}

		#endregion
	}

	[RequireComponent(typeof(ProjectilePrefab))]
	public abstract class ProjectileControlBehavior : MonoBehaviour, IProjectileGroupController {

		private SpriteRenderer spriteRenderer;
		public SpriteRenderer SpriteRenderer {
			get {
				if(spriteRenderer == null)
					spriteRenderer = (SpriteRenderer)renderer;
				return spriteRenderer;
			}
		}

		private CircleCollider2D circleColldier;
		public CircleCollider2D CircleCollider {
			get {
				if(circleColldier == null)
					circleColldier = (CircleCollider2D)collider2D;
				return circleColldier;
			}
		}

		private bool initialized = false;

		public ProjectileGroup ProjectileGroup {
			get;
			set;
		}

		internal void Init() {
			if(!initialized) {
				Initialize ();
				initialized = true;
			}
		}

		public virtual void Initialize() {
			Debug.Log ("Hello");
			ProjectileGroup = new ProjectileGroup ();
			ProjectileGroup.Controller = this;
		}

		public abstract Vector2 UpdateProjectile(Projectile projectile, float dt);
	}
}