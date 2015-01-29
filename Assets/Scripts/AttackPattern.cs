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

	private bool attackActive;

	protected abstract void MainLoop(float dt);

	protected void Deactivate() {
		attackActive = false;
	}

	public void Fire() {
		StartCoroutine (Execute ());
	}

	private IEnumerator Execute() {
		float executionTime = 0f, dt;
		attackActive = true;
		while(executionTime < timeout && attackActive) {
			dt = Time.fixedDeltaTime;
			MainLoop(dt);
			yield return new WaitForFixedUpdate();
			executionTime += dt;
		}
	}

	protected float AngleToEntity(Vector3 location, Vector3 entity) {
		//TODO: Implement this function
		throw new NotImplementedException ();
	}
}
