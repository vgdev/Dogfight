using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomPropertyDrawer(typeof(BaseLib.CountdownDelay))]
public class CountdownDelayDrawer : PropertyDrawer {

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		SerializedProperty maxDelayProp = property.FindPropertyRelative ("maxDelay");
		SerializedProperty currentDelayProp = property.FindPropertyRelative ("currentDelay");
		EditorGUI.PropertyField (position, maxDelayProp, label);
		currentDelayProp.floatValue = maxDelayProp.floatValue;
		EditorGUI.EndProperty();
	}
}
