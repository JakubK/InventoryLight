using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEditor;
using UnityEditorInternal;

public class ItemDatabaseWindow : EditorWindow
{
    private static ItemDatabase _database;
    private bool _showList;
    private SerializedObject _serializedObject;

    private int toolbarIndex = 0;
    private string[] toolbarStrings = new[] {"Items", "Item Properties & Categories", "Crafting"};
    public string[] CategoryStrings;

    private Vector2 itemListVector = Vector2.zero;
    private Vector2 itemDataVector = Vector2.zero;

    private Vector2 categoryListVector = Vector2.zero;
    private Vector2 propertyListVector2 = Vector2.zero;

    protected Item EditedItem;
    protected Item ItemToDelete;

    private GUIStyle ButtonStyle;

    private string newItemCategory = "";
    private string newItemProperty = "";

    protected ItemCategory selectedCategory;
    protected ItemProperty propertyToDelete;

    public ReorderableList PropertyReorderableList = null;

    public static void ShowWindow(ItemDatabase itemDatabase)
    {
        _database = itemDatabase;
        GetWindow<ItemDatabaseWindow>("ItemDatabaseWindow");
    }

    void OnEnable()
    {
        ReinitializeDatabase();
        _serializedObject = new SerializedObject(_database);

        ButtonStyle = new GUIStyle();
        ButtonStyle.border = new RectOffset(6,6,6,4);
        ButtonStyle.margin = new RectOffset(4,4,4,4);
        ButtonStyle.padding = new RectOffset(6,6,3,3);
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
                ReinitializeDatabase();
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical("Box",GUILayout.Width(350));
                if (GUILayout.Button("Create New Item"))
                {
                    Item newItem = new Item("New Item " + _database.ItemList.Count.ToString(),"Sample Description");
                    EditedItem = newItem;
                    _database.ItemList.Add(newItem);
                    ReinitializeDatabase();
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
                if (EditedItem != null)
                {
                    GUILayout.BeginVertical("Box", GUILayout.Height(650));
                    if (GUILayout.Button("Delete this Item"))
                    {
                        ItemToDelete = EditedItem;
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
                        EditedItem.Description = GUILayout.TextArea(EditedItem.Description, GUILayout.Width(460));
                        GUILayout.EndHorizontal();

                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Max Stack's length");
                        int.TryParse(GUILayout.TextField(EditedItem.MaxStackCount.ToString(),GUILayout.Width(460)), out EditedItem.MaxStackCount);
                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("Box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Icon");
                    EditedItem.Icon = (Sprite) EditorGUILayout.ObjectField(EditedItem.Icon, typeof (Sprite),GUILayout.Width(460));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Prefab");
                    EditedItem.ItemPrefab =
                        (GameObject)
                            EditorGUILayout.ObjectField(EditedItem.ItemPrefab, typeof(GameObject), GUILayout.Width(460));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Item Category");
                    EditedItem.categoryChoiceID = EditorGUILayout.Popup(EditedItem.categoryChoiceID, CategoryStrings);
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    //ReorderableListHere
                    GUILayout.EndVertical();

                    GUILayout.EndScrollView();

                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            else if (toolbarIndex == 1) //ItemCategories & Properties
            {
                ReinitializeDatabase();
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical("Box", GUILayout.Width(250));
                newItemCategory = GUILayout.TextField(newItemCategory, GUILayout.Width(250));
                if (GUILayout.Button("Create Item Category"))
                {
                    if (!_database.CategoryExist(newItemCategory))
                    {
                        _database.ItemCategories.Add(new ItemCategory(newItemCategory));
                    }
                }
                if (_database.ItemCategories.Count != 0)
                {
                    categoryListVector = GUILayout.BeginScrollView(categoryListVector, GUILayout.Width(250));

                    GUILayout.BeginVertical("Box");
                    foreach (ItemCategory category in _database.ItemCategories)
                    {
                        GUILayout.Space(10);
                        if (GUILayout.Button(category.Category))
                        {
                            selectedCategory = category;
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                  }
                GUILayout.Space(20);
                GUILayout.BeginVertical("Box");
                if (GUILayout.Button("Remove"))
                {
                    _database.ItemCategories.Remove(selectedCategory);
                    selectedCategory = null;
                }
                GUILayout.EndVertical();
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box");
                GUILayout.BeginHorizontal();
                newItemProperty = GUILayout.TextField(newItemProperty, GUILayout.Width(250));
                if (GUILayout.Button("Create Item Property"))
                {
                    if (!_database.PropertyExist(newItemProperty))
                    {
                        _database.ItemProperties.Add(new ItemProperty(newItemProperty, string.Empty));
                    }
                }
                GUILayout.EndHorizontal();
                if (_database.ItemProperties.Count != 0)
                {
                    propertyListVector2 = GUILayout.BeginScrollView(propertyListVector2, GUILayout.Height(650));
                    GUILayout.BeginVertical("Box");
                    foreach (ItemProperty property in _database.ItemProperties)
                    {
                        GUILayout.Space(10);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Name:");
                        property.PropertyName = GUILayout.TextField(property.PropertyName, GUILayout.Width(450));
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Default Value:");
                        property.PropertyValue = GUILayout.TextField(property.PropertyValue.ToString(), GUILayout.Width(450));
                        GUILayout.EndHorizontal();
                        if(GUILayout.Button("Remove"))
                        {
                            propertyToDelete = property;
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                }
                                
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            _serializedObject.Update();
            _serializedObject.ApplyModifiedProperties();
        }
        if (ItemToDelete != null)
        {
                if (ItemToDelete.ID != 0)
                {
                    EditedItem = _database.ItemList[ItemToDelete.ID - 1];
                }
                else if (_database.ItemList.Count == 1)
                {
                    EditedItem = null;
                }
                else
                {
                    EditedItem = _database.ItemList[ItemToDelete.ID + 1];
                }

                _database.ItemList.Remove(ItemToDelete);

                ItemToDelete = null;
                ReinitializeDatabase(); 
        }
        if (propertyToDelete != null)
        {
            _database.ItemProperties.Remove(propertyToDelete);
            propertyToDelete = null;
        }
    }

    void OnDisable()
    {
        ReinitializeDatabase();
        _serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    private void ReinitializeDatabase()
    {
        CategoryStrings = new string[_database.ItemCategories.Count];
        for (int i = 0; i < _database.ItemList.Count; i++)
        {
            _database.ItemList[i].ID = i;
        }
        for (int j = 0; j < _database.ItemCategories.Count; j++)
        {
            CategoryStrings[j] = _database.ItemCategories[j].Category;
        }
    }
}

