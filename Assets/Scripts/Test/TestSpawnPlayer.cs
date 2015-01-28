using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameController))]
public class TestSpawnPlayer : TestScript 
{
	public GameObject character1;
	public GameObject character2;

	void Start()
	{
		GameController controller = GetComponent<GameController> ();
		controller.player1.field.SpawnPlayer (character1, new ControlledAgent());
		controller.player2.field.SpawnPlayer (character2, new ControlledAgent());
	}
}