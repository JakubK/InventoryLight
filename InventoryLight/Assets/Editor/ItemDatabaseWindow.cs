using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEditor;

public class ItemDatabaseWindow : EditorWindow
{
    private static ItemDatabase _database;
    private bool _showList;
    private SerializedObject _serializedObject;

    private int toolbarIndex = 0;
    private string[] toolbarStrings = new[] {"Items", "Item Properties & Categories", "Crafting"};

    private Vector2 itemListVector = Vector2.zero;
    private Vector2 itemDataVector = Vector2.zero;

    protected Item EditedItem;
    protected Item ItemToDelete;

    private GUIStyle ButtonStyle;

    public static void ShowWindow(ItemDatabase itemDatabase)
    {
        _database = itemDatabase;
        GetWindow<ItemDatabaseWindow>("ItemDatabaseWindow");
    }

    void OnEnable()
    {
        SortItemList();
        _serializedObject = new SerializedObject(_database);

        ButtonStyle = new GUIStyle();
        ButtonStyle.border = new RectOffset(6,6,6,4);
        ButtonStyle.margin = new RectOffset(4,4,4,4);
        ButtonStyle.padding = new RectOffset(6,6,3,3);
        //ButtonStyle.normal.background = null;

    }

    void OnGUI()
    {
        if(_serializedObject != null)
        {
            if (_database.ItemList == null)
            {
                _database.ItemList = new List<Item>();
            }
          
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);
            GUILayout.EndHorizontal();

            //Items
            if (toolbarIndex == 0)
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical("Box",GUILayout.Width(350));
                if (GUILayout.Button("Create New Item"))
                {
                    Item newItem = new Item("New Item " + _database.ItemList.Count.ToString(),"Sample Description");
                    EditedItem = newItem;
                    _database.ItemList.Add(newItem);
                }
                    GUILayout.BeginVertical("Box",GUILayout.Height(650));

                    itemListVector = GUILayout.BeginScrollView(itemListVector, GUILayout.Height(650));
                    foreach (Item item in _database.ItemList)
                    {
                        GUILayout.Space(5);
                        if (GUILayout.Button(item.Name))
                        {
                            EditedItem = item;
                        }
                    }
                    GUILayout.EndScrollView();

                    GUILayout.EndVertical();
                GUILayout.EndVertical();
                if (EditedItem != null && EditedItem != ItemToDelete)
                {
                    GUILayout.BeginVertical("Box", GUILayout.Height(650));
                    if (GUILayout.Button("Delete this Item"))
                    {
                        ItemToDelete = EditedItem;
                        EditedItem = null;
                    }

                    itemDataVector = GUILayout.BeginScrollView(itemDataVector, GUILayout.Height(650));

                    GUILayout.BeginVertical("Box");

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("ID: ");
                        GUILayout.Label(EditedItem.ID.ToString());
                        GUILayout.EndHorizontal();

                        GUILayout.Space(10);

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Name");
                        EditedItem.Name = GUILayout.TextField(EditedItem.Name,GUILayout.Width(460));
                        GUILayout.EndHorizontal();

                        GUILayout.Space(10);

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Description");
                        EditedItem.Description = GUILayout.TextArea(EditedItem.Description,GUILayout.Width(460));
                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    GUILayout.EndScrollView();

                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }

            _serializedObject.Update();
            _serializedObject.ApplyModifiedProperties();
        }
        if (ItemToDelete != null)
        {
            _database.ItemList.Remove(ItemToDelete);
            ItemToDelete = null;
            SortItemList();
        }
    }

    void OnDisable()
    {
        SortItemList();
        _serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    private void SortItemList()
    {
        for (int i = 0; i < _database.ItemList.Count; i++)
        {
            _database.ItemList[i].ID = i;
        }
    }
}

