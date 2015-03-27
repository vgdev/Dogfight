using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public sealed class DanmakuField : MonoBehaviour {

		internal static List<DanmakuField> fields;

		public enum CoordinateSystem { View, Relative, World }

		public float ClipBoundary = 1f;

		public float GamePlaneDistance = 10f;

		[SerializeField]
		private float camera2DRotation = 0f;

		[SerializeField]
		private Transform cameraTransform2D;

		[SerializeField]
		private float size;

		[SerializeField]
		private bool fixedCameraArea;

		[SerializeField]
		private float cameraScreenAnchorPoint = 0.5f;

		[SerializeField]
		private float nativeScreenAspectRatio = 4f/3f;

		[SerializeField]
		private Rect nativeScreenBounds;

		[SerializeField]
		private Vector2 playerSpawnLocation = new Vector2(0.5f, 0.25f);

		[SerializeField]
		private Camera camera2D;
		
		[SerializeField]
		private Camera camera3D;

		[System.NonSerialized]
		public DanmakuField TargetField;

		private DanmakuPlayer player;
		private float currentAspectRatio;
		private float screenOffset;
		internal Bounds2D bounds;
		private Bounds2D movementBounds;
		private Vector2 x, y, z, scale, bottomLeft;

		[SerializeField]
		private Rect viewportRect = new Rect(0f, 0f, 1f, 1f);

		[SerializeField]
		private LayerMask cullingMask;

		[SerializeField]
		private float depth;

		[SerializeField]
		private RenderingPath renderingPath;

		[SerializeField]
		private RenderTexture renderTexture;

		[SerializeField]
		private bool occlusionCulling;

		[SerializeField]
		private bool hdr;

		public float Camera2DRotation {
			get {
				return camera2DRotation;
			}
			set {
				cameraTransform2D.localRotation = Quaternion.Euler(0f, 0f, value);
				camera2DRotation = value;
			}
		}

		public DanmakuPlayer Player {
			get {
				return player;
			}
		}

		public Bounds2D Bounds {
			get {
				return bounds;
			}
		}

		public Bounds2D MovementBounds {
			get {
				return movementBounds;
			}
		}

		public void RecomputeWorldPoints() {
			Vector2 UL, BR;
			bottomLeft = camera2D.ViewportToWorldPoint (new Vector3(0f, 0f, 0f));
			UL = camera2D.ViewportToWorldPoint (Vector2.up);
			BR = camera2D.ViewportToWorldPoint (Vector2.right);
			x = (BR - bottomLeft);
			y = (UL - bottomLeft);

			scale = new Vector2 (x.magnitude, y.magnitude);
		}

		public float XSize {
			get { return scale.x; }
		}

		public float YSize {
			get { return scale.y; }
		}

		public Vector2 BottomLeft {
			get { return WorldPoint (new Vector2(0f, 0f)); }
		}

		public Vector2 BottomRight {
			get { return WorldPoint (new Vector2(1f, 0f)); }
		}

		public Vector2 TopLeft {
			get { return WorldPoint (new Vector2(0f, 1f)); }
		}

		public Vector2 TopRight {
			get { return WorldPoint (new Vector2(1f, 1f)); }
		}

		public Vector2 Center {
			get { return WorldPoint (new Vector2(0.5f, 0.5f)); }
		}

		public Vector2 Top {
			get { return WorldPoint (new Vector2 (0.5f, 1f)); }
		}

		public Vector2 Bottom {
			get { return WorldPoint (new Vector2 (0.5f, 0f));}
		}

		public Vector2 Right {
			get { return WorldPoint (new Vector2 (1f, 0.5f)); }
		}

		public Vector2 Left {
			get { return WorldPoint (new Vector2 (0f, 0.5f));}
		}

		void Awake () {
			if (fields == null) {
				fields = new List<DanmakuField>();
			}
			fields.Add (this);
			TargetField = this;
			CameraSetup ();
			
			#if UNITY_EDITOR
			EditorApplication.playmodeStateChanged += PlayStateModeChange;
			#endif

			RecomputeWorldPoints ();

			screenOffset = nativeScreenBounds.x + 0.5f * nativeScreenBounds.width - cameraScreenAnchorPoint;

			#if UNITY_EDITOR
			if(!Application.isEditor) {
			#endif
				if(fixedCameraArea) {
					currentAspectRatio = (float)Screen.width / (float)Screen.height;
					Resize ();
				}
			#if UNITY_EDITOR
			}
			#endif
		}

		#if UNITY_EDITOR

		void PlayStateModeChange() {
			if(!EditorApplication.isPlaying){
				camera2D.rect = viewportRect;
				//camera2D.cullingMask = cullingMask;
				camera2D.depth = depth;
				camera2D.renderingPath = renderingPath;
				camera2D.targetTexture = renderTexture;
				camera2D.useOcclusionCulling = occlusionCulling;
				camera2D.hdr = hdr;
			}
		}

		#endif

		void Start() {
			CameraSetup ();
		}

		private void Resize() {
			float changeRatio = nativeScreenAspectRatio / currentAspectRatio;
			float targetWidth = changeRatio * nativeScreenBounds.width;
			float center = cameraScreenAnchorPoint + changeRatio * screenOffset;
			Rect cameraRect = camera2D.rect;
			cameraRect.x = center - targetWidth / 2;
			cameraRect.width = targetWidth;
			camera2D.rect = cameraRect;
		}

		private void CameraSetup() {			
			if (camera2D == null) {
				print("Camera is null");
				GameObject camObj = gameObject.FindChild("Field Camera");
				if(camObj == null) {
					camObj = new GameObject ("Field Camera");
				}
				cameraTransform2D = camObj.transform;
				camera2D = camObj.GetComponent<Camera>();
				if(camera2D == null){
					camera2D = camObj.AddComponent<Camera>();
				}
				camObj.hideFlags = HideFlags.HideInHierarchy;
			}
			camera2D.orthographic = true;
			cameraTransform2D.parent = transform;
			cameraTransform2D.position = transform.position + Vector3.back * GamePlaneDistance;
			cameraTransform2D.LookAt ((Vector2)transform.position);
			camera2D.clearFlags = CameraClearFlags.Depth;
			//camera2D.cullingMask = cullingMask;
			camera2D.depth = depth;
			camera2D.renderingPath = renderingPath;
			camera2D.targetTexture = renderTexture;
			camera2D.useOcclusionCulling = occlusionCulling;
			camera2D.hdr = hdr;
		}

		void Update() {
			Vector3 camPos = cameraTransform2D.position;
			camPos.z = -GamePlaneDistance;
			cameraTransform2D.position = camPos;
			cameraTransform2D.rotation = Quaternion.Euler ((camPos.z > 0) ? 180f : 0f, 0f, camera2DRotation);
			camera2D.clearFlags = CameraClearFlags.Depth;
			camera2D.nearClipPlane = 0f;
			camera2D.farClipPlane = Mathf.Abs(camPos.z * 2);
			if (camera3D != null) {
				camera3D.rect = camera2D.rect;
			}
			movementBounds.Center = bounds.Center = (Vector2)transform.position;
			float size = camera2D.orthographicSize;
			movementBounds.Extents = new Vector2 (camera2D.aspect * size, size);
			bounds.Extents = movementBounds.Extents + Vector2.one * ClipBoundary * movementBounds.Extents.Max();
			#if UNITY_EDITOR
			if(Application.isPlaying) {
			#endif
				if(fixedCameraArea) {
					float newAspectRatio = (float)Screen.width / (float)Screen.height;
					if (currentAspectRatio != newAspectRatio) {
						currentAspectRatio = newAspectRatio;
						Resize ();
					}
				}
			#if UNITY_EDITOR
			}
			#endif
		}

		void OnDestroy() {
			if (fields != null) {
				fields.Remove(this);
			}
			#if UNITY_EDITOR
			EditorApplication.playmodeStateChanged -= PlayStateModeChange;
			if(Application.isEditor) {
				DestroyImmediate (camera2D.gameObject);
			} else  {
				Destroy (camera2D.gameObject);
			}
			#else
			Destroy(camera2D.gameObject);
			#endif
		}

		public Vector2 WorldPoint(Vector2 point, CoordinateSystem coordSys = CoordinateSystem.View) {
			switch (coordSys) {
				case CoordinateSystem.World:
					return point;
				case CoordinateSystem.Relative:
					return bottomLeft + point;
				default:
				case CoordinateSystem.View:
					return bottomLeft + point.x * x + point.y * y;
			}
		}

		public Vector2 RelativePoint(Vector2 point, CoordinateSystem coordSys = CoordinateSystem.View) {
			switch (coordSys) {
				case CoordinateSystem.World:
					return point - bottomLeft;
				case CoordinateSystem.Relative:
					return point;
				default:
				case CoordinateSystem.View:
					return point.x * x + point.y * y;
			}
		}

		public Vector2 ViewPoint(Vector2 point, CoordinateSystem coordSys = CoordinateSystem.World) {
			switch (coordSys) {
				default:
				case CoordinateSystem.World:
					Vector2 offset = point - bottomLeft;
					return new Vector2(
						Vector3.Project (offset, x).magnitude / scale.x,
						Vector3.Project (offset, y).magnitude / scale.y);
				case CoordinateSystem.Relative:
					return new Vector2(
						Vector3.Project (point, x).magnitude / scale.x,
						Vector3.Project (point, y).magnitude / scale.y);
				case CoordinateSystem.View:
					return point;
			}
		}

		public float AngleTowardPlayer(Vector2 startLocation, CoordinateSystem coordinateSystem = CoordinateSystem.World) {
			return Util.AngleBetween2D (startLocation, Player.transform.position);
		}

		/// <summary>
		/// Spawns the player with the given controller
		/// </summary>
		/// <param name="character">Character prefab, defines character behavior and attack patterns.</param>
		/// <param name="controller">Controller for the player, allows for a user to manually control it or let an AI take over.</param>
		public DanmakuPlayer SpawnPlayer(DanmakuPlayer playerCharacter, CoordinateSystem coordSys = CoordinateSystem.View) {
			Vector2 spawnPos = WorldPoint((Vector2)playerSpawnLocation, coordSys);
			player =  (DanmakuPlayer) Instantiate(playerCharacter, spawnPos, Quaternion.identity);
			if(player != null) {
				player.Reset (5);
				player.transform.parent = transform;
				player.Field = this;
			}
			return player;
		}

		
		public void SpawnEnemy(Enemy prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			Enemy enemy = (Enemy)Instantiate(prefab);
			Transform transform = enemy.transform;
			transform.position = WorldPoint(location, coordSys);
			enemy.Field = this;
		}
		
		public GameObject SpawnGameObject(GameObject gameObject, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			GameObject instance = (GameObject)Instantiate (gameObject);
			instance.transform.position = WorldPoint (location, coordSys);
			return instance;
		}

		public Component SpawnObject(Component prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) {
			Component instance = (Component)Instantiate (prefab);
			instance.transform.position = WorldPoint (location, coordSys);
			return instance;
		}

		public T SpawnObject<T>(T prefab, Vector2 location, CoordinateSystem coordSys = CoordinateSystem.View) where T : Component {
			T instance = (T)Instantiate (prefab);
			instance.transform.position = WorldPoint (location, coordSys);
			return instance;
		}
		
		/// <summary>
		/// Spawns a projectile in the field.
		/// 
		/// If absoluteWorldCoord is set to false, location specifies a relative position in the field. 0.0 = left/bottom, 1.0 = right/top. Values greater than 1 or less than 0 spawn
		/// outside of of the camera view.
		/// </summary>
		/// <param name="prefab">Prefab for the spawned projectile, describes the visuals, size, and hitbox characteristics of the prefab.</param>
		/// <param name="location">The location within the field to spawn the projectile.</param>
		/// <param name="rotation">Rotation.</param>
		/// <param name="absoluteWorldCoord">If set to <c>true</c>, <c>location</c> is in absolute world coordinates relative to the bottom right corner of the game plane.</param>
		public Projectile SpawnProjectile(ProjectilePrefab bulletType, Vector2 location, float rotation, CoordinateSystem coordSys = CoordinateSystem.View) {
			Projectile bullet = Projectile.Get (bulletType, WorldPoint(location, coordSys), rotation, this);
			bullet.Activate ();
			return bullet;
		}

		public FireData<LinearProjectile> FireLinearProjectile(ProjectilePrefab bulletType, 
		                                         Vector2 location, 
		                                         float rotation, 
		                                         float velocity,
		                                         CoordinateSystem coordSys = CoordinateSystem.View) {
			LinearProjectile controller = new LinearProjectile (velocity);
			Projectile projectile = Projectile.Get (bulletType, WorldPoint(location, coordSys), rotation, this);
			projectile.Activate ();
			projectile.AddController(controller);
			return new FireData<LinearProjectile>(projectile, controller);
		}
		
		public FireData<CurvedProjectile> FireCurvedProjectile(ProjectilePrefab bulletType,
		                                         Vector2 location,
		                                         float rotation,
		                                         float velocity,
		                                         float angularVelocity,
		                                         CoordinateSystem coordSys = CoordinateSystem.View) {
			CurvedProjectile controller = new CurvedProjectile (velocity, angularVelocity);
			Projectile projectile = Projectile.Get (bulletType, WorldPoint(location, coordSys), rotation, this);
			projectile.Activate ();
			projectile.AddController(controller);
			return new FireData<CurvedProjectile>(projectile, controller);
		}
		
		public Projectile FireControlledProjectile(ProjectilePrefab bulletType, 
		                                 Vector2 location, 
		                                 float rotation, 
		                                 IProjectileController controller,
		                                 CoordinateSystem coordSys = CoordinateSystem.View) {
			Projectile projectile = Projectile.Get (bulletType, WorldPoint(location, coordSys), rotation, this);
			projectile.Activate ();
			projectile.AddController(controller);
			return projectile;
		}

		public void SpawnBurst(ProjectilePrefab bulletType,
		                       Vector2 location,
		                       float direction,
		                       float range,
		                       int count,
		                       ProjectileGroup group = null,
		                       int depth = 1,
		                       BurstController burstController = null,
		                       CoordinateSystem coordSys = CoordinateSystem.View) {
			bool haveGroup = group != null;
			bool haveController = burstController != null;
			count = Mathf.Abs (count);
			depth = Mathf.Abs (depth);
			range = Mathf.Abs (range);
			Vector2 position = WorldPoint (location, coordSys);
			float start = direction - range * 0.5f;
			float end = direction + range * 0.5f;
			float delta = range / count;
			for (int j = 0; j < depth; j++) {
				IProjectileController controller = (haveController) ? burstController(j) : null;
				for (float rotation = start; rotation <= end; rotation += delta) {
					Projectile bullet = Projectile.Get (bulletType, position, rotation, this);
					bullet.AddController(controller);
					bullet.Activate ();
					if (haveGroup) {
						group.Add (bullet);
					}
				}
			}
		}

		public Projectile FireLinearProjectile(FireBuilder data, float velocity) {
			LinearProjectile controller = new LinearProjectile (velocity);
			Projectile projectile = Projectile.Get (this, data);
			projectile.Activate ();
			projectile.AddController(controller);
			data.Controller = controller;
			return projectile;
		}
		
		public Projectile FireCurvedProjectile(FireBuilder data, float velocity, float angularVelocity) {
			CurvedProjectile controller = new CurvedProjectile (velocity, angularVelocity);
			Projectile projectile = Projectile.Get (this, data);
			projectile.Activate ();
			projectile.AddController(controller);
			data.Controller = controller;
			return projectile;
		}
		
		public Projectile FireControlledProjectile(FireBuilder data) {
			Projectile projectile = Projectile.Get (this, data);
			projectile.Activate ();
			projectile.AddController(data.Controller);
			return projectile;
		}

		public void SpawnBurst(BurstBuilder data) {
			IProjectileController allController = data.Controller;
			ProjectileGroup group = data.Group;
			BurstController burstController = data.BurstController;
			bool haveGroup = group != null;
			bool haveController = data.BurstController != null;
			float direction = data.Rotation;
			float range = data.Range;
			float start = direction - range * 0.5f;
			float end = direction + range * 0.5f;
			float delta = range / data.Count;
			for (int j = 0; j < data.Depth; j++) {
				IProjectileController controller = (haveController) ? burstController(j) : null;
				for (float rotation = start; rotation <= end; rotation += delta) {
					data.Rotation = rotation;
					Projectile bullet = Projectile.Get (this, data);
					bullet.AddController(allController);
					bullet.AddController(controller);
					bullet.Activate ();
					if (haveGroup) {
						group.Add (bullet);
					}
				}
			}
			data.Rotation = direction;
		}

		#if UNITY_EDITOR
		void OnDrawGizmos() {
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube (bounds.Center, bounds.Size);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube (movementBounds.Center, movementBounds.Size);
			Vector3 newExtents = movementBounds.Size;
			newExtents.z = 2 * GamePlaneDistance;
			Gizmos.color = Color.white;
			Vector3 camPos = movementBounds.Center;
			Gizmos.matrix = Matrix4x4.TRS(camPos, Quaternion.Euler((camPos.z > 0) ? 180f : 0f, 0f, -Camera2DRotation), Vector3.one);
			Gizmos.DrawWireCube(Vector3.zero, newExtents);
		}
		#endif
	}

	public struct FireData<T> where T : IProjectileController {
		public Projectile Projectile;
		public T Controller;
		
		public FireData(Projectile projectile, T controller) {
			Projectile = projectile;
			Controller = controller;
		}
	}

	[System.Serializable]
	public class FireBuilder : IClonable<FireBuilder> {
		public ProjectilePrefab Prefab = null;
		public Vector2 Position = Vector2.zero;
		public float Rotation = 0;
		public IProjectileController Controller = null;
		public DanmakuField.CoordinateSystem CoordinateSystem = DanmakuField.CoordinateSystem.View;
		
		public FireBuilder(ProjectilePrefab prefab) {
			this.Prefab = prefab;
		}

		#region IClonable implementation
		public FireBuilder Clone () {
			FireBuilder copy = new FireBuilder (Prefab);
			copy.Position = Position;
			copy.Rotation = Rotation;
			copy.Controller = Controller;
			copy.CoordinateSystem = CoordinateSystem;
			return copy;
		}
		#endregion
	}
	
	public delegate IProjectileController BurstController(int depth);

	[System.Serializable]
	public class BurstBuilder : FireBuilder {
		[SerializeField]
		private float range = 360f;
		public float Range {
			get {
				return range;
			}
			set {
				range = System.Math.Abs(value);
			}
		}

		[SerializeField]
		private int count = 1;
		public int Count {
			get {
				return count;
			}
			set {
				count = System.Math.Abs(value);
			}
		}

		[SerializeField]
		private int depth = 1;
		public int Depth {
			get {
				return depth;
			}
			set {
				depth = System.Math.Abs(value);
			}
		}

		public ProjectileGroup Group = null;
		public BurstController BurstController;

		public BurstBuilder(ProjectilePrefab prefab) : base(prefab) {
		}

		#region IClonable implementation

		public new BurstBuilder Clone () {
			BurstBuilder copy = new BurstBuilder (Prefab);
			copy.Position = Position;
			copy.Rotation = Rotation;
			copy.Controller = Controller;
			copy.CoordinateSystem = CoordinateSystem;
			copy.range = range;
			copy.count = count;
			copy.depth = depth;
			copy.Group = Group;
			copy.BurstController = BurstController;
			return copy;
		}

		#endregion
	}
}