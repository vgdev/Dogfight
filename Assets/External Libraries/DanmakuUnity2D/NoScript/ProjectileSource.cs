using UnityEngine;
using System.Collections.Generic;
using UnityUtilLib;

namespace Danmaku2D {
	
	public struct SourcePoint {
		public Vector2 Position;
		public float BaseRotation;
		
		public SourcePoint(Vector2 location, float rotation) {
			this.Position = location;
			this.BaseRotation = rotation;
		}
	}

	public abstract class ProjectileSource : CachedObject {

		[System.NonSerialized]
		public DanmakuField TargetField;

		protected SourcePoint[] sourcePoints;

		protected abstract void UpdateSourcePoints ();

		void Update() {
			if (transform.hasChanged) {
				UpdateSourcePoints ();
				transform.hasChanged = false;
			}
		}

		public override void Awake () {
			base.Awake ();
			TargetField = Util.FindClosest<DanmakuField> (transform.position);
			UpdateSourcePoints ();
		}

		public SourcePoint[] SourcePoints {
			get {
				UpdateSourcePoints();
				return sourcePoints;
			}
		}

		private static IProjectileController DefaultController(int depth) {
			return new LinearProjectile (5f);
		}

		public void FireSingle(ProjectilePrefab prefab, 
		                       float rotationOffset = 0, 
		                       IProjectileController controller = null) {
			if(TargetField == null) {
				Debug.LogWarning("Firing from a Projectile Source without a Target Field");
				return;
			}
			if (controller == null) {
				controller = DefaultController(0);
			}
			for (int i = 0; i < sourcePoints.Length; i++) {
				SourcePoint source = sourcePoints[i];
				TargetField.FireControlledProjectile (prefab,
				                                      source.Position,
				                                      source.BaseRotation + rotationOffset,
				                                      controller,
				                                      DanmakuField.CoordinateSystem.World);
			}
		}

		public void FireSingle(FireBuilder data) {
			if(TargetField == null) {
				Debug.LogWarning("Firing from a Projectile Source without a Target Field");
				return;
			}
			IProjectileController controller = data.Controller;
			if (controller == null) {
				controller = new LinearProjectile (5f);
			}
			FireBuilder copy = data.Clone ();
			float rotationOffset = data.Rotation;
			copy.CoordinateSystem = DanmakuField.CoordinateSystem.World;
			copy.Controller = controller;
			for (int i = 0; i < sourcePoints.Length; i++) {
				SourcePoint source = sourcePoints[i];
				copy.Position = source.Position;
				copy.Rotation = source.BaseRotation + rotationOffset;
				TargetField.FireControlledProjectile(copy);
			}
		}

		public void FireBurst(ProjectilePrefab prefab,
		                      int count,
		                      float rotationOffset = 0, 
		                      float rotationRange = 360f, 
		                      int depth = 1,
		                      BurstController controller = null) {
			if(TargetField == null) {
				Debug.LogWarning("Firing from a Projectile Source without a Target Field");
				return;
			}
			if (controller == null) {
				controller = DefaultController;
			}
			for (int i = 0; i < sourcePoints.Length; i++) {
				SourcePoint source = sourcePoints[i];
				TargetField.SpawnBurst (prefab,
				                        source.Position,
				                        source.BaseRotation + rotationOffset,
				                        rotationRange,
				                        count,
				                        null,
				                        depth,
				                        controller,
				                        DanmakuField.CoordinateSystem.World);
			}
		}

		public void FireBurst(BurstBuilder data) {
			if(TargetField == null) {
				Debug.LogWarning("Firing from a Projectile Source without a Target Field");
				return;
			}
			BurstBuilder copy = data.Clone ();
			float rotationOffset = data.Rotation;
			copy.CoordinateSystem = DanmakuField.CoordinateSystem.World;

			for (int i = 0; i < sourcePoints.Length; i++) {
				SourcePoint source = sourcePoints[i];
				copy.Position = source.Position;
				copy.Rotation = source.BaseRotation + rotationOffset;
				TargetField.SpawnBurst (copy);
			}
		}

		void OnDrawGizmos() {
			UpdateSourcePoints ();
			for(int i = 0; i < sourcePoints.Length; i++) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(sourcePoints[i].Position, 1f);
				Gizmos.color = Color.red;
				Vector3 endRay = sourcePoints[i].Position + 5 * Util.OnUnitCircle(sourcePoints[i].BaseRotation + 90f).normalized;
				Gizmos.DrawLine(sourcePoints[i].Position, endRay);
			}
		}
	}
	
}