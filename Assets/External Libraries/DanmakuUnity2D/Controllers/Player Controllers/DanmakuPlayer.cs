using UnityEngine;
using UnityUtilLib;
using System.Collections.Generic;

namespace Danmaku2D {

	[RequireComponent(typeof(Collider2D))]
	public abstract class DanmakuPlayer : DanmakuTrigger, IPausable {

		public virtual DanmakuField Field {
			get;
			set;
		}

		public bool Paused {
			get;
			set;
		}
		
		private PlayerAgent agent;
		public PlayerAgent Agent {
			get {
				return agent;
			}
			set {
				agent = value;
				agent.Player = this;
			}
		}

		public override void Awake () {
			base.Awake ();
			Field = DanmakuField.FindClosest (transform.position);
			Field.player = this;
			movementCollider = GetComponent<Collider2D> ();
		}

		void Update() {
			if(!Paused)
				NormalUpdate();
		}

		public virtual void NormalUpdate() {
			if (agent != null)
				agent.Update ();
		}

		[SerializeField]
		private float normalMovementSpeed = 5f;

		[SerializeField]
		private float focusMovementSpeed = 3f;

		private int livesRemaining;
		public int LivesRemaining {
			get {
				return livesRemaining;
			}
		}

		public virtual bool IsFiring {
			get;
			set;
		}

		public virtual bool IsFocused {
			get;
			set;
		}

		private Collider2D movementCollider;

		[SerializeField]
		private float fireRate = 4.0f;
		private float fireDelay;

		private Vector2 forbiddenMovement = Vector3.zero;

		public int CanMoveHorizontal {
			get { return -(int)Util.Sign(forbiddenMovement.x); }
		}

		public int CanMoveVertical {
			get { return -(int)Util.Sign(forbiddenMovement.y); }
		}

		public virtual void Move(float horizontalDirection, float verticalDirection) {
			Bounds2D fieldBounds = Field.MovementBounds;
			Bounds2D myBounds = new Bounds2D(movementCollider.bounds);
			float dt = Util.TargetDeltaTime;
			float movementSpeed = (IsFocused) ? focusMovementSpeed : normalMovementSpeed;
			Vector2 position = transform.position;
			Vector2 dir = new Vector2 (Util.Sign(horizontalDirection), Util.Sign(verticalDirection));
			Vector2 movementVector = movementSpeed * Vector2.one;
			movementVector.x *= dt * ((dir.x == Util.Sign(forbiddenMovement.x)) ? 0f : dir.x);
			movementVector.y *= dt * ((dir.y == Util.Sign(forbiddenMovement.y)) ? 0f : dir.y);
			position += (Vector2)movementVector;
			myBounds.Center += movementVector;
			Vector2 myMin = myBounds.Min;
			Vector2 myMax = myBounds.Max;
			Vector2 fMin = fieldBounds.Min;
			Vector2 fMax = fieldBounds.Max;
//			Debug.Log (myMin.ToString () + " " + fMin.ToString () + " " + fMax.ToString());
			if (myMin.x < fMin.x)
				position.x += fMin.x - myMin.x;
			else if (myMax.x > fMax.x)
				position.x += fMax.x - myMax.x;
			else if (myMin.y < fMin.y)
				position.y += fMin.y - myMin.y;
			else if (myMax.y > fMax.y)
				position.y += fMax.y - myMax.y;
			transform.position = position;
		}

		public void AllowMovement(Vector2 direction) {
			if(Util.Sign(direction.x) == Util.Sign(forbiddenMovement.x)) {
				forbiddenMovement.x = 0;
			}
			if(Util.Sign(direction.y) == Util.Sign(forbiddenMovement.y)) {
				forbiddenMovement.y = 0;
			}
		}

		public void ForbidMovement(Vector2 direction) {
			if(direction.x != 0) {
				forbiddenMovement.x = direction.x;
			}
			if(direction.y != 0) {
				forbiddenMovement.y = direction.y;
			}
		}

		public virtual void Hit(Danmaku proj) {
			livesRemaining--;
		}

		public void Reset(int maxLives) {
			livesRemaining = maxLives;
		}

		public virtual void Graze (Danmaku proj) {
		}

		public void FireCheck(float dt) {
			if(IsFiring) {
				fireDelay -= dt;
				if(fireDelay < 0f) {
					Trigger();
					fireDelay = 1f / fireRate;
				}
			}
		}
	}
}