using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using UnityEditor;

[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemDatabase database = (ItemDatabase) target;

        if (GUILayout.Button("Modify this Item Database"))
        {
            ItemDatabaseWindow.ShowWindow(database);
        }
    }
}
