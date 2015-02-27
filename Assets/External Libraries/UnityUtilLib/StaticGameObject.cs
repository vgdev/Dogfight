using UnityEngine;
using System.Collections;

public class StaticGameObject : MonoBehaviour {

	
	/// <summary>
	/// The keep between scenes.
	/// </summary>
	[SerializeField]
	private bool keepBetweenScenes = true;

	void Awake() {
		if(keepBetweenScenes) {
			DontDestroyOnLoad (this);
		}
	}

}
