using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Danmaku2D;
using Danmaku2D.Phantasmagoria;

public class DogSelect : MonoBehaviour {

	public Image player1Image;
	public Image player2Image;
	public Text player1name;
	public Text player2name;
	public Image player1Lock;
	public Image player2Lock;
	public Color normalColor;
	public Color lockColor;
	public bool player1Locked = false;
	public bool player2Locked = false;
	public string player1ChooseAxis;
	public string player2ChooseAxis;
	public string player1LockAxis;
	public string player2LockAxis;
	public string player1CancelAxis;
	public string player2CancelAxis;
	public int maxSceneNum;

	public PlayerSpawnPass data;

	private bool p1ChooseOld;
	private bool p2ChooseOld;
	private bool p1LockOld;
	private bool p2LockOld;
	private bool p1CancelOld;
	private bool p2CancelOld;
	private PhantasmagoriaPlayableCharacter[] characters;

	public int player1Selected;
	public int player2Selected;

	void P1Lock() {
		player1Locked = !player1Locked;
	}

	void P2Lock() {
		player2Locked = !player2Locked;
	}

	void P1Inc() {
		player1Selected++;
		if(player1Selected >= characters.Length)
			player1Selected = 0;
	}

	void P2Inc() {
		player2Selected++;
		if(player2Selected >= characters.Length)
			player2Selected = 0;
	}

	void P1Dec() {
		player1Selected--;
		if(player1Selected < 0)
			player1Selected = characters.Length - 1;
	}

	void P2Dec() {
		player2Selected--;
		if(player2Selected < 0)
			player2Selected = characters.Length - 1;
	}


	void Awake() {
		var characterList = new List<PhantasmagoriaPlayableCharacter> ();
		Object[] prefabs = Resources.LoadAll ("Characters");
		for (int i = 0; i < prefabs.Length; i++) {
			if(prefabs[i] is GameObject) {
				PhantasmagoriaPlayableCharacter character = (prefabs[i] as GameObject).GetComponent<PhantasmagoriaPlayableCharacter>();
				if(character != null) {
					characterList.Add (character);
				}
			}
		}
		characters = characterList.ToArray ();
	}

	void Update() {	
		bool p1ChooseCurrent = Input.GetButton (player1ChooseAxis);
		bool p2ChooseCurrent = Input.GetButton (player2ChooseAxis);
		bool p1LockCurrent = Input.GetButton (player1LockAxis);
		bool p2LockCurrent = Input.GetButton (player2LockAxis);
		bool p1CancelCurrent = Input.GetButton (player1CancelAxis);
		bool p2CancelCurrent = Input.GetButton (player2CancelAxis);
		if (!p1LockOld && p1LockCurrent)
			player1Locked = true;
		if (!p2LockOld && p2LockCurrent)
			player2Locked = true;
		if (!p1CancelOld && p1CancelCurrent)
			player1Locked = false;
		if (!p2CancelOld && p2CancelCurrent)
			player2Locked = false;
		if (player1Lock != null)
			player1Lock.color = (player1Locked) ? lockColor : normalColor;
		if (player2Lock != null)
			player2Lock.color = (player2Locked) ? lockColor : normalColor;
		if(!p1ChooseOld && p1ChooseCurrent) {
			if(Input.GetAxisRaw(player1ChooseAxis) > 0) {
				player1Selected++;
				if(player1Selected >= characters.Length)
					player1Selected = 0;
			} else {
				player1Selected--;
				if(player1Selected < 0)
					player1Selected = characters.Length - 1;
			}
		}

		if(!p2ChooseOld && p2ChooseCurrent) {
			if(Input.GetAxisRaw(player2ChooseAxis) > 0) {
				player2Selected++;
				if(player2Selected >= characters.Length)
					player2Selected = 0;
			} else {
				player2Selected--;
				if(player2Selected < 0)
					player2Selected = characters.Length - 1;
			}
		}

		if (player1Selected >= 0 && player1Selected < characters.Length) {
			if (player1Image != null)
				player1Image.sprite = characters [player1Selected].characterPortrait;
			if(player1name != null)
				player1name.text = characters[player1Selected].name;
		}
		
		if (player2Selected >= 0 && player2Selected < characters.Length) {
			if (player2Image != null)
				player2Image.sprite = characters [player2Selected].characterPortrait;
			if(player2name != null)
				player2name.text = characters[player2Selected].name;
		}
		
		p1ChooseOld = p1ChooseCurrent;
		p2ChooseOld = p2ChooseCurrent;
		p1LockOld = p1LockCurrent;
		p2LockOld = p2LockCurrent;
		p1CancelOld = p1CancelCurrent;
		p2CancelOld = p2CancelCurrent;

		if (player1Locked && player2Locked) {
			PlayerSpawnPass dataPass = (PlayerSpawnPass)Instantiate(data);
			dataPass.character1 = characters[player1Selected];
			dataPass.character2 = characters[player2Selected];
			Application.LoadLevel(Mathf.FloorToInt(Random.Range(1, maxSceneNum)));
		}
	}
}
