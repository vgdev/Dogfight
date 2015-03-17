using UnityEngine;
using UnityUtilLib;
using UnityUtilLib.Pooling;
using System.Collections.Generic;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D {

	/// <summary>
	/// A single projectile fired.
	/// The base object that represents a single bullet in a Danmaku game
	/// </summary>
	public sealed class Projectile : PooledObject, IColorable, IPrefabed<ProjectilePrefab> {

		private static int[] collisionMask;

		internal static void SetupCollisions() {
			collisionMask = Util.CollisionLayers2D ();
		}

		internal int index;

		private bool to_deactivate;
		private GameObject gameObject;
		private Transform transform;
		private SpriteRenderer renderer;
		private ProjectilePrefab prefab;
		private ProjectilePrefab runtime;
		
		private IProjectileController controller;
		internal List<ProjectileGroup> groups;
		private ProjectileGroup[] groupCache;
		private Vector2 circleCenter = Vector2.zero; 

		private float rotation;
		private Vector2 direction;

		//Preallocated variables to avoid allocation in Update
		private Vector2 originalPosition;
		private Vector2 movementVector;
		private int count;
		private RaycastHit2D[] raycastHits;
		private Collider2D[] colliders;
		private IProjectileCollider[] scripts;

		//Cached check for controllers to avoid needing to calculate them in Update
		internal bool groupCheck;
		private bool controllerCheck;
		
		private float circleRaidus = 1f;
		private Sprite sprite;
		private Color color;
		private string tag;
		private int layer;

		/// <summary>
		/// Gets or sets the damage this projectile does to entities.
		/// Generally speaking, this is only used for projectiles fired by the player at enemies
		/// </summary>
		/// <value>The damage this projectile does.</value>
		public int Damage {
			get;
			set;
		}

		/// <summary>
		/// The controller 
		/// </summary>
		/// <value>The controller.</value>
		public IProjectileController Controller {
			get {
				return controller;
			}
			set {
				controller = value;
				controllerCheck = controller != null;
				if(controllerCheck) {
					controller.Projectile = this;
				}
			}
		}

		/// <summary>
		/// Gets the renderer sprite of the projectile.
		/// <see href="http://docs.unity3d.com/ScriptReference/SpriteRenderer-sprite.html">SpriteRenderer.sprite</see>
		/// </summary>
		/// <value>The sprite.</value>
		public Sprite Sprite {
			get {
				return sprite;
			}
		}

		/// <summary>
		/// Gets or sets the renderer color of the projectile.
		/// <see href="http://docs.unity3d.com/ScriptReference/SpriteRenderer-color.html">SpriteRenderer.color</see>
		/// </summary>
		/// <value>The renderer color.</value>
		public Color Color {
			get {
				return color;
			}
			set {
				renderer.color = value;
				color = value;
			}
		}

		/// <summary>
		/// Gets or sets the position, in world space, of the projectile.
		/// Exposed as a public variable instead of a property to decrease computational overhead.
		/// </summary>
		/// <value>The position of the projectile.</value>
		public Vector2 Position;

		public Vector2 PositionImmediate {
			get {
				return Position;
			}
			set {
				Position = transform.localPosition = value;
			}
		}

		/// <summary>
		/// Gets or sets the rotation of the projectile, in degrees.
		/// If viewed from a unrotated orthographic camera:
		/// 0 - Straight up
		/// 90 - Straight Left
		/// 180 - Straight Down
		/// 270 -  Straight Right
		/// </summary>
		/// <value>The rotation of the bullet in degrees.</value>
		public float Rotation {
			get {
				return rotation;
			}
			set {
				transform.rotation = Quaternion.Euler(0f, 0f, value);
				rotation = value;
				direction = transform.up;
			}
		}

		/// <summary>
		/// Gets the direction vector the projectile is facing.
		/// It is a unit vector.
		/// Changing <see cref="Rotation"/> will change this vector.
		/// </summary>
		/// <value>The direction vector the projectile is facing toward.</value>
		public Vector2 Direction {
			get {
				return direction;
			}
		}
		
		private int frames;

		/// <summary>
		/// The amount of time, in seconds,that has passed since this bullet has been fired.
		/// This is calculated based on the number of AbstractProjectileControllerd frames that has passed since the bullet has fired
		/// Pausing will cause this value to stop increasing
		/// </summary>
		/// <value>The time since the projectile has been fired.</value>
		public float Time {
			get {
				return frames * Util.TargetDeltaTime;
			}
		}

		/// <summary>
		/// The number of frames that have passed since this bullet has been fired.
		/// </summary>
		/// <value>The frame count since this bullet has been fired.</value>
		public int Frames {
			get {
				return frames;
			}
		}

		/// <summary>
		/// Gets the projectile's tag.
		/// </summary>
		/// <value>The tag of the projectile.</value>
		public string Tag {
			get {
				return tag;
			}
		}

		/// <summary>
		/// Gets the projectile's layer.
		/// </summary>
		/// <value>The layer of the projectile.</value>
		public int Layer {
			get {
				return layer;
			}
		}

		/// <summary>
		/// Gets the DanmakuField this instance was fired from.
		/// </summary>
		/// <value>The field the projectile was fired from.</value>
		public DanmakuField Field {
			get;
			internal set;
		}

		/// <summary>
		/// Compares the tag of the Projectile instance to the given string.
		/// Mirrors <a href="http://docs.unity3d.com/ScriptReference/GameObject.CompareTag.html">GameObject.CompareTag</a>.
		/// </summary>
		/// <returns><c>true</c>, if tag is an exact match to the string, <c>false</c> otherwise.</returns>
		/// <param name="tag">Tag.</param>
		public bool CompareTag(string tag) {
			return gameObject.CompareTag (tag);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Danmaku2D.Projectile"/> class.
		/// </summary>
		internal Projectile() {
			groups = new List<ProjectileGroup> ();
			gameObject = new GameObject ("Projectile");
			#if UNITY_EDITOR
			//This is purely for cleaning up the inspector, no need in an actual build
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			#endif
			renderer = gameObject.AddComponent<SpriteRenderer> ();
			transform = gameObject.transform;
			//gameObject.SetActive (false);
			raycastHits = new RaycastHit2D[10];
			colliders = new Collider2D[10];
			scripts = new IProjectileCollider[10];
		}

		public void Update(float dt) {
			frames++;
	
			originalPosition = Position;

			if (controllerCheck) {
				controller.UpdateProjectile (dt);
			}

			if(groupCheck) {
				int groupCount = groups.Count;
				for(int i = 0; i < groupCount; i++) {
					groups[i].UpdateProjectile(this, dt);
				}
			}

			movementVector = Position - originalPosition;

			float distance = movementVector.magnitude;
			bool discrete = distance < circleRaidus;
			if (discrete) {
				count = Physics2D.OverlapCircleNonAlloc(originalPosition + circleCenter,
				                                        circleRaidus,
				                                        colliders);
			} else {
				count = Physics2D.CircleCastNonAlloc(originalPosition + circleCenter, 
				                                     circleRaidus,
				                                     movementVector,
				                                     raycastHits,
				                                     distance,
				                                     collisionMask[layer]);
			}

			//Translate
			if(count > 0) {
				for (int i = 0; i < count; i++) {
					Collider2D collider = (discrete) ? colliders[i] : raycastHits[i].collider;
					int count2 = 0;
					scripts = Util.GetComponentsPrealloc(collider, scripts, out count2);
					for(int j = 0; j < count2; j++) {
						scripts[j].OnProjectileCollision(this);
					}
					if(to_deactivate){
						if(discrete) {
							Position = Physics2D.CircleCast(originalPosition + circleCenter, circleRaidus, movementVector, distance).point;
						} else {
							Position = raycastHits[i].point;
						}
						break;
					}
				}
			}

			transform.localPosition = Position;
			
			if (to_deactivate) {
				DeactivateImmediate();
			}
		}

		/// <summary>
		/// Makes the instance of Projectile match the given ProjectilePrefab
		/// This copies:
		/// - the sprite, material, sorting layer, and color from the ProjectilePrefab's SpriteRenderer
		/// - the collider's size and offset from the ProjectilePrefab's CircleCollider2D
		/// - the tag and layer from the ProjectilePrefab's GameObject
		/// - any <see cref="ProjectileControlBehavior"/> on the ProjectilePrefab will be included as additional <see cref="IProjectileGroupController"/> that will affect the behavior of this bullet
		/// </summary>
		/// <param name="prefab">the ProjectilePrefab to match.</param>
		public void MatchPrefab(ProjectilePrefab prefab) {

			if (prefab != this.prefab) {
				this.prefab = prefab;
				runtime = prefab.GetRuntime();
				transform.localScale = runtime.Scale;
				sprite = renderer.sprite = runtime.Sprite;
				renderer.sharedMaterial = runtime.Material;
				renderer.sortingLayerID = renderer.sortingLayerID;
				circleCenter = Util.HadamardProduct2(transform.lossyScale, runtime.ColliderOffset);
				circleRaidus = runtime.ColliderRadius * Util.MaxComponent2(transform.lossyScale);
				tag = gameObject.tag = runtime.Tag;
				layer = runtime.Layer;
			}

			renderer.color = runtime.Color;

			ProjectileControlBehavior[] pcbs = runtime.ExtraControllers;
			if (pcbs.Length > 0) {
				for (int i = 0; i < pcbs.Length; i++) {
					pcbs [i].ProjectileGroup.Add (this);
				}
			}
		}

		/// <summary>
		/// Activates this instance.
		/// Calling this on a already fired projectile does nothing.
		/// Calling this on a projectile marked for deactivation will unmark the projectile and keep it from deactivating.
		/// </summary>
		public override void Activate () {
			base.Activate ();
			to_deactivate = false;
			//gameObject.SetActive (true);
			renderer.enabled = true;
		}

		/// <summary>
		/// Marks the Projectile for deactivation, and the Projectile will deactivate and return to the ProjectileManager after 
		/// finishing processing current updates, or when the Projectile is next updated
		/// If Projectile needs to be deactivated in a moment when it is not being updated (i.e. when the game is paused), use <see cref="DeactivateImmediate"/> instead.
		/// </summary>
		public override void Deactivate()  {
			to_deactivate = true;
		}

		/// <summary>
		/// Adds this projectile to the given ProjectileGroup
		/// </summary>
		/// <param name="group">the group this Projectile is to be added to</param>
		public void AddToGroup(ProjectileGroup group) {
			groups.Add (group);
			group.group.Add (this);
			groupCheck = groups.Count > 0;
		}

		/// <summary>
		/// Removes this projectile from the given ProjectileGroup
		/// </summary>
		/// <param name="group">the group this Projectile is to be removed from</param>
		public void RemoveFromGroup(ProjectileGroup group) {
			groups.Remove (group);
			group.group.Remove (this);
			groupCheck = groups.Count > 0;
		}

		/// <summary>
		/// Immediately deactivates this Projectile and returns it to the pool it came from
		/// Calling this generally unadvised. Use <see cref="Deactivate"/> whenever possible.
		/// This method should only be used when dealing with Projectiles while the game is paused or when ProjectileManager is not enabled
		/// </summary>
		public void DeactivateImmediate() {
			for (int i = 0; i < groups.Count; i++) {
				groups[i].group.Remove (this);
			}
			groups.Clear ();
			groupCheck = false;
			Controller = null;
			controllerCheck = false;
			Damage = 0;
			frames = 0;
			//gameObject.SetActive (false);
			renderer.enabled = false;
			IsActive = false;
			base.Deactivate ();
			//ProjectileManager.Return (this);
		}
	}
}
