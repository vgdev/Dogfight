using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class RoundTimer : MonoBehaviour {

	[SerializeField]
	private GameController gameController;
	[SerializeField]
	private Color flashColor;
	[SerializeField]
	private float flashInterval;
	[SerializeField]
	private float flashThreshold;

	private Color normalColor;
	private bool flashState;
	private float flashDelay;
	private GUIText label;

	void Start() {
		label = guiText;
		normalColor = label.color;
		flashState = false;
		flashDelay = 0f;
	}

	void Update() {
		int timeSec = Mathf.FloorToInt (gameController.RemainingRoundTime);
		int seconds = timeSec % 60;
		int minutes = timeSec / 60;
		label.text = minutes.ToString ("D2") + ":" + seconds.ToString ("D2");;
		if (timeSec < flashThreshold) {
			flashDelay -= Time.deltaTime;
			if(flashDelay <= 0) {
				label.color = (flashState) ? flashColor : normalColor;
				flashState = !flashState;
				flashDelay = flashInterval;
			}
		} else {
			label.color = normalColor;
			flashState = false;
			flashDelay = 0f;
		}
	}
}
