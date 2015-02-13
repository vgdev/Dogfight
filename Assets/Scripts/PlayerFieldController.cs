using UnityEngine;
using BaseLib;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Player field controller.
/// </summary>
public class PlayerFieldController : BaseLib.CachedObject {
	
	public enum CoordinateSystem { Screen, FieldRelative, AbsoluteWorld }

	/// <summary>
	/// The player spawn location.
	/// </summary>
	public Vector2 playerSpawnLocation;

	private int playerNumber = 1;
	/// <summary>
	/// Gets or sets the player number.
	/// </summary>
	/// <value>The player number.</value>
	public int PlayerNumber {
		get { 
			return playerNumber; 
		}
		set { 
			playerNumber = value; 
		}
	}

	/// <summary>
	/// The game plane distance.
	/// </summary>
	[SerializeField]
	private float gamePlaneDistance;

	/// <summary>
	/// The field camera.
	/// </summary>
	[SerializeField]
	private Camera fieldCamera;

	private Transform cameraTransform;
	/// <summary>
	/// Gets or sets the camera transform.
	/// </summary>
	/// <value>The camera transform.</value>
	public Transform CameraTransform {
		get {
			return cameraTransform;
		}
		set {
			cameraTransform = value;
		}
	}

	private PlayerAgent playerController;
	/// <summary>
	/// Gets or sets the player controller.
	/// </summary>
	/// <value>The player controller.</value>
	public PlayerAgent PlayerController {
		get {
			return playerController;
		}
		set {
			playerController = value;
		}
	}

	private PlayerFieldController targetField;
	/// <summary>
	/// Sets the target field.
	/// </summary>
	/// <value>The target field.</value>
	public PlayerFieldController TargetField {
		set {
			targetField = value;
		}
	}

	/// <summary>
	/// Gets the lives remaining.
	/// </summary>
	/// <value>The lives remaining.</value>
	public int LivesRemaining {
		get {
			if(playerController != null && playerController.PlayerAvatar != null) {
				return playerController.PlayerAvatar.LivesRemaining;
			} else {
				Debug.Log("Player Field without Player");
				return int.MinValue;
			}
		}
	}

	/// <summary>
	/// Gets the player position.
	/// </summary>
	/// <value>The player position.</value>
	public Vector3 PlayerPosition {
		get {
			if(playerController != null && playerController.PlayerAvatar != null) {
				return playerController.PlayerAvatar.Transform.position;
			} else {
				Debug.Log("Player Field without Player");
				return Vector3.zero;
			}
		}
	}

	/// <summary>
	/// Angles the toward player.
	/// </summary>
	/// <returns>The toward player.</returns>
	/// <param name="startLocation">Start location.</param>
	public float AngleTowardPlayer(Vector3 startLocation) {
		return Projectile.AngleBetween2D (startLocation, PlayerPosition);
	}

	/// <summary>
	/// The death cancel raidus.
	/// </summary>
	[SerializeField]
	private float deathCancelRaidus;

	private Vector3 xDirection;
	private Vector3 yDirection;
	private Vector3 zDirection;
	private Vector3 bottomLeft;

	/// <summary>
	/// Gets the size of the X.
	/// </summary>
	/// <value>The size of the X.</value>
	public float XSize {
		get { return xDirection.magnitude; }
	}

	/// <summary>
	/// Gets the size of the Y.
	/// </summary>
	/// <value>The size of the Y.</value>
	public float YSize {
		get { return yDirection.magnitude; }
	}

	/// <summary>
	/// Gets the bottom left.
	/// </summary>
	/// <value>The bottom left.</value>
	public Vector3 BottomLeft {
		get { return WorldPoint (new Vector3(0f, 0f, gamePlaneDistance)); }
	}

	/// <summary>
	/// Gets the bottom right.
	/// </summary>
	/// <value>The bottom right.</value>
	public Vector3 BottomRight {
		get { return WorldPoint (new Vector3(1f, 0f, gamePlaneDistance)); }
	}

	/// <summary>
	/// Gets the top left.
	/// </summary>
	/// <value>The top left.</value>
	public Vector3 TopLeft {
		get { return WorldPoint (new Vector3(0f, 1f, gamePlaneDistance)); }
	}

