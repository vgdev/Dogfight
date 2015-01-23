using UnityEngine;
using System;
using System.Collections;

[Serializable]
public abstract class PlayerAgent
{
	public Avatar playerAvatar;
	public PlayerFieldController field;

	public PlayerAgent()
	{
	}

	public abstract void Update(float dt);
}
