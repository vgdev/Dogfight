using UnityEngine;
using System.Collections;
using Danmaku2D;
using Danmaku2D.Phantasmagoria;
using UnityUtilLib;

public class PlayerSpawnPass : MonoBehaviour {

	public PhantasmagoriaPlayableCharacter character1;
	public PhantasmagoriaPlayableCharacter character2;

	void Awake() {
		DontDestroyOnLoad (this);
	}

	void OnLevelWasLoaded(int level) {
		var test = FindObjectsOfType<PlayerSpawnPass>();
		Debug.Log (level);
		if (test.Length > 1) {
			Debug.Log("too many");
			Destroy (gameObject);
			return;
		}
		if (level < 1) {
			Debug.Log("not in game");
			return;
		}
		Debug.Log ("hi");
		PhantasmagoriaGameController gc = FindObjectOfType<PhantasmagoriaGameController> ();
		if (gc != null) {
			PhantasmagoriaPlayableCharacter player1 = (PhantasmagoriaPlayableCharacter) gc.player1.Field.SpawnPlayer (character1);
			PhantasmagoriaPlayableCharacter player2 = (PhantasmagoriaPlayableCharacter) gc.player2.Field.SpawnPlayer (character2);
			player1.Agent = new PhantasmagoriaControlledAgent(1);
			player2.Agent = new PhantasmagoriaControlledAgent(2);
		}
		Destroy (gameObject);
	}
}
