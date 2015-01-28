using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour 
{
	[Serializable]
	public class PlayerData
	{
		public PlayerFieldController field;
		public int score;
	}

	public PlayerData player1;
	public PlayerData player2;
	public int winningScore;

	void Awake() {
		if(player1.field != null && player2.field != null) {
			player1.field.TargetField = player2.field;
			player2.field.TargetField = player1.field;
			player1.field.PlayerNumber = 1;
			player2.field.PlayerNumber = 2;
		}
	}
}
