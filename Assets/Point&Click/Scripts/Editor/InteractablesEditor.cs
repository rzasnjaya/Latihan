using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactable))]
public class InteractablesEditor : Editor
{
    SerializedProperty s_actions, s_distancePosition;

    private void OnEnable()
    {
        s_actions = serializedObject.FindProperty("actions");
        s_distancePosition = serializedObject.FindProperty("distancePosition");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical("box");

        EditorGUILayout.PropertyField(s_distancePosition, new GUIContent("Distance Position: "));

        EditorExtensions.DrawActionsArray(s_actions, "Actions: ");

        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}