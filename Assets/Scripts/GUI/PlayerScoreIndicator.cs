using UnityEngine;
using System.Collections;

/// <summary>
/// Player score indicator.
/// </summary>
public class PlayerScoreIndicator : MultiObjectValueIndicator {

	/// <summary>
	/// Gets the max value.
	/// </summary>
	/// <returns>The max value.</returns>
	protected override int GetMaxValue () {
		return gameController.winningScore;
	}

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	protected override int GetValue() {
		return ((player) ? gameController.player1 : gameController.player2).score;
	}
}
