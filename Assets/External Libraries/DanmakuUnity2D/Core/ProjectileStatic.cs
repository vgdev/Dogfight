using System;
using UnityEngine;
using UnityUtilLib;
using UnityUtilLib.Pooling;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {
	
	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed partial class Projectile : IPooledObject, IColorable, IPrefabed<ProjectilePrefab> {
		
		private static int[] collisionMask;
		private static ProjectilePool projectilePool;
		
		internal static void Setup(int initial, int spawn) {
			collisionMask = Util.CollisionLayers2D ();
			projectilePool = new ProjectilePool (initial, spawn);
		}
		
		internal static void UpdateAll() {
			float dt = Util.TargetDeltaTime;
			Projectile[] all = projectilePool.all;
			int totalCount = projectilePool.totalCount;
			for (int i = 0; i < totalCount; i++) {
				all[i].Update(dt);
			}
		}
		
		public static void DeactivateAll() {
			Projectile[] all = projectilePool.all;
			int totalCount = projectilePool.totalCount;
			for (int i = 0; i < totalCount; i++) {
				if(all[i].is_active)
					all[i].DeactivateImmediate();
			}
		}
		
		public static int TotalCount {
			get {
				return (projectilePool != null) ? projectilePool.totalCount : 0;
			}
		}
		
		public static int ActiveCount {
			get {
				return (projectilePool != null) ? projectilePool.totalCount : 0;
			}
		}
		
		internal static Projectile Get (ProjectilePrefab projectileType, Vector2 position, float rotation, DanmakuField field) {
			Projectile proj = projectilePool.Get ();
			proj.MatchPrefab (projectileType);
			proj.PositionImmediate = position;
			proj.Rotation = rotation;
			proj.field = field;
			proj.bounds = field.bounds;
			return proj;
		}
		
		internal static Projectile Get(DanmakuField field, FireBuilder builder) {
			Projectile proj = projectilePool.Get ();
			proj.MatchPrefab (builder.Prefab);
			proj.PositionImmediate = field.WorldPoint (builder.Position, builder.CoordinateSystem);
			proj.Rotation = builder.Rotation;
			proj.field = field;
			proj.bounds = field.bounds;
			return proj;
		}
	}
}

