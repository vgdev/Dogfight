using System;
using UnityEngine;
using UnityUtilLib;

namespace Danmaku2D {
	public class CurvedProjectile : LinearProjectile {

		private Quaternion angularVelocity = Quaternion.identity;
		public float AngularVelocity {
			get {
				return angularVelocity.eulerAngles.z;
			}
			set {
				angularVelocity = Quaternion.Euler(new Vector3(0f, 0f, value));
			}
		}
		
		public float AngularVelocityRadians {
			get {
				return AngularVelocity * Util.Degree2Rad;
			}
			set {
				AngularVelocity = value * Util.Rad2Degree;
			}
		}


		public CurvedProjectile(float velocity, float angularVelocity) : base(velocity) {
			AngularVelocity = angularVelocity;
		}

		public override Vector2 UpdateProjectile (Projectile projectile, float dt) {
			if(angularVelocity != Quaternion.identity) {
				Transform transform = projectile.Transform;
				Quaternion rot = transform.rotation;
				transform.rotation = Quaternion.Slerp (rot, rot * angularVelocity, dt);
			}
			return base.UpdateProjectile (projectile, dt);
		}
	}
}

