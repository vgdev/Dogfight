using UnityEngine;

namespace Danmaku2D.ProjectileControllers {

	public class LinearProjectileControl : ControllerWrapperBehavior<LinearProjectile> {
		[SerializeField]
		private LinearProjectile controller;

		#region implemented abstract members of ControllerWrapperBehavior
		protected override LinearProjectile CreateController () {
			return controller;
		}
		#endregion
	}

}


/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {
	
	/// <summary>
	/// A ProjectileController or ProjectileGroupController for creating bullets that move along a straight path.
	/// </summary>
	[System.Serializable]
	public class LinearProjectile : ProjectileController, IProjectileGroupController {
		
		[SerializeField]
		private float velocity;
		
		/// <summary>
		/// Gets or sets the velocity of the controlled Projectile(s)
		/// </summary>
		/// <value>The velocity of the controlled Projectile(s) in absolute world coordinates per second</value>
		public float Velocity {
			get {
				return velocity;
			}
			set {
				velocity = value;
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.ProjectileControllers.LinearProjectile"/> class.
		/// </summary>
		/// <value>The velocity of the controlled Projectile(s) in absolute world coordinates per second</value>
		public LinearProjectile (float velocity) : base() {
			Velocity = velocity;
		}
		
		#region IProjectileController implementation
		
		public sealed override Vector2 UpdateProjectile (float dt) {
			return UpdateProjectile (Projectile, dt);
		}
		
		#endregion
		
		#region IProjectileGroupController implementation
		
		public ProjectileGroup ProjectileGroup {
			get;
			set;
		}
		
		public virtual Vector2 UpdateProjectile (Projectile projectile, float dt) {
			if (Velocity != 0)
				return projectile.Direction * Velocity * dt;
			else
				return Vector2.zero;
		}
		
		#endregion
	}
}

