using UnityEngine;
using System;
using System.Collections;

[Serializable]
public abstract class PlayerAgent
{
	private Avatar playerAvatar;
	private PlayerFieldController fieldController;

	public Avatar PlayerAvatar {
		get {
			return playerAvatar;
		}
	}

	public PlayerFieldController FieldController {
		get {
			return fieldController;
		}
	}

	public virtual void Initialize(PlayerFieldController fieldController, Avatar playerAvatar, PlayerFieldController targetField) {
		this.fieldController = fieldController;
		this.playerAvatar = playerAvatar;
		playerAvatar.Initialize (fieldController, targetField);
	}

	public abstract void Update(float dt);
}
