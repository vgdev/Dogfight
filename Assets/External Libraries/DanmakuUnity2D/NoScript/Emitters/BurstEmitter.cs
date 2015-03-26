using UnityEngine;
using System.Collections;

namespace Danmaku2D.NoScript {

	internal sealed class BurstEmitter : ProjectileEmitter {

		#pragma warning disable 0649
		public BurstBuilder burst;
		public ProjectileControlBehavior controller;
		#pragma warning restore 0649


		#region implemented abstract members of ProjectileEmitter

		protected override void FireProjectiles () {
			burst.Controller = controller;
			Source.FireBurst (burst);
		}

		#endregion
	}
}