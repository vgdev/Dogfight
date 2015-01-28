using UnityEngine;
using System.Collections;

public class PlayerScoreIndicator : MonoBehaviour 
{
	public GameController gameController;
	public bool player;
	public GameObject baseIndicator;
	public Vector2 additionalOffset;

	private GameObject[] indicators;

	void Start()
	{
		indicators = new GameObject[gameController.winningScore];
		indicators [0] = baseIndicator;
		Vector3 basePosition = baseIndicator.transform.position;
		for(int i = 1; i < indicators.Length; i++)
		{
			indicators[i] = (GameObject)Instantiate(baseIndicator);
			indicators[i].transform.parent = transform;
			indicators[i].transform.position = basePosition + i * new Vector3(additionalOffset.x, additionalOffset.y);
		}
	}

	void Update()
	{		
		Vector3 basePosition = baseIndicator.transform.position;
		for(int i = 0; i < indicators.Length; i++)
		{
			indicators[i].SetActive(((player) ? gameController.player1 : gameController.player2).score > i);
			indicators[i].transform.position = basePosition + i * new Vector3(additionalOffset.x, additionalOffset.y);
		}
	}
}
