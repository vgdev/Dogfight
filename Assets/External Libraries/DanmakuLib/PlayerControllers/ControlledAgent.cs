using UnityEngine;
using BaseLib;
using System.Collections;

/// <summary>
/// Controlled agent.
/// </summary>
public class ControlledAgent : PlayerAgent  {
	private string horizontalMoveAxis;
	private string verticalMoveAxis;
	private string fireButton;
	private string focusButton;
	private string chargeButton;

	/// <summary>
	/// Initialize the specified fieldController, playerAvatar and targetField.
	/// </summary>
	/// <param name="fieldController">Field controller.</param>
	/// <param name="playerAvatar">Player avatar.</param>
	/// <param name="targetField">Target field.</param>
	public override void Initialize (AbstractDanmakuField fieldController, Avatar playerAvatar) {
		base.Initialize (fieldController, playerAvatar);
		string strPlay = "Player ";
		int playerNumber = ((PhantasmagoriaField)fieldController).PlayerNumber;
		horizontalMoveAxis = "Horizontal Movement " + strPlay + playerNumber;
		verticalMoveAxis = "Vertical Movement " + strPlay + playerNumber;
		focusButton = "Focus " + strPlay + playerNumber;
		fireButton = "Fire " + strPlay + playerNumber;
		chargeButton = "Charge " + strPlay + playerNumber;
	}

	/// <summary>
	/// Update the specified dt.
	/// </summary>
	/// <param name="dt">Dt.</param>
	public override void Update (float dt) {
		Vector2 movementVector = Vector2.zero;
		movementVector.x = Util.Sign(Input.GetAxis (horizontalMoveAxis));
		movementVector.y = Util.Sign(Input.GetAxis (verticalMoveAxis));
		//Debug.Log (horizontalMoveAxis + " : " + Input.GetAxis (horizontalMoveAxis));
		//Debug.Log ("movement vector: " + movementVector.ToString ());
		bool focus = Input.GetButton (focusButton);
		bool fire = Input.GetButton (fireButton);
		bool charge = Input.GetButton (chargeButton);

		PlayerAvatar.Move (movementVector.x, movementVector.y, focus, dt);
		if(fire) {
			PlayerAvatar.StartFiring();
		} else {
			PlayerAvatar.StopFiring();
		}

		if(charge) {
			PlayerAvatar.StartCharging();
		} else if(PlayerAvatar.CurrentChargeLevel != 0) {
			PlayerAvatar.ReleaseCharge();
		}
	}
}
