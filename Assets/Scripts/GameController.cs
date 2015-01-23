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

	public PlayerData[] players;
	public int winningScore;
}
