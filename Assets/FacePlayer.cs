using UnityEngine;
using System.Collections;
using Danmaku2D;
using UnityUtilLib;

public class FacePlayer : MonoBehaviour {

	private DanmakuField field;

	void Awake() {
		field = Util.FindClosest<DanmakuField> (transform.position);
	}

	void Update() {
		transform.rotation = Quaternion.Euler (0f, 0f, field.AngleTowardPlayer (transform.position));
	}
}
