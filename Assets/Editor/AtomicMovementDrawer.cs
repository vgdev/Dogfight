using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomPropertyDrawer(typeof(FieldMovementPattern.AtomicMovement))]
public class AtomicMovementDrawer : PropertyDrawer {

	float individualHeight = 17f;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		float baseValue = 3 * individualHeight;
		SerializedProperty mt = property.FindPropertyRelative ("movementType");
		string movementType = mt.enumDisplayNames [mt.enumValueIndex];
		switch (movementType) {
			case "Linear":
			default:
				return baseValue;
			case "Curve":
				return baseValue + 2 * individualHeight;

		}
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		float half = position.width / 2;
		Rect movementTypeRect = new Rect (position.x, position.y, half, individualHeight);
		Rect locationTypeRect = new Rect (position.x + half, position.y, half, individualHeight);
		Rect timeRect = new Rect (position.x, position.y + individualHeight, position.width, individualHeight);
		Rect targetLocationRect = new Rect (position.x, position.y +  2 * individualHeight, position.width, individualHeight);
		SerializedProperty mt = property.FindPropertyRelative ("movementType");
		SerializedProperty lt = property.FindPropertyRelative ("locationType");
		SerializedProperty tl = property.FindPropertyRelative ("targetLocation");
		SerializedProperty t = property.FindPropertyRelative ("time");
		EditorGUI.PropertyField (movementTypeRect, mt, GUIContent.none);
		EditorGUI.PropertyField (locationTypeRect, lt, GUIContent.none);
		EditorGUI.PropertyField (timeRect, t);
		EditorGUI.PropertyField (targetLocationRect, tl);
		string movementType = mt.enumDisplayNames [mt.enumValueIndex];
		switch (movementType) {
			case "Linear":
				break;
			case "Curve":
				SerializedProperty c1 = property.FindPropertyRelative ("curveControlPoint1");
				SerializedProperty c2 = property.FindPropertyRelative ("curveControlPoint2");
				Rect point1Rect = new Rect (position.x, position.y +  3 * individualHeight, position.width, individualHeight);
				Rect point2Rect = new Rect (position.x, position.y +  4 * individualHeight, position.width, individualHeight);
				EditorGUI.PropertyField(point1Rect, c1);
				EditorGUI.PropertyField(point2Rect, c2);
				break;
		}
		EditorGUI.EndProperty();
	}
}
