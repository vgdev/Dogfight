using UnityEngine;
using System.Collections;

/// <summary>
/// Test spawn player.
/// </summary>
[RequireComponent(typeof(GameController))]
public class TestSpawnPlayer : TestScript 
{
	public GameObject character1;
	public GameObject character2;

	void Start() {
		GameController controller = GetComponent<GameController> ();
		controller.player1.Field.SpawnPlayer (character1, new ControlledAgent());
		controller.player2.Field.SpawnPlayer (character2, new ControlledAgent());
	}
}