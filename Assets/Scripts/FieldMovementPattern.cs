using UnityEngine;
using System;
using System.Collections;
using BaseLib;

/// <summary>
/// Field movement pattern.
/// </summary>
public class FieldMovementPattern : CachedObject {
	
	//TODO: Document Comment
	public enum LocationType { Relative, Absolute, WorldRelative, WorldAbsolute }
	
	//TODO: Document Comment
	public enum MovementType { Linear, Curve }

	/// <summary>
	/// The field.
	/// </summary>
	public PhantasmagoriaField field;

	/// <summary>
	/// The test start point.
	/// </summary>
	public Vector2 testStartPoint;

	/// <summary>
	/// The test invert y.
	/// </summary>
	public bool testInvertY;

	/// <summary>
	/// The test invert x.
	/// </summary>
	public bool testInvertX;

	/// <summary>
	/// The destroy on end.
	/// </summary>
	public bool DestroyOnEnd;

	//TODO: Document Comment
	[Serializable]
	public class AtomicMovement {

		/// <summary>
		/// The type of the location.
		/// </summary>
		public LocationType locationType;

		/// <summary>
		/// The type of the movement.
		/// </summary>
		public MovementType movementType;

		/// <summary>
		/// The time.
		/// </summary>
		public float time;

		/// <summary>
		/// The target location.
		/// </summary>
		public Vector2 targetLocation;

		/// <summary>
		/// The curve control point1.
		/// </summary>
		public Vector2 curveControlPoint1;

		/// <summary>
		/// The curve control point2.
		/// </summary>
		public Vector2 curveControlPoint2;

		/// <summary>
		/// Nexts the location.
		/// </summary>
		/// <returns>The location.</returns>
		/// <param name="field">Field.</param>
		/// <param name="startLocation">Start location.</param>
		public Vector3 NextLocation(PhantasmagoriaField field, Vector3 startLocation) {
			return Interpret (targetLocation, field, startLocation);
		}

		/// <summary>
		/// Nexts the control point1.
		/// </summary>
		/// <returns>The control point1.</returns>
		/// <param name="field">Field.</param>
		/// <param name="startLocation">Start location.</param>
		public Vector3 NextControlPoint1(PhantasmagoriaField field, Vector3 startLocation) {
			return Interpret (curveControlPoint1, field, startLocation);
		}

		/// <summary>
		/// Nexts the control point2.
		/// </summary>
		/// <returns>The control point2.</returns>
		/// <param name="field">Field.</param>
		/// <param name="startLocation">Start location.</param>
		public Vector3 NextControlPoint2(PhantasmagoriaField field, Vector3 startLocation) {
			return Interpret (curveControlPoint2, field, startLocation);
		}

		/// <summary>
		/// Interpret the specified loc, field and startLocation.
		/// </summary>
		/// <param name="loc">Location.</param>
		/// <param name="field">Field.</param>
		/// <param name="startLocation">Start location.</param>
		private Vector3 Interpret(Vector2 loc, PhantasmagoriaField field, Vector3 startLocation) {	
			Vector3 nextLocation = Util.To3D(loc);
			switch(locationType) {
				case FieldMovementPattern.LocationType.Relative:
					return startLocation + field.Relative2Absolute(nextLocation);
				case FieldMovementPattern.LocationType.Absolute:
					return field.Relative2Absolute(nextLocation);
				case FieldMovementPattern.LocationType.WorldRelative:
					return startLocation + nextLocation;
				case FieldMovementPattern.LocationType.WorldAbsolute:
					return startLocation;
				default:
					return Vector3.zero;
			}
		}
	}

	/// <summary>
	/// The movements.
	/// </summary>
	public AtomicMovement[] movements;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		PhantasmagoriaField[] controllers = FindObjectsOfType<PhantasmagoriaField> ();
		float minDist = float.MaxValue;
		for (int i = 0; i < controllers.Length; i++) {
			float dist = (controllers[i].transform.position - transform.position).magnitude;
			if(dist < minDist) {
				field = controllers[i];
				minDist = dist;
			}
		}
		StartCoroutine (Move ());
	}

	/// <summary>
	/// Move this instance.
	/// </summary>
	private IEnumerator Move() {
		for(int i = 0; i < movements.Length; i++) {
			if(movements[i] != null) {
				float totalTime = movements[i].time;
				float t = 0f;
				Vector3 startLocation = Transform.position;
				Vector3 targetLocation = movements[i].NextLocation(field, startLocation);
				Vector3 control1 = movements[i].NextControlPoint1(field, startLocation);
				Vector3 control2 = movements[i].NextControlPoint2(field, startLocation);
				while(t < 1f) {
					if(movements[i].movementType == MovementType.Curve) {
						Transform.position = Util.BerzierCurveVectorLerp(startLocation, targetLocation, control1, control2, t);
					} else { 
						Transform.position = Vector3.Lerp(startLocation, targetLocation, t);
					}
					yield return new WaitForFixedUpdate();
					t += Time.deltaTime / totalTime;
				}
			}
		}
		if (DestroyOnEnd) {
			Destroy (GameObject);
		}
	}
}
