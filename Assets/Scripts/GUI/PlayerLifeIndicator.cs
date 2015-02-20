using UnityEngine;
using System.Collections;
using UnityUtilLib.GUI;

/// <summary>
/// Player life indicator.
/// </summary>
public class PlayerLifeIndicator : MultiObjectValueIndicator {

	private PhantasmagoriaGameController gameControl;

	void Awake() {
		gameControl = (PhantasmagoriaGameController)GameController;
	}

	/// <summary>
	/// Gets the max value.
	/// </summary>
	/// <returns>The max value.</returns>
	protected override int GetMaxValue () {
		return gameControl.MaximumLives;
	}

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	protected override int GetValue () {
		return ((player) ? gameControl.player1 : gameControl.player2).Field.LivesRemaining;
	}
}
