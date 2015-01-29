using UnityEngine;
using System.Collections;

public class PlayerScoreIndicator : MultiObjectValueIndicator {

	protected override int GetMaxValue () {
		return gameController.winningScore;
	}

	protected override int GetValue() {
		return ((player) ? gameController.player1 : gameController.player2).score;
	}
}
