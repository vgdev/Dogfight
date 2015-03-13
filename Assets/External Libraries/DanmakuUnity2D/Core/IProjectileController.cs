using UnityEngine;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// An interface for defining any controller of single Projectile.
	/// If looking to reuse behavior among large numbers of projectiles, use a ProjectileGroup and IProjectileGroupController instead.
	/// Generally speaking it's best not to directly implement this interface manually, use only when sublcassing ProjectileController or ProjectileControlBehavior does not work.
	/// </summary>
	public interface IProjectileController {

		/// <summary>
		/// Gets or sets the projectile controlled by this controller.
		/// </summary>
		/// <value>The projectile controlled by this controller</value>
		Projectile Projectile { get; set; }

		/// <summary>
		/// Updates the Projectile controlled by the controller instance.
		/// </summary>
		/// <returns>the displacement from the Projectile's original position after udpating</returns>
		/// <param name="dt">the change in time since the last update</param>
		Vector2 UpdateProjectile (float dt);

		/// <summary>
		/// Gets or sets the damage of the Projecitle controlled by this controller.
		/// See <see cref="Projectile.Damage"/>.
		/// </summary>
		/// <value>The damage of the Projectile</value>
		int Damage { get; set; }

		/// <summary>
		/// Gets the sprite of the Projectile controlled by this controller.
		/// See <see cref="Projectile.Sprite"/>.
		/// </summary>
		/// <value>The sprite of the Projectile controlled by this controller.</value>
		Sprite Sprite { get; }

		/// <summary>
		/// Gets or sets the color of the Projectile controlled by this controller.
		/// See <see cref="Projectile.Color"/>.
		/// </summary>
		/// <value>The color of the Projectile controlled by this contorller.</value>
		Color Color { get; set; }

		/// <summary>
		/// Gets or sets the absolute world coordinate of the position of where the Projectile contorlled by this controller is.
		/// See <see cref="Projectile.Position"/>.
		/// </summary>
		/// <value>The absolute world coordinate of the position of where the Projectile contorlled by this controller is.</value>
		Vector2 Position { get; set; }

		/// <summary>
		/// Gets or sets the rotation of the Projectile controlled by this controller in degrees.
		/// See <see cref="Projectile.Rotation"/>.
		/// </summary>
		/// <value>The rotation of the Projectile controlled by this controller in degrees./value>
		float Rotation { get; set; }

		/// <summary>
		/// Gets the direction that Projectile controlled by this controller is moving in.
		/// See <see cref="Projectile.Direction"/>.
		/// </summary>
		/// <value>The direction that Projectile controlled by this controller is moving in.</value>
		Vector2 Direction { get; }

		/// <summary>
		/// Gets the time since the Projectile controlled by this controller was fired.
		/// See <see cref="Projectile.Time"/>.
		/// </summary>
		/// <value>The time since the Projectile controlled by this controller was fired.</value>
		float Time { get; }

		/// <summary>
		/// Gets the number of frames that have passed since the Projectile controlled by this controller was fired.
		/// See <see cref="Projectile.Frames"/>. 
		/// </summary>
		/// <value>The number of frames that have passed since the Projectile controlled by this controller was fired.</value>
		int Frames { get; }
	}
}