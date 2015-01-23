using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class Avatar : BaseLib.CachedObject
{
	[SerializeField]
	private float normalMovementSpeed = 5f;

	[SerializeField]
	private float focusMovementSpeed = 3f;

	private bool charging;
	public bool IsCharging
	{
		get { return charging; }
	}

	private float chargeLevel = 0f;
	public int ChargeLevel
	{
		get { return Mathf.FloorToInt (chargeLevel); }
	}

	[HideInInspector]
	[SerializeField]
	private float chargeRate = 1.0f;

	[HideInInspector]
	[SerializeField]
	private int maxChargeLevel = 3;

	[SerializeField]
	private float fireRate = 4.0f;
	private float fireDelay;

	private bool firing = false;
	public bool IsFiring
	{
		get { return firing && !charging; }
	}

	private Vector2 forbiddenMovement = Vector3.zero;
	public int CanMoveHorizontal
	{
		get { return -(int)Mathf.Sign (forbiddenMovement.x); }
	}
	public int CanMoveVertical
	{
		get { return -(int)Mathf.Sign (forbiddenMovement.y); }
	}

	public virtual void Move(float horizontalDirection, float verticalDirection, bool focus, float dt = 1.0f)
	{
		float movementSpeed = (focus) ? focusMovementSpeed : normalMovementSpeed;
		Vector2 dir = new Vector2 (Mathf.Sign (horizontalDirection), Mathf.Sign(verticalDirection));
		Vector3 movementVector = movementSpeed * Vector3.one;
		movementVector.x *= (dir.x == Mathf.Sign (forbiddenMovement.x)) ? 0f : dir.x;
		movementVector.y *= (dir.y == Mathf.Sign (forbiddenMovement.y)) ? 0f : dir.y;
		movementVector.z = 0f;
		Transform.position += movementVector * dt;
	}

	public void AllowMovement(Vector2 direction)
	{
		if(Mathf.Sign (direction.x) == Mathf.Sign (forbiddenMovement.x))
		{
			forbiddenMovement.x = 0;
		}
		if(Mathf.Sign (direction.y) == Mathf.Sign (forbiddenMovement.y))
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

	public virtual void Fire()
	{

	}

	public virtual void SpecialAttack(int level)
	{

	}

	public void StartCharging()
	{
		charging = true;
	}

	public void ReleaseCharge()
	{
		if(charging)
		{
			SpecialAttack(ChargeLevel);
		}
		charging = false;
	}

	void FixedUpdate()
	{
		float dt = Time.fixedDeltaTime;
		if(charging)
		{
			chargeLevel = (ChargeLevel >= maxChargeLevel) ? (float)maxChargeLevel : chargeLevel + chargeRate * dt;
		}
		else
		{
			if(firing)
			{
				fireDelay -= dt;
				if(fireDelay < 0f)
				{
					Fire ();
					fireDelay = 1f / fireRate;
				}
			}
		}
	}
}
