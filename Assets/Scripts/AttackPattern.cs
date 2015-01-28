using UnityEngine;
using BaseLib;
using System;
using System.Collections;

public abstract class AttackPattern : CachedObject {
	private PlayerFieldController targetField;
	public PlayerFieldController TargetField {
		get {
			return targetField;
		}
	}

	public void Initialize(PlayerFieldController targetField) {
		this.targetField = targetField;
	}

	[SerializeField]
	private float timeout;

	private bool active;

	protected abstract void MainLoop();

	protected void Deactivate() {
		active = false;
	}

	public void Fire() {
		StartCoroutine (Execute ());
	}

	private IEnumerator Execute() {
		float executionTime = 0f;
		active = true;
		while(executionTime < timeout && active) {
			MainLoop();
			yield return new WaitForFixedUpdate();
		}
	}

	protected float AngleToEntity(Vector3 location, Vector3 entity) {
		//TODO: Implement this function
		throw new NotImplementedException ();
	}
}
