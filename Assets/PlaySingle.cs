using UnityEngine;
using System.Collections;
using UnityUtilLib;

public class PlaySingle : MonoBehaviour {

	public AudioClip clip;

	void Start() {
		AudioManager.PlaySFX (clip);
	}

}
