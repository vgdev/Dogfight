using UnityEngine;
using System.Collections;
using Danmaku2D;
using UnityUtilLib;

public class SpawnOnOther : MonoBehaviour {

	[SerializeField]
	private GameObject spawn;

	void Awake() {
		Vector3 position = transform.position;
		DanmakuField field = Util.FindClosest<DanmakuField> (position);
		Vector2 location = field.ViewPoint (position);
		field.TargetField.SpawnGameObject (spawn, location);
		Destroy (this);
	}

}
