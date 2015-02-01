using UnityEngine;
using System.Collections;

/// <summary>
/// Player life indicator.
/// </summary>
public class PlayerLifeIndicator : MultiObjectValueIndicator {

	/// <summary>
	/// Gets the max value.
	/// </summary>
	/// <returns>The max value.</returns>
	protected override int GetMaxValue () {
		return gameController.MaximumLives;
	}

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	protected override int GetValue () {
		return ((player) ? gameController.player1 : gameController.player2).Field.LivesRemaining;
	}
}