	/// <summary>
	/// Gets the top right.
	/// </summary>
	/// <value>The top right.</value>
	public Vector3 TopRight {
		get { return WorldPoint (new Vector3(1f, 1f, gamePlaneDistance)); }
	}

	/// <summary>
	/// Gets the center.
	/// </summary>
	/// <value>The center.</value>
	public Vector3 Center {
		get { return WorldPoint (new Vector3(0.5f, 0.5f, gamePlaneDistance)); }
	}

	/// <summary>
	/// Gets the top.
	/// </summary>
	/// <value>The top.</value>
	public Vector3 Top {
		get { return WorldPoint (new Vector3 (0.5f, 1f, gamePlaneDistance)); }
	}

	/// <summary>
	/// Gets the bottom.
	/// </summary>
	/// <value>The bottom.</value>
	public Vector3 Bottom {
		get { return WorldPoint (new Vector3 (0.5f, 0f, gamePlaneDistance));}
	}

	/// <summary>
	/// Gets the right.
	/// </summary>
	/// <value>The right.</value>
	public Vector3 Right {
		get { return WorldPoint (new Vector3 (1f, 0.5f, gamePlaneDistance)); }
	}

	/// <summary>
	/// Gets the left.
	/// </summary>
	/// <value>The left.</value>
	public Vector3 Left {
		get { return WorldPoint (new Vector3 (0f, 0.5f, gamePlaneDistance));}
	}

	/// <summary>
	/// The bullet pool.
	/// </summary>
	[SerializeField]
	private ProjectilePool bulletPool;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		fieldCamera.orthographic = true;
		cameraTransform = fieldCamera.transform;
		RecomputeWorldPoints ();
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate ()  {
		if(playerController != null) {
			playerController.Update(Time.fixedDeltaTime);
		}
	}

	/// <summary>
	/// Worlds the point.
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="fieldPoint">Field point.</param>
	public Vector3 WorldPoint(Vector3 fieldPoint) {
		return bottomLeft + Relative2Absolute (fieldPoint);
	}

	/// <summary>
	/// Fields the point.
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="worldPoint">World point.</param>
	public Vector3 FieldPoint(Vector3 worldPoint) {
		Vector3 offset = worldPoint - bottomLeft;
		return new Vector3 (Vector3.Project (offset, xDirection).magnitude, Vector3.Project (offset, yDirection).magnitude, Vector3.Project (offset, zDirection).magnitude);
	}

	/// <summary>
	/// Relative2s the absolute.
	/// </summary>
	/// <returns>The absolute.</returns>
	/// <param name="relativeVector">Relative vector.</param>
	public Vector3 Relative2Absolute(Vector3 relativeVector) {
		return relativeVector.x * xDirection + relativeVector.y * yDirection + relativeVector.z * zDirection;
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void Reset() {
		playerController.PlayerAvatar.Reset (5);
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
		avatar.Transform.parent = Transform;
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
	public Projectile SpawnProjectile(ProjectilePrefab prefab, Vector2 location, float rotation, CoordinateSystem coordSys = CoordinateSystem.Screen, ProjectileController[] extraControllers = null) {
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
		projectile.Transform.rotation = Quaternion.Euler(0f, 0f, rotation);
		projectile.Activate ();
		return projectile;
	}

	/// <summary>
	/// Spawns the enemy.
	/// </summary>
	/// <param name="prefab">Prefab.</param>
	/// <param name="fieldLocation">Field location.</param>
	public void SpawnEnemy(GameObject prefab, Vector2 fieldLocation) {
		FieldMovementPattern enemy = ((GameObject)Instantiate (prefab)).GetComponent<FieldMovementPattern> ();
		enemy.Transform.position = WorldPoint (Util.To3D (fieldLocation, gamePlaneDistance));
	}

	/// <summary>
	/// Gets all bullets.
	/// </summary>
	/// <returns>The all bullets.</returns>
	/// <param name="position">Position.</param>
	/// <param name="radius">Radius.</param>
	/// <param name="layerMask">Layer mask.</param>
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
