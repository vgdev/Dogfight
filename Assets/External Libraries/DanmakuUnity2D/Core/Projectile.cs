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

		private Vector2 circleCenter = Vector2.zero; 
		private float circleRaidus = 1f;
		public SpriteRenderer SpriteRenderer {
			get {
				return renderer;
			}
		}
		
		public Transform Transform {
			get {
				return transform;
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

		public void Update() {
			bulletFrames++;
			Vector3 oldScale = transform.localScale;

			Vector2 movementVector = Vector3.zero;

			if(Controller != null)
				movementVector += Controller.UpdateProjectile (this, Util.TargetDeltaTime);

			//Debug.DrawRay (Transform.position, movementVector);
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
						Transform.position = hits[i].point;
						break;
					}
				}
			}
			
			if (to_deactivate) {
				DeactivateImmediate();
			}
		}

		public void MatchPrefab(ProjectilePrefab prefab) {
			CircleCollider2D cc = prefab.CircleCollider;
			SpriteRenderer sr = prefab.SpriteRenderer;
			
			transform.localScale = prefab.Transform.localScale;
			gameObject.tag = prefab.GameObject.tag;
			gameObject.layer = prefab.GameObject.layer;
			
			if(sr != null) {
				renderer.sprite = sr.sprite;
				renderer.color = sr.color;
				renderer.sharedMaterial = sr.sharedMaterial;
				renderer.sortingOrder = sr.sortingOrder;
				renderer.sortingLayerID = sr.sortingLayerID;
			}
			else
				Debug.LogError("The provided prefab should have a SpriteRenderer!");
			
			if(cc != null) {
				circleCenter = Util.ComponentProduct2(Transform.lossyScale, cc.center);
				circleRaidus = cc.radius * Util.MaxComponent2(Util.To2D(Transform.lossyScale));
			}
			else
				Debug.LogError("The provided prefab should a CircleCollider2D!");
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
			for (int i = 0; i < groups.Count; i++) {
				groups[i].Remove(this);
			}
			groups.Clear ();
			damage = 0;
			gameObject.SetActive (false);
			base.Deactivate ();
		}
	}
}
