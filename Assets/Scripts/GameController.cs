using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour 
{
	[Serializable]
	public class PlayerData {
		[SerializeField]
		private PlayerFieldController field;
		public PlayerFieldController Field {
			get {
				return field;
			}
		}

		public int score;
	}

	public PlayerData player1;
	public PlayerData player2;

	[SerializeField]
	public int winningScore;
	public int WinningScore {
		get {
			return winningScore;
		}
	}

	[SerializeField]
	private int maximumLives;
	public int MaximumLives {
		get {
			return maximumLives;
		}
	}


	[SerializeField]
	private float roundTime;
	private float roundTimeRemaining;
	public float RemainingRoundTime {
		get {
			return roundTimeRemaining;
		}
	}

	[SerializeField]
	public GameObject guardian;

	private bool guardianSummoned;

	void Awake() {
		//Physics2D.raycastsHitTriggers = true;
		if(player1.Field != null && player2.Field != null) {
			player1.Field.TargetField = player2.Field;
			player2.Field.TargetField = player1.Field;
			player1.Field.PlayerNumber = 1;
			player2.Field.PlayerNumber = 2;
			StartRound();
		}
	}

	void FixedUpdate() {
		bool reset = false;
		if (player1.Field.LivesRemaining <= 0) {
			player1.score++;
			reset = true;
		}
		if (player2.Field.LivesRemaining <= 0) {
			player2.score++;
			reset = true;
		}
		if(player1.score >= winningScore && player2.score >= winningScore) {
			//Signal Sudden Death
			player1.score = player2.score = 0;
			winningScore = 0;
		} else if(player1.score >= winningScore) {
			//Declare Player 1 the winner
		} else if(player1.score >= winningScore) {
			//Declare Player 2 the winner
		} else if(reset) {
			//Reset both Fields
		}
		roundTimeRemaining -= Time.fixedDeltaTime;
		if (roundTimeRemaining < 0f && !guardianSummoned) {
			SpawnEnemy(guardian, new Vector2(0.5f, 1.1f));
			guardianSummoned = true;
		}
	}

	public void StartRound() {
		roundTimeRemaining = roundTime;
		guardianSummoned = false;
	}

	public void Reset() {

	}

	public void SpawnEnemy(GameObject prefab, Vector2 relativeLocations) {
		if(player1.Field != null && player2.Field != null) {
			player1.Field.SpawnEnemy(prefab, relativeLocations);
			player2.Field.SpawnEnemy(prefab, relativeLocations);
		}
	}
}
