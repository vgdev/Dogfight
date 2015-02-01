using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Player agent.
/// </summary>
[Serializable]
public abstract class PlayerAgent {

	private Avatar playerAvatar;
	private PlayerFieldController fieldController;

	/// <summary>
	/// Gets the player avatar.
	/// </summary>
	/// <value>The player avatar.</value>
	public Avatar PlayerAvatar {
		get {
			return playerAvatar;
		}
	}

	/// <summary>
	/// Gets the field controller.
	/// </summary>
	/// <value>The field controller.</value>
	public PlayerFieldController FieldController {
		get {
			return fieldController;
		}
	}

	/// <summary>
	/// Initialize the specified fieldController, playerAvatar and targetField.
	/// </summary>
	/// <param name="fieldController">Field controller.</param>
	/// <param name="playerAvatar">Player avatar.</param>
	/// <param name="targetField">Target field.</param>
	public virtual void Initialize(PlayerFieldController fieldController, Avatar playerAvatar, PlayerFieldController targetField) {
		this.fieldController = fieldController;
		this.playerAvatar = playerAvatar;
		playerAvatar.Initialize (fieldController, targetField);
	}

	/// <summary>
	/// Update the specified dt.
	/// </summary>
	/// <param name="dt">Dt.</param>
	public abstract void Update(float dt);
}
