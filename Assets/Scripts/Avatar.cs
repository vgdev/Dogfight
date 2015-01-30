using UnityEngine;
using BaseLib;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class Avatar : CachedObject
{
	private PlayerFieldController fieldController;
	public PlayerFieldController FieldController {
		get {
			return fieldController;
		}
	}

	private PlayerAgent agent;
	public PlayerAgent Agent {
		get {
			return agent;
		}
		set {
			agent = value;
		}
	}

	[SerializeField]
	private float normalMovementSpeed = 5f;

	[SerializeField]
	private float focusMovementSpeed = 3f;

	[SerializeField]
	private ProjectilePrefab shotType;

	[SerializeField]
	private Vector2 shotOffset;

	[SerializeField]
	private float shotVelocity;

	[SerializeField]
	private float shotDamage;

	[SerializeField]
	private float chargeCapacityRegen;

	[SerializeField]
	private float currentChargeCapacity;

	[SerializeField]
	private AttackPattern[] attackPatterns;

	private bool charging;
	public bool IsCharging
	{
		get { return charging; }
	}

	private float chargeLevel = 0f;
	public float CurrentChargeLevel
	{
		get { return chargeLevel; }
	}

	public int MaxChargeLevel
	{
		get { return attackPatterns.Length + 1; }
	}

	public float CurrentChargeCapacity {
		get {
			return currentChargeCapacity;
		}
	}

	private int livesRemaining;
	public int LivesRemaining {
		get {
			return livesRemaining;
		}
	}

	[HideInInspector]
	[SerializeField]
	private float chargeRate = 1.0f;

	[SerializeField]
	private float fireRate = 4.0f;
	private float fireDelay;

	[SerializeField]
	private float deathCancelRadius;

	private bool firing = false;
	public bool IsFiring
	{
		get { return firing && !charging; }
	}

	private Vector2 forbiddenMovement = Vector3.zero;
	public int CanMoveHorizontal
	{
		get { return -(int)Util.Sign(forbiddenMovement.x); }
	}
	public int CanMoveVertical
	{
		get { return -(int)Util.Sign(forbiddenMovement.y); }
	}

	public void Initialize(PlayerFieldController playerField, PlayerFieldController targetField) {
		this.fieldController = playerField;
		for(int i = 0; i < attackPatterns.Length; i++)
			if(attackPatterns[i] != null)
				attackPatterns[i].Initialize(targetField);
	}

	public virtual void Move(float horizontalDirection, float verticalDirection, bool focus, float dt = 1.0f)
	{
		float movementSpeed = (focus) ? focusMovementSpeed : normalMovementSpeed;
		Vector2 dir = new Vector2 (Util.Sign(horizontalDirection), Util.Sign(verticalDirection));
		Vector3 movementVector = movementSpeed * Vector3.one;
		movementVector.x *= (dir.x == Util.Sign(forbiddenMovement.x)) ? 0f : dir.x;
		movementVector.y *= (dir.y == Util.Sign(forbiddenMovement.y)) ? 0f : dir.y;
		movementVector.z = 0f;
		Transform.position += movementVector * dt;
	}

	public void AllowMovement(Vector2 direction)
	{
		if(Util.Sign(direction.x) == Util.Sign(forbiddenMovement.x))
		{
			forbiddenMovement.x = 0;
		}
		if(Util.Sign(direction.y) == Util.Sign(forbiddenMovement.y))
		{
			forbiddenMovement.y = 0;
		}
	}

	public void ForbidMovement(Vector2 direction)
	{
		if(direction.x != 0)
		{
			forbiddenMovement.x = direction.x;
		}
		if(direction.y != 0)
		{
			forbiddenMovement.y = direction.y;
		}
	}

	public void StartFiring()
	{
		firing = true;
	}

	public void StopFiring()
	{
		firing = false;
	}

	public virtual void Fire() {
		if(FieldController != null) {
			Vector2 offset1, offset2, location;
			offset1 = offset2 = shotOffset;
			offset2.x *= -1;
			location = Util.To2D(Transform.position);
			Projectile shot1 = FieldController.SpawnProjectile(shotType, location + offset1, 0f, PlayerFieldController.CoordinateSystem.AbsoluteWorld);
			Projectile shot2 = FieldController.SpawnProjectile(shotType, location + offset2, 0f, PlayerFieldController.CoordinateSystem.AbsoluteWorld);
			shot1.Velocity = shot2.Velocity = shotVelocity;
			shot1.Damage = shot2.Damage = shotDamage;
		}
	}

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

	public void StartCharging()
	{
		charging = true;
	}

	public void ReleaseCharge()
	{
		if(charging)
		{
			SpecialAttack(Mathf.FloorToInt(CurrentChargeLevel));
		}
		charging = false;
	}

	public void Hit() {
		livesRemaining--;
		float radius = deathCancelRadius * Util.MaxComponent2(Util.To2D(Transform.lossyScale));
		Projectile[] toCanccel = fieldController.GetAllBullets (Transform.position, radius);
		for(int i = 0; i < toCanccel.Length; i++) {
			toCanccel[i].Active = false;
		}
	}

	public void Reset(int maxLives) {
		livesRemaining = maxLives;
	}

	public void Graze() {
	
	}

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
