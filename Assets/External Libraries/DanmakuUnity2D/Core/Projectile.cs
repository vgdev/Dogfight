using UnityEngine;
using UnityUtilLib;
using System.Collections;
using System.Collections.Generic;

namespace Danmaku2D {

	[RequireComponent(typeof(SpriteRenderer))]
	public sealed class Projectile : PooledObject, IPrefabed<ProjectilePrefab> {

		private static Vector2 unchanged = Vector2.zero;
		private static int[] collisionMask;

		private bool to_deactivate;
		
		private GameObject gameObject;
		private Transform transform;
		private SpriteRenderer renderer;

		private int damage;
		public int Damage {
			get {
				return damage;
			}
			set {
				damage = value;
			}
		}

		public IProjectileController Controller {
			get;
			set;
		}

		#region IPrefabed implementation

		public ProjectilePrefab Prefab {
			get;
			set;
		}

		#endregion

		private List<ProjectileGroup> groups;

		internal List<ProjectileGroup> Groups {
			get {
				return groups;
			}
		}

		private Vector2 circleCenter = Vector2.zero; 
		private float circleRaidus = 1f;

		public Sprite Sprite {
			get {
				return renderer.sprite;
			}
			set {
				renderer.sprite = value;
			}
		}

		public Color Color {
			get {
				return renderer.color;
			}
			set {
				renderer.color = value;
			}
		}

		public SpriteRenderer SpriteRenderer {
			get {
				return renderer;
			}
		}

		public Vector2 Position {
			get {
				return transform.position;
			}
			set {
				transform.position = value;
			}
		}

		public float Rotation {
			get {
				return transform.rotation.eulerAngles.z;
			}
			set {
				transform.rotation = Quaternion.Euler(0f, 0f, value);
			}
		}

		public Vector2 Direction {
			get {
				return transform.up;
			}
		}
		
		private int bulletFrames;
		
		public float BulletTime {
			get {
				return bulletFrames * Util.TargetDeltaTime;
			}
		}
		
		private RaycastHit2D[] hits;

		public Projectile() {
			if(collisionMask == null)
				collisionMask = Util.CollisionLayers2D();
			groups = new List<ProjectileGroup> ();
			gameObject = new GameObject ("Projectile");
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			renderer = gameObject.AddComponent<SpriteRenderer> ();
			transform = gameObject.transform;
			gameObject.SetActive (false);
			hits = new RaycastHit2D[10];
		}

		internal void Update() {
			bulletFrames++;
			float dt = Util.TargetDeltaTime;

			Vector2 movementVector = Vector3.zero;

			if(Controller != null)
				movementVector += Controller.UpdateProjectile (this, dt);

			if(groups.Count > 0) {
				for (int i = 0; i < groups.Count; i++) {
					movementVector += groups[i].UpdateProjectile(this, dt);
				}
			}

			int count = Physics2D.CircleCastNonAlloc(transform.position + Util.To3D(circleCenter), 
			                                         circleRaidus,
			                                         movementVector,
			                                         hits,
			                                         movementVector.magnitude,
			                                         collisionMask[gameObject.layer]);
			
			if(movementVector != unchanged)
				transform.position += (Vector3)movementVector;

			//Translate
			if(count > 0) {
				int i;
				for (i = 0; i < count; i++) {
					RaycastHit2D hit = hits[i];
					if (hit.collider != null) {
						hit.collider.SendMessage("OnProjectileCollision", this, SendMessageOptions.DontRequireReceiver);
					}
					if(to_deactivate){
						Position = hits[i].point;
						break;
					}
				}
			}
			
			if (to_deactivate) {
				DeactivateImmediate();
			}
		}

		public void MatchPrefab(ProjectilePrefab prefab) {
			ProjectilePrefab runtime = prefab.GetRuntime ();
			CircleCollider2D cc = runtime.CircleCollider;
			SpriteRenderer sr = runtime.SpriteRenderer;
			ProjectileControlBehavior[] pcbs = runtime.ExtraControllers;
			
			transform.localScale = runtime.Transform.localScale;
			gameObject.tag = runtime.GameObject.tag;
			gameObject.layer = runtime.GameObject.layer;
			
			if(sr != null) {
				renderer.sprite = sr.sprite;
				renderer.color = sr.color;
				renderer.sharedMaterial = sr.sharedMaterial;
				renderer.sortingOrder = sr.sortingOrder;
			}
			else
				Debug.LogError("The provided prefab should have a SpriteRenderer!");
			
			if(cc != null) {
				circleCenter = Util.ComponentProduct2(transform.lossyScale, cc.center);
				circleRaidus = cc.radius * Util.MaxComponent2(Util.To2D(transform.lossyScale));
			}
			else
				Debug.LogError("The provided prefab should a CircleCollider2D!");

			for(int i = 0; i < pcbs.Length; i++) {
				pcbs[i].Init();
				pcbs[i].ProjectileGroup.Add(this);
			}
		}
		
		public override void Activate () {
			to_deactivate = false;
			gameObject.SetActive (true);
		}

		public override void Deactivate()  {
			to_deactivate = true;
		}
		
		public void DeactivateImmediate() {
			Controller = null;
			ProjectileGroup[] temp = groups.ToArray ();
			for (int i = 0; i < temp.Length; i++) {
				temp[i].Remove(this);
			}
			damage = 0;
			bulletFrames = 0;
			gameObject.SetActive (false);
			base.Deactivate ();
		}
	}
}
