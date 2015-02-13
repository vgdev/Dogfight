using UnityEngine;
using BaseLib;
using System.Collections.Generic;

/// <summary>
/// Avatar.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Avatar : CachedObject {

	private PlayerAgent agent;
	/// <summary>
	/// Gets or sets the agent.
	/// </summary>
	/// <value>The agent.</value>
	public PlayerAgent Agent {
		get {
			return agent;
		}
		set {
			agent = value;
		}
	}

	/// <summary>
	/// Gets the field controller.
	/// </summary>
	/// <value>The field controller.</value>
	public AbstractDanmakuField FieldController {
		get {
			return agent.FieldController;
		}
	}

	/// <summary>
	/// The normal movement speed.
	/// </summary>
	[SerializeField]
	private float normalMovementSpeed = 5f;

	/// <summary>
	/// The focus movement speed.
	/// </summary>
	[SerializeField]
	private float focusMovementSpeed = 3f;

	/// <summary>
	/// The type of the shot.
	/// </summary>
	[SerializeField]
	private ProjectilePrefab shotType;

	/// <summary>
	/// The shot offset.
	/// </summary>
	[SerializeField]
	private Vector2 shotOffset;

	/// <summary>
	/// The shot velocity.
	/// </summary>
	[SerializeField]
	private float shotVelocity;

	/// <summary>
	/// The charge capacity regen.
	/// </summary>
	[SerializeField]
	private float chargeCapacityRegen;

	/// <summary>
	/// The current charge capacity.
	/// </summary>
	[SerializeField]
	private float currentChargeCapacity;

	/// <summary>
	/// The attack patterns.
	/// </summary>
	[SerializeField]
	private AbstractAttackPattern[] attackPatterns;

	private bool charging;
	/// <summary>
	/// Gets a value indicating whether this instance is charging.
	/// </summary>
	/// <value><c>true</c> if this instance is charging; otherwise, <c>false</c>.</value>
	public bool IsCharging {
		get { 
			return charging; 
		}
	}

	private float chargeLevel = 0f;
	/// <summary>
	/// Gets the current charge level.
	/// </summary>
	/// <value>The current charge level.</value>
	public float CurrentChargeLevel {
		get { return chargeLevel; }
	}

	/// <summary>
	/// Gets the max charge level.
	/// </summary>
	/// <value>The max charge level.</value>
	public int MaxChargeLevel {
		get { return attackPatterns.Length + 1; }
	}

	/// <summary>
	/// Gets the current charge capacity.
	/// </summary>
	/// <value>The current charge capacity.</value>
	public float CurrentChargeCapacity {
		get {
			return currentChargeCapacity;
		}
	}

	private int livesRemaining;
	/// <summary>
	/// Gets the lives remaining.
	/// </summary>
	/// <value>The lives remaining.</value>
	public int LivesRemaining {
		get {
			return livesRemaining;
		}
	}

	/// <summary>
	/// The charge rate.
	/// </summary>
	[SerializeField]
	private float chargeRate = 1.0f;

	/// <summary>
	/// The fire rate.
	/// </summary>
	[SerializeField]
	private float fireRate = 4.0f;
	private float fireDelay;

	/// <summary>
	/// The death cancel radius.
	/// </summary>
	[SerializeField]
	private float deathCancelRadius;

	private bool firing = false;
	/// <summary>
	/// Gets a value indicating whether this instance is firing.
	/// </summary>
	/// <value><c>true</c> if this instance is firing; otherwise, <c>false</c>.</value>
	public bool IsFiring
	{
		get { return firing && !charging; }
	}

	private Vector2 forbiddenMovement = Vector3.zero;
	/// <summary>
	/// Gets a value indicating whether this instance can move horizontal.
	/// </summary>
	/// <value><c>true</c> if this instance can move horizontal; otherwise, <c>false</c>.</value>
	public int CanMoveHorizontal
	{
		get { return -(int)Util.Sign(forbiddenMovement.x); }
	}
	/// <summary>
	/// Gets a value indicating whether this instance can move vertical.
	/// </summary>
	/// <value><c>true</c> if this instance can move vertical; otherwise, <c>false</c>.</value>
	public int CanMoveVertical
	{
		get { return -(int)Util.Sign(forbiddenMovement.y); }
	}

	/// <summary>
	/// Initialize the specified playerField and targetField.
	/// </summary>
	/// <param name="playerField">Player field.</param>
	/// <param name="targetField">Target field.</param>
	public void Initialize(PlayerAgent agent) {
		this.agent = agent;
		for(int i = 0; i < attackPatterns.Length; i++)
			if(attackPatterns[i] != null)
				attackPatterns[i].TargetField = FieldController.TargetField;
	}

	/// <summary>
	/// Move the specified horizontalDirection, verticalDirection, focus and dt.
	/// </summary>
	/// <param name="horizontalDirection">Horizontal direction.</param>
	/// <param name="verticalDirection">Vertical direction.</param>
	/// <param name="focus">If set to <c>true</c> focus.</param>
	/// <param name="dt">Dt.</param>
	public virtual void Move(float horizontalDirection, float verticalDirection, bool focus, float dt = 1.0f) {
		float movementSpeed = (focus) ? focusMovementSpeed : normalMovementSpeed;
		Vector2 dir = new Vector2 (Util.Sign(horizontalDirection), Util.Sign(verticalDirection));
		Vector3 movementVector = movementSpeed * Vector3.one;
		movementVector.x *= (dir.x == Util.Sign(forbiddenMovement.x)) ? 0f : dir.x;
		movementVector.y *= (dir.y == Util.Sign(forbiddenMovement.y)) ? 0f : dir.y;
		movementVector.z = 0f;
		Transform.position += movementVector * dt;
	}

	/// <summary>
	/// Allows the movement.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void AllowMovement(Vector2 direction) {
		if(Util.Sign(direction.x) == Util.Sign(forbiddenMovement.x)) {
			forbiddenMovement.x = 0;
		}
		if(Util.Sign(direction.y) == Util.Sign(forbiddenMovement.y)) {
			forbiddenMovement.y = 0;
		}
	}

	/// <summary>
	/// Forbids the movement.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void ForbidMovement(Vector2 direction) {
		if(direction.x != 0) {
			forbiddenMovement.x = direction.x;
		}
		if(direction.y != 0) {
			forbiddenMovement.y = direction.y;
		}
	}

	/// <summary>
	/// Starts the firing.
	/// </summary>
	public void StartFiring() {
		firing = true;
	}

	/// <summary>
	/// Stops the firing.
	/// </summary>
	public void StopFiring(){
		firing = false;
	}

	/// <summary>
	/// Fire this instance.
	/// </summary>
	public virtual void Fire() {
		if(FieldController != null) {
			Vector2 offset1, offset2, location;
			offset1 = offset2 = shotOffset;
			offset2.x *= -1;
			location = Util.To2D(Transform.position);
			Projectile shot1 = FieldController.SpawnProjectile(shotType, location + offset1, 0f, FieldCoordinateSystem.AbsoluteWorld);
			Projectile shot2 = FieldController.SpawnProjectile(shotType, location + offset2, 0f, FieldCoordinateSystem.AbsoluteWorld);
			shot1.Velocity = shot2.Velocity = shotVelocity;
		}
	}

	/// <summary>
	/// Specials the attack.
	/// </summary>
	/// <param name="level">Level.</param>
	public virtual void SpecialAttack(int level) {
		int index = level - 1;
		if (index >= 0 && index < attackPatterns.Length) {
			if(attackPatterns[index] != null) {
				attackPatterns[index].Fire();
			} else {
				Debug.Log("Null AttackPattern triggered. Make Sure all AttackPatterns are fully implemented");
			}
		}
		chargeLevel -= level;
		currentChargeCapacity -= level;
	}

	/// <summary>
	/// Starts the charging.
	/// </summary>
	public void StartCharging() {
		charging = true;
	}

	/// <summary>
	/// Releases the charge.
	/// </summary>
	public void ReleaseCharge() {
		if(charging) {
			SpecialAttack(Mathf.FloorToInt(CurrentChargeLevel));
		}
		charging = false;
	}

	/// <summary>
	/// Hit this instance.
	/// </summary>
	public void Hit() {
		livesRemaining--;
		float radius = deathCancelRadius * Util.MaxComponent2(Util.To2D(Transform.lossyScale));
		Projectile[] toCanccel = FieldController.GetAllBullets (Transform.position, radius);
		for(int i = 0; i < toCanccel.Length; i++) {
			toCanccel[i].Deactivate();
		}
	}

	/// <summary>
	/// Reset the specified maxLives.
	/// </summary>
	/// <param name="maxLives">Max lives.</param>
	public void Reset(int maxLives) {
		livesRemaining = maxLives;
	}

	/// <summary>
	/// Graze this instance.
	/// </summary>
	public void Graze() {
		//TODO: Implement
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate() {
		float dt = Time.fixedDeltaTime;
		currentChargeCapacity += chargeCapacityRegen * dt;
		if(currentChargeCapacity > MaxChargeLevel) {
			currentChargeCapacity = MaxChargeLevel;
		}
		if(charging) {
			chargeLevel += chargeRate * dt;
			if(chargeLevel > currentChargeCapacity)
				chargeLevel = currentChargeCapacity;
		} else {
			if(firing) {
				fireDelay -= dt;
				if(fireDelay < 0f) {
					Fire ();
					fireDelay = 1f / fireRate;
				}
			}
		}
	}
}
