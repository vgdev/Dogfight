using UnityEngine;
using System;
using System.Collections;
using BaseLib;

public class FieldMovementPattern : CachedObject {
	public enum LocationType { Relative, Absolute, WorldRelative, WorldAbsolute }
	public enum MovementType { Linear, Curve }

	public PlayerFieldController field;
	public Vector2 testStartPoint;
	public bool testInvertY;
	public bool testInvertX;
	public bool DestroyOnEnd;

	[Serializable]
	public class AtomicMovement {
		public LocationType locationType;
		public MovementType movementType;
		public float time;
		public Vector2 targetLocation;
		public Vector2 curveControlPoint1;
		public Vector2 curveControlPoint2;

		public Vector3 NextLocation(PlayerFieldController field, Vector3 startLocation) {
			return Interpret (targetLocation, field, startLocation);
		}

		public Vector3 NextControlPoint1(PlayerFieldController field, Vector3 startLocation) {
			return Interpret (curveControlPoint1, field, startLocation);
		}

		public Vector3 NextControlPoint2(PlayerFieldController field, Vector3 startLocation) {
			return Interpret (curveControlPoint2, field, startLocation);
		}

		private Vector3 Interpret(Vector2 loc, PlayerFieldController field, Vector3 startLocation) {	
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

	public AtomicMovement[] movements;

	void Start() {
		StartCoroutine (Move ());
	}

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
