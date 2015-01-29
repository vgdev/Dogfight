using UnityEngine;
using BaseLib;
using System.Collections;
using System.Collections.Generic;

public class PlayerFieldController : BaseLib.CachedObject {
	public enum CoordinateSystem { Screen, FieldRelative, AbsoluteWorld }

	public Vector2 playerSpawnLocation;

	private int playerNumber = 1;
	public int PlayerNumber {
		get { 
			return playerNumber; 
		}
		set { 
			playerNumber = value; 
		}
	}

	[SerializeField]
	private float gamePlaneDistance;
	[SerializeField]
	private Camera fieldCamera;

	private Transform cameraTransform;
	public Transform CameraTransform {
		get {
			return cameraTransform;
		}
		set {
			cameraTransform = value;
		}
	}

	private PlayerAgent playerController;
	public PlayerAgent PlayerController {
		get {
			return playerController;
		}
		set {
			playerController = value;
		}
	}

	private PlayerFieldController targetField;
	public PlayerFieldController TargetField {
		set {
			targetField = value;
		}
	}

	public int LivesRemaining {
		get {
			if(playerController.PlayerAvatar != null) {
				return playerController.PlayerAvatar.LivesRemaining;
			} else { 
				Debug.Log("Player Field without Player");
				return int.MinValue;
			}
		}
	}

	[SerializeField]
	private float deathCancelRaidus;

	private Vector3 xDirection;
	private Vector3 yDirection;
	private Vector3 zDirection;
	private Vector3 bottomLeft;

	public float XSize {
		get { return xDirection.magnitude; }
	}
	public float YSize {
		get { return yDirection.magnitude; }
	}
	public Vector3 BottomLeft {
		get { return WorldPoint (new Vector3(0f, 0f, gamePlaneDistance)); }
	}
	public Vector3 BottomRight {
		get { return WorldPoint (new Vector3(1f, 0f, gamePlaneDistance)); }
	}
	public Vector3 TopLeft {
		get { return WorldPoint (new Vector3(0f, 1f, gamePlaneDistance)); }
	}
	public Vector3 TopRight {
		get { return WorldPoint (new Vector3(1f, 1f, gamePlaneDistance)); }
	}
	public Vector3 Center {
		get { return WorldPoint (new Vector3(0.5f, 0.5f, gamePlaneDistance)); }
	}
	public Vector3 Top {
		get { return WorldPoint (new Vector3 (0.5f, 1f, gamePlaneDistance)); }
	}
	public Vector3 Bottom {
		get { return WorldPoint (new Vector3 (0.5f, 0f, gamePlaneDistance));}
	}
	public Vector3 Right {
		get { return WorldPoint (new Vector3 (1f, 0.5f, gamePlaneDistance)); }
	}
	public Vector3 Left {
		get { return WorldPoint (new Vector3 (0f, 0.5f, gamePlaneDistance));}
	}

	[SerializeField]
	private ProjectilePool bulletPool;

	void Start() {
		fieldCamera.orthographic = true;
		cameraTransform = fieldCamera.transform;
		RecomputeWorldPoints ();
	}

	void FixedUpdate ()  {
		if(playerController != null) {
			playerController.Update(Time.fixedDeltaTime);
		}
	}

	public Vector3 WorldPoint(Vector3 fieldPoint) {
		return bottomLeft + Relative2Absolute (fieldPoint);
	}

	public Vector3 FieldPoint(Vector3 worldPoint) {
		Vector3 offset = worldPoint - bottomLeft;
		return new Vector3 (Vector3.Project (offset, xDirection).magnitude, Vector3.Project (offset, yDirection).magnitude, Vector3.Project (offset, zDirection).magnitude);
	}

	public Vector3 Relative2Absolute(Vector3 relativeVector) {
		return relativeVector.x * xDirection + relativeVector.y * yDirection + relativeVector.z * zDirection;
	}

	/// <summary>
	/// Recomputes the bounding area for 
	/// </summary>
	public void RecomputeWorldPoints() {
		bottomLeft = fieldCamera.ViewportToWorldPoint (Vector3.zero);
		Vector3 UL = fieldCamera.ViewportToWorldPoint (Vector3.up);
		Vector3 BR = fieldCamera.ViewportToWorldPoint (Vector3.right);
		xDirection = (BR - bottomLeft);
		yDirection = (UL - bottomLeft);
		zDirection = Vector3.Cross (xDirection, yDirection).normalized;
	}

	/// <summary>
	/// Spawns the player with the given controller
	/// </summary>
	/// <param name="character">Character prefab, defines character behavior and attack patterns.</param>
	/// <param name="controller">Controller for the player, allows for a user to manually control it or let an AI take over.</param>
	public void SpawnPlayer(GameObject character, PlayerAgent controller) {
		Vector3 spawnPos = WorldPoint(Util.To3D(playerSpawnLocation, gamePlaneDistance));
		Avatar avatar = ((GameObject)Instantiate (character, spawnPos, Quaternion.identity)).GetComponent<Avatar>();
		avatar.Reset (5);
		playerController = controller;
		playerController.Initialize(this, avatar, targetField);
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
	/// <param name="extraControllers">Extra ProjectileControllers to change the behavior of the projectile.</param>
	public Projectile SpawnProjectile(ProjectilePrefab prefab, Vector2 location, Quaternion rotation, CoordinateSystem coordSys = CoordinateSystem.Screen, ProjectileController[] extraControllers = null) {
		Vector3 worldLocation = Vector3.zero;
		switch(coordSys) {
			case CoordinateSystem.Screen:
				worldLocation = WorldPoint(new Vector3(location.x, location.y, gamePlaneDistance));
				break;
			case CoordinateSystem.FieldRelative:
				worldLocation = BottomLeft + new Vector3(location.x, location.y, 0f);
				break;
			case CoordinateSystem.AbsoluteWorld:
				worldLocation = location;
				break;
		}
		Projectile projectile = (Projectile)bulletPool.Get (prefab);
		projectile.Transform.position = worldLocation;
		projectile.Transform.rotation = rotation;
		projectile.Active = true;
		return projectile;
	}

	public void SpawnEnemy(GameObject prefab, Vector2 fieldLocation) {
		FieldMovementPattern enemy = ((GameObject)Instantiate (prefab)).GetComponent<FieldMovementPattern> ();
		enemy.Transform.position = WorldPoint (Util.To3D (fieldLocation, gamePlaneDistance));
		enemy.field = this;
	}

	public Projectile[] GetAllBullets(Vector3 position, float radius, int layerMask = 1 << 14) {
		Collider2D[] hits = Physics2D.OverlapCircleAll (position, radius, layerMask);
		List<Projectile> projectiles = new List<Projectile> ();
		for (int i = 0; i < hits.Length; i++) {
			Projectile proj = hits[i].GetComponent<Projectile>();
			if(proj != null) {
				projectiles.Add(proj);
			}
		}
		return projectiles.ToArray ();
	}
}
