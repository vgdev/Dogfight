using UnityEngine;
using System.Collections;

public class SwitchMenu : MonoBehaviour {

	public GameObject currentMenu;
	public GameObject newMenu;

	void Switch() {
		currentMenu.SetActive (false);
		newMenu.SetActive (true);
	}

}
