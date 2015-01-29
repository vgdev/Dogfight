using UnityEngine;
using System.Collections;

public class PlayerLifeIndicator : MultiObjectValueIndicator {

	protected override int GetMaxValue () {
		return gameController.MaximumLives;
	}

	protected override int GetValue () {
		return ((player) ? gameController.player1 : gameController.player2).Field.LivesRemaining;
	}
}
