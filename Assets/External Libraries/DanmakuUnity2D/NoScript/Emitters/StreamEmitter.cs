using UnityEngine;
using System.Collections;

namespace Danmaku2D.NoScript {

	internal class StreamEmitter : ProjectileEmitter {
		
		#pragma warning disable 0649
		public ProjectilePrefab prefab;
		public float rotationOffset;
		public ProjectileControlBehavior controller;
		#pragma warning restore 0649

		#region implemented abstract members of ProjectileEmitter

		protected override void FireProjectiles () {
			Source.FireSingle (prefab, rotationOffset, controller);
		}

		#endregion
		
	}
}