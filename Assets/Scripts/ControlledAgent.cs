using UnityEngine;
using BaseLib;
using System.Collections;

public class ControlledAgent : PlayerAgent 
{
	private string horizontalMoveAxis;
	private string verticalMoveAxis;
	private string fireButton;
	private string focusButton;
	private string chargeButton;

	public override void Initialize()
	{
		string strPlay = "Player ";
		int playerNumber = field.PlayerNumber;
		horizontalMoveAxis = "Horizontal Movement " + strPlay + playerNumber;
		verticalMoveAxis = "Vertical Movement " + strPlay + playerNumber;
		focusButton = "Focus " + strPlay + playerNumber;
		fireButton = "Fire " + strPlay + playerNumber;
		chargeButton = "Charge " + strPlay + playerNumber;
	}

	public override void Update (float dt)
	{
		Vector2 movementVector = Vector2.zero;
		movementVector.x = Util.Sign(Input.GetAxis (horizontalMoveAxis));
		movementVector.y = Util.Sign(Input.GetAxis (verticalMoveAxis));
		//Debug.Log (horizontalMoveAxis + " : " + Input.GetAxis (horizontalMoveAxis));
		//Debug.Log ("movement vector: " + movementVector.ToString ());
		bool focus = Input.GetAxis (focusButton)  != 0f;
		bool fire = Input.GetAxis (fireButton) != 0f;
		bool charge = Input.GetAxis (chargeButton) != 0f;

		playerAvatar.Move (Input.GetAxis (horizontalMoveAxis), Input.GetAxis (verticalMoveAxis), focus, dt);
		if(fire)
		{
			playerAvatar.StartFiring();
		}
		else
		{
			playerAvatar.StopFiring();
		}

		if(charge)
		{
			playerAvatar.StartCharging();
		}
		else
		{
			if(playerAvatar.ChargeLevel != 0)
			{
				playerAvatar.ReleaseCharge();
			}
		}
	}
}
