using UnityEngine;
using BaseLib;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class PlayerFieldController : BaseLib.CachedObject {
	public Vector2 playerSpawnLocation;
	public int playerNumber = 1;

	private Rect cameraArea;
	private Camera playerCamera;
	private PlayerAgent playerController;
	[SerializeField]
	private GameObjectPool bulletPool;

	void Start() {
		playerCamera = GameObject.GetComponent<Camera> ();
	}

	void FixedUpdate ()  {
		if(playerController != null) {
			playerController.Update(Time.fixedDeltaTime);
		}
	}

	public void SpawnPlayer<T>(GameObject character) where T : PlayerAgent, new() {
		Vector3 spawnPos = playerCamera.ViewportToWorldPoint (new Vector3 (playerSpawnLocation.x, playerSpawnLocation.y, 0f));
		spawnPos.z = 0f;
		GameObject avatar = (GameObject)Instantiate (character, spawnPos, Quaternion.identity);
		avatar.transform.parent = Transform;
		playerController = new T();
		playerController.field = this;
		playerController.playerAvatar = avatar.GetComponent<Avatar>();
		playerController.Initialize ();
	}

	public void SpawnProjectile(GameObject prefab, ProjectileController controller, Vector3 fieldLocation, Quaternion rotation) {
		Vector3 worldLocation = playerCamera.ViewportToWorldPoint (fieldLocation);
		Projectile projectile = (Projectile)bulletPool.Get (prefab);
		projectile.Transform.position = worldLocation;
		projectile.Transform.rotation = rotation;
		projectile.Active = true;
	}

	public void SpawnEnemy(GameObject prefab, Vector3 fieldLocation) {

	}
}
