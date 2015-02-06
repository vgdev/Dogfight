using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(Rotation2DAttribute))]
public class Rotation2DDrawer : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);
		EditorGUI.Slider (position, property, 0f, 360f);
		EditorGUI.EndProperty ();
	}
}