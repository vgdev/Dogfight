using UnityEngine;
using System.Collections;
using UnityUtilLib;

public class PhantasmagoriaEnemy : BasicEnemy {

	[SerializeField]
	private float deathReflectDuration;

	/// <summary>
	/// The death reflect radius.
	/// </summary>
	[SerializeField]
	private float deathReflectRadius;

	protected override void OnDeath () {
		float radius = Util.MaxComponent3 (Transform.lossyScale) * deathReflectRadius;
		//TODO: FINISH
	}
}
