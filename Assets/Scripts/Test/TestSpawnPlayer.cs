using UnityEngine;
using System.Collections;

/// <summary>
/// Test spawn player.
/// </summary>
[RequireComponent(typeof(PhantasmagoriaGameController))]
public class TestSpawnPlayer : TestScript 
{
	public GameObject character1;
	public GameObject character2;

	void Start() {
		PhantasmagoriaGameController controller = GetComponent<PhantasmagoriaGameController> ();
		controller.player1.Field.SpawnPlayer (character1, new ControlledAgent());
		controller.player2.Field.SpawnPlayer (character2, new ControlledAgent());
	}
}