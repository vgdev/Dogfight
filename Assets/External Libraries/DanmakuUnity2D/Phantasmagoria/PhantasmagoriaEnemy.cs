using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaEnemy : BasicEnemy {

		[SerializeField]
		public GameObject onDeathSpawn;

		protected override void OnDeath () {
			Field.SpawnGameObject (onDeathSpawn, transform.position, DanmakuField.CoordinateSystem.World);
		}
	}
}