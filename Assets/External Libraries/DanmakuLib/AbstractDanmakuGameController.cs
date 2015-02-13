using UnityEngine;
using System.Collections;

public class AbstractDanmakuGameController : MonoBehaviour {

	[SerializeField]
	private ProjectilePool bulletPool;

	public ProjectilePool BulletPool {
		get {
			return bulletPool;
		}
	}
}
