using UnityEngine;
using System.Collections;
using UnityUtilLib.GUI;

/// <summary>
/// Player score indicator.
/// </summary>
public class PlayerScoreIndicator : MultiObjectValueIndicator {

	private PhantasmagoriaGameController gameControl;
	
	void Awake() {
		gameControl = (PhantasmagoriaGameController)GameController;
	}


	/// <summary>
	/// Gets the max value.
	/// </summary>
	/// <returns>The max value.</return>
	protected override int GetMaxValue () {
		return gameControl.WinningScore;
	}

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	protected override int GetValue() {
		return ((player) ? gameControl.player1 : gameControl.player2).score;
	}
}
