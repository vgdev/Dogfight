using UnityEngine;
using System.Collections;
using UnityUtilLib;

public class ProjectilePool : GameObjectPool<Projectile, ProjectilePrefab> {

	public override void OnSpawn (Projectile newPO) {
		newPO.SpriteRenderer.sortingOrder = TotalCount;
	}
}
