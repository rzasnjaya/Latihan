using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.TerrainTools;

[CustomEditor(typeof(ItemActions))]
public class ItemActionEditor : Editor
{
    ItemActions source;
    SerializedProperty s_itemDatabase, s_giveItem, s_yesActions, s_noActions, s_amount;

    private void OnEnable()
    {
        source = (ItemActions)target;
        s_itemDatabase = serializedObject.FindProperty("itemDatabase");
        s_giveItem = serializedObject.FindProperty("giveItem");
        s_yesActions = serializedObject.FindProperty("yesActions");
        s_noActions = serializedObject.FindProperty("noActions");
        s_amount = serializedObject.FindProperty("amount");

        if (source.ItemDatabase != null)
            source.ChangeItem(source.ItemDatabase.GetItem(source.itemId));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(s_itemDatabase, new GUIContent("Item Database: "));

        // Fix: check source.ItemDatabase (the actual object), not the SerializedProperty
        if (source.ItemDatabase != null)
        {
            source.itemId = EditorGUILayout.Popup(source.itemId, source.ItemDatabase.ItemsNames.ToArray());
            EditorGUILayout.PropertyField(s_giveItem, new GUIContent("Give Item: "));

            // Fix: guard against CurrentItem being null
            if (source.CurrentItem != null)
                DrawItemEntry(source.CurrentItem);

            EditorExtensions.DrawActionArray(s_yesActions, "Yes Actions: ");
            EditorExtensions.DrawActionArray(s_noActions, "No Actions: ");
        }

        if (GUI.changed)
        {
            if (source.ItemDatabase != null)
                source.ChangeItem(source.ItemDatabase.GetItem(source.itemId));
            EditorUtility.SetDirty(source);

            // Fix: only mark scene dirty when not in Play Mode
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(source.gameObject.scene);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void DrawItemEntry(Item item)
    {
        GUILayout.BeginVertical("box");
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Item Id:" + item.ItemId, GUILayout.Width(75f));
        EditorGUILayout.LabelField("Item Name: " + item.ItemName);
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Item Description: " + item.ItemDesc, GUILayout.Height(70f));
        GUILayout.BeginHorizontal();
        var spriteViewer = AssetPreview.GetAssetPreview(item.ItemSprite);
        GUILayout.Label(spriteViewer);
        if (item.AllowMultiple)
            EditorGUILayout.PropertyField(s_amount);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}