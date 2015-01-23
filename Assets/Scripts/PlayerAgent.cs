using UnityEngine;
using System;
using System.Collections;

[Serializable]
public abstract class PlayerAgent
{
	public Avatar playerAvatar;
	public PlayerFieldController field;

	public abstract void Initialize();

	public abstract void Update(float dt);
}
