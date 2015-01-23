using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameController))]
public class TestSpawnPlayer : TestScript 
{
	public GameObject[] characters;

	void Start()
	{
		GameController controller = GetComponent<GameController> ();
		for(int i = 0; i < characters.Length; i++)
		{
			controller.players[i].field.SpawnPlayer<ControlledAgent>(characters[i]);
		}
	}
}
