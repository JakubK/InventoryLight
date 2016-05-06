using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEditor;
using UnityEditorInternal;
using Assets.Scripts.Crafting;
using Assets.Scripts.Currencies;

public class ItemDatabaseWindow : EditorWindow
{
    private static ItemDatabase _database;
    private bool _showList;
    private SerializedObject _serializedObject;

    private int toolbarIndex = 0;
    private int craftToolbarIndex = 0;
    private string[] toolbarStrings = new[] { "Items", "Item Properties & Categories", "Crafting","Currencies" };
    private string[] craftToolbarStrings = new[] {"Recipes", "BluePrints" };
    public string[] CategoryStrings;
    public string[] PropertyStrings;

    private Vector2 itemListVector = Vector2.zero;
    private Vector2 itemDataVector = Vector2.zero;

    private Vector2 categoryListVector = Vector2.zero;
    private Vector2 propertyListVector2 = Vector2.zero;

    protected Item EditedItem;
    protected Item ItemToDelete;
    protected Item CraftDataToRemove;

    protected Recipe EditedRecipe;
    protected Recipe RecipeToRemove;

    protected BluePrint EditedBluePrint;
    protected BluePrint BluePrintToRemove;

    protected Currency EditedCurrency;
    protected Currency CurrencyToRemove;

    private GUIStyle ButtonStyle;

    private string newItemCategory = "";
    private string newItemProperty = "";
    private string newRecipe = "";
    private string newBluePrint = "";
    private string newCurrency = "";

    protected ItemCategory selectedCategory;
    protected ItemProperty propertyToDelete;

    public ReorderableList ItemPropertyList;
    public ReorderableList CurrencyDependencyList;

    public static void ShowWindow(ItemDatabase itemDatabase)
    {
        _database = itemDatabase;
        GetWindow<ItemDatabaseWindow>("ItemDatabaseWindow");
    }

    void OnEnable()
    {
        ReinitializeDatabase();
        ReinitializeProperties();
        _serializedObject = new SerializedObject(_database);

        ButtonStyle = new GUIStyle();
        ButtonStyle.border = new RectOffset(6, 6, 6, 4);
        ButtonStyle.margin = new RectOffset(4, 4, 4, 4);
        ButtonStyle.padding = new RectOffset(6, 6, 3, 3);
    }

    void OnGUI()
    {
        if (_serializedObject != null)
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
            EditorUtility.SetDirty(_database);
            if (toolbarIndex == 0)
            {
                ReinitializeDatabase();
                ReinitializeProperties();
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical("Box", GUILayout.Width(350));
                if (GUILayout.Button("Create New Item"))
                {
                    Item newItem = new Item("New Item " + _database.ItemList.Count.ToString(), "Sample Description");
                    EditedItem = newItem;
                    _database.ItemList.Add(newItem);
                    ReinitializeDatabase();
                }
                GUILayout.BeginVertical("Box", GUILayout.Height(650));

                itemListVector = GUILayout.BeginScrollView(itemListVector, GUILayout.Height(650));
                foreach (Item item in _database.ItemList)
                {
                    GUILayout.Space(5);
                    if (GUILayout.Button(item.Name))
                    {
                        EditedItem = item;
                        EditorGUI.FocusTextInControl(null);
                       
                    }
                }
                GUILayout.EndScrollView();

                GUILayout.EndVertical();
                GUILayout.EndVertical();
                if (EditedItem != null)
                {
                        if (ItemPropertyList == null || !ItemPropertyList.list.Equals(EditedItem.ItemProperties))
                        {
                            if (_database.ItemList.Count > 0)
                            {
                                if (EditedItem.ItemProperties != null)
                                {
                                    ItemPropertyList = new ReorderableList(EditedItem.ItemProperties,
                                        typeof (ItemProperty));

                                    ItemPropertyList.drawElementCallback =
                                        (Rect rect, int index, bool active, bool focused) =>
                                        {
                                            ItemProperty property = EditedItem.ItemProperties[index];
                                            rect.y += 2;

                                                EditorGUI.LabelField(
                                                    new Rect(rect.x + 25, rect.y, 150, EditorGUIUtility.singleLineHeight),property.PropertyName);
                                            property.PropertyValue = EditorGUI.TextField(new Rect(rect.x + 250, rect.y, 150, EditorGUIUtility.singleLineHeight), property.PropertyValue);
                                        };

                                    ItemPropertyList.drawHeaderCallback = (Rect rect) =>
                                    {
                                        EditorGUI.LabelField(new Rect(rect.x + 25, rect.y, rect.width, rect.height),
                                            "Name");
                                        EditorGUI.LabelField(new Rect(rect.x + 250, rect.y, rect.width, rect.height),
                                            "Value");
                                    };

                                    //ItemPropertyList.onAddCallback = (ReorderableList l) =>
                                    //{
                                    //    var index = EditedItem.ItemProperties.Count;
                                    //    ItemProperty property = new ItemProperty();
                                    //    EditedItem.ItemProperties.Add(property);

                                    //    property = EditedItem.ItemProperties[index];

                                    //    property.PropertyName = property.GetExisting(PropertyStrings);
                                    //    property.PropertyValue =
                                    //        _database.GetByNameProperty(property.PropertyName).PropertyValue;

                                    //};

                                    ItemPropertyList.onAddDropdownCallback = (Rect rect, ReorderableList list) =>
                                    {
                                        var menu = new GenericMenu();

                                        for (int i = 0; i < _database.ItemProperties.Count; i++)
                                        {
                                            if (!EditedItem.ItemProperties.Contains(_database.ItemProperties[i]))
                                            {
                                                menu.AddItem(new GUIContent(_database.ItemProperties[i].PropertyName), false, AddProperty, _database.GetByNameProperty(_database.ItemProperties[i].PropertyName));
                                            }
                                        }

                                        menu.ShowAsContext();
                                    };
                                }
                            }
                        }
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
                    EditedItem.Name = GUILayout.TextField(EditedItem.Name, GUILayout.Width(460));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Description");
                    EditedItem.Description = GUILayout.TextArea(EditedItem.Description, GUILayout.Width(460));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Max Stack's length");
                    int.TryParse(GUILayout.TextField(EditedItem.MaxStackCount.ToString(), GUILayout.Width(460)), out EditedItem.MaxStackCount);
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("Box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Icon");
                    EditedItem.Icon = (Sprite)EditorGUILayout.ObjectField(EditedItem.Icon, typeof(Sprite), GUILayout.Width(460));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Prefab");
                    EditedItem.ItemPrefab =
                        (GameObject)
                            EditorGUILayout.ObjectField(EditedItem.ItemPrefab, typeof(GameObject), GUILayout.Width(460));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("On Use AudioClip");
                    EditedItem.OnUseAudioClip =
                        (AudioClip)
                            EditorGUILayout.ObjectField(EditedItem.OnUseAudioClip, typeof(AudioClip),
                                GUILayout.Width(460));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("On Gear AudioClip");
                    EditedItem.OnGearAudioClip =
                        (AudioClip)
                            EditorGUILayout.ObjectField(EditedItem.OnGearAudioClip, typeof(AudioClip),
                                GUILayout.Width(460));
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

                    GUILayout.BeginVertical("Box");
                    //ReorderableListHere
                    GUILayout.Label("Item Properties");
                    EditorUtility.SetDirty(_database);
                    ItemPropertyList.DoLayoutList();
                    EditorUtility.SetDirty(_database);

                    GUILayout.EndVertical();

                    GUILayout.EndScrollView();

                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            else if (toolbarIndex == 1) //ItemCategories & Properties
            {
                ReinitializeDatabase();
                ReinitializeProperties();
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
                        if (GUILayout.Button("Remove"))
                        {
                            foreach (Item i in _database.ItemList)
                            {
                                if (i.ItemProperties.Contains(property))
                                {
                                    i.ItemProperties.Remove(property);
                                }
                            }
                            propertyToDelete = property;
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndScrollView();
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            else if (toolbarIndex == 2)
            {
                GUILayout.BeginHorizontal();
                //EditorUtility.SetDirty(_database);
                //GUILayout.BeginVertical("box");


                //if(GUILayout.Button("New Recipe"))
                //{
                //    _database.Recipes.Add(new Recipe());
                //}
                //foreach (Recipe rec in _database.Recipes)
                //{
                //    GUILayout.Space(15);
                //  int.TryParse(GUILayout.TextField(rec.OutputID.ToString()), out rec.OutputID);
                //  if (GUILayout.Button("Remove Recipe"))
                //  {
                //      if (RecipeToRemove == null)
                //      {
                //          RecipeToRemove = rec;
                //      }
                //  }
                //}

                
                //GUILayout.EndVertical();
                //GUILayout.BeginVertical("box");

                //GUILayout.EndVertical();
                //EditorUtility.SetDirty(_database);
                craftToolbarIndex = GUILayout.Toolbar(craftToolbarIndex, craftToolbarStrings);
                GUILayout.EndHorizontal();
             
                if (craftToolbarIndex == 0) //Recipes
                {

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical("box",GUILayout.Width(250)); //list
                    GUILayout.BeginHorizontal();
                    newRecipe = GUILayout.TextField(newRecipe.ToString(), GUILayout.Width(150));
                    if (GUILayout.Button("Create Recipe"))
                    {
                        if(int.Parse(newRecipe) <= _database.ItemList.Count-1)
                        {
                            bool foundDuplicate = false;
                            for (int i = 0; i < _database.Recipes.Count; i++)
                            {
                                if (_database.Recipes[i].OutputID == int.Parse(newRecipe))
                                {
                                    foundDuplicate = true;
                                }
                            }
                            if (!foundDuplicate)
                            {
                                _database.Recipes.Add(new Recipe(int.Parse(newRecipe)));
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
					try
					{
                    foreach (Recipe rec in _database.Recipes)
	                    {
	                        if (GUILayout.Button(_database.ItemByID(rec.OutputID).Name.ToString()))
							{
								EditedRecipe = rec;
	                        }
	                    }
					}
					catch(Exception ex) {

					}

                    GUILayout.EndVertical();
                    if (EditedRecipe != null)
                    {
						try
						{
							GUILayout.BeginVertical("box");
							GUILayout.BeginHorizontal();
							GUILayout.Label("Result Item");
							GUILayout.Label(_database.ItemByID(EditedRecipe.OutputID).Name.ToString());
							GUILayout.EndHorizontal();

							GUILayout.Label("Ingredients");

							if (GUILayout.Button("New Ingredient"))
							{
								EditedRecipe.RequiredData.Add(_database.ItemByID(0));
							}
							if (EditedRecipe.RequiredData != null)
							{
								foreach (Item i in EditedRecipe.RequiredData)
								{
									GUILayout.Space(10);
									GUILayout.BeginHorizontal();
									if (GUILayout.Button("Remove"))
									{
										CraftDataToRemove = i;
									}
									GUILayout.Label("Ingredient ID");
									int.TryParse(GUILayout.TextField(i.ID.ToString()), out i.ID);

									GUILayout.EndHorizontal();
								}
							}
						}
						catch(Exception ex) {

						}
                        

                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
					
                }
                else if (craftToolbarIndex == 1) //BluePrints
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical("box", GUILayout.Width(250)); //list
                    GUILayout.BeginHorizontal();
                    newBluePrint = GUILayout.TextField(newBluePrint.ToString(), GUILayout.Width(150));
                    EditorUtility.SetDirty(_database);
                    if (GUILayout.Button("Create BluePrint"))
                    {
                        if (int.Parse(newBluePrint) <= _database.ItemList.Count - 1)
                        {
                            bool foundDuplicate = false;
                            for (int i = 0; i < _database.BluePrints.Count; i++)
                            {
                                if (_database.BluePrints[i].OutputID == int.Parse(newBluePrint))
                                {
                                    foundDuplicate = true;
                                }
                            }
                            if (!foundDuplicate)
                            {
                                _database.BluePrints.Add(new BluePrint(int.Parse(newBluePrint)));
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                    foreach (BluePrint bp in _database.BluePrints)
                    {
                        if (GUILayout.Button(_database.ItemByID(bp.OutputID).Name.ToString()))
                        {
                            EditedBluePrint = bp;
                        }
                    }

                    GUILayout.EndVertical();
                    if (EditedBluePrint!= null)
                    {
                        GUILayout.BeginVertical("box", GUILayout.Width(200));
                        GUILayout.BeginHorizontal();
                        GUILayout.EndHorizontal();

                            GUILayout.BeginVertical();

                            GUILayout.BeginHorizontal();
                            EditedBluePrint.x1y1 = GUILayout.TextField(EditedBluePrint.x1y1.ToString(),GUILayout.Width(40));
                            EditedBluePrint.x1y2 = GUILayout.TextField(EditedBluePrint.x1y2.ToString(), GUILayout.Width(40));
                            EditedBluePrint.x1y3 = GUILayout.TextField(EditedBluePrint.x1y3.ToString(), GUILayout.Width(40));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            EditedBluePrint.x2y1 = GUILayout.TextField(EditedBluePrint.x2y1.ToString(), GUILayout.Width(40));
                            EditedBluePrint.x2y2 = GUILayout.TextField(EditedBluePrint.x2y2.ToString(), GUILayout.Width(40));
                            EditedBluePrint.x2y3 = GUILayout.TextField(EditedBluePrint.x2y3.ToString(), GUILayout.Width(40));
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            EditedBluePrint.x3y1 = GUILayout.TextField(EditedBluePrint.x3y1.ToString(), GUILayout.Width(40));
                            EditedBluePrint.x3y2 = GUILayout.TextField(EditedBluePrint.x3y2.ToString(), GUILayout.Width(40));
                            EditedBluePrint.x3y3 = GUILayout.TextField(EditedBluePrint.x3y3.ToString(), GUILayout.Width(40));
                            GUILayout.EndHorizontal();

                            GUILayout.EndVertical();
                        
                        
                        GUILayout.EndVertical();
                      
                    }
                    GUILayout.EndHorizontal();
                }
            }
            else if (toolbarIndex == 3) //Currencies
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical("box", GUILayout.Width(this.position.width / 2));

                GUILayout.BeginHorizontal();
                newCurrency = GUILayout.TextField(newCurrency, GUILayout.Width(125));
                GUILayout.Space(20);
                if (GUILayout.Button("Create new currency", GUILayout.Width(200)))
                {
                    if(!string.IsNullOrEmpty(newCurrency))
                    {
                        if (!_database.CurrencyExist(newCurrency))
                        {
                            Currency currency = new Currency();
                            currency.Name = newCurrency;

                            _database.Currencies.Add(currency);
                        }
                    }
                }
                GUILayout.EndHorizontal();

                foreach (var c in _database.Currencies)
                {
                    GUILayout.Space(10);

                    if (GUILayout.Button(c.Name))
                    {
                        EditedCurrency = _database.CurrencyByName(c.Name);
                        CurrencyDependencyList = new ReorderableList(EditedCurrency.Dependencies, typeof(CurrencyDependency));
                        EditorGUI.FocusTextInControl(null);
                    }
                }
                
                GUILayout.EndVertical();

                GUILayout.BeginVertical("box");

                        if (EditedCurrency != null)
                        {

                        if (CurrencyDependencyList == null)
                        {
                            CurrencyDependencyList = new ReorderableList(EditedCurrency.Dependencies, typeof(CurrencyDependency));
                        }

                        if (CurrencyDependencyList != null || !CurrencyDependencyList.list.Equals(EditedCurrency.Dependencies))
                        {
                            if (EditedCurrency.Dependencies != null)
                            {
                                CurrencyDependencyList.elementHeight = EditorGUIUtility.singleLineHeight * 5f;

                                CurrencyDependencyList.drawHeaderCallback = (Rect rect) =>
                                   {
                                       EditorGUI.LabelField(rect, "Currency Names and Currency Values");
                                   };

                                CurrencyDependencyList.onAddCallback = (ReorderableList l) =>
                                    {
                                        var index = EditedCurrency.Dependencies.Count;
                                        CurrencyDependency dependency = new CurrencyDependency();
                                        EditedCurrency.Dependencies.Add(dependency);

                                        dependency = EditedCurrency.Dependencies[index];

                                        dependency.FirstCurrency = EditedCurrency.Name;
                                        dependency.SecondCurrency = EditedCurrency.Name;

                                        dependency.FirstCurrencyCount = 1;
                                        dependency.SecondCurrencyCount = 1;
                                    };

                                CurrencyDependencyList.drawElementCallback = (Rect rect, int index, bool active, bool focused) =>
                                    {
                                        try
                                        {
                                            CurrencyDependency dependency = EditedCurrency.Dependencies[index];
                                            rect.y += 2;

                                            dependency.FirstCurrency = EditorGUI.TextField(new Rect(rect.x, rect.y, 90, EditorGUIUtility.singleLineHeight), dependency.FirstCurrency);
                                            dependency.SecondCurrency = EditorGUI.TextField(new Rect(rect.x, rect.y + 50, 90, EditorGUIUtility.singleLineHeight), dependency.SecondCurrency);

                                            int.TryParse(EditorGUI.TextField(new Rect(rect.x + 100, rect.y, 90, EditorGUIUtility.singleLineHeight), dependency.FirstCurrencyCount.ToString()), out dependency.FirstCurrencyCount);
                                            int.TryParse(EditorGUI.TextField(new Rect(rect.x + 100, rect.y + 50, 90, EditorGUIUtility.singleLineHeight), dependency.SecondCurrencyCount.ToString()), out dependency.SecondCurrencyCount);
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    };
                            }
                        }
                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Remove"))
                        {
                            CurrencyToRemove = EditedCurrency;
                        }
                        GUILayout.Label("Currency Name:");
                        EditedCurrency.Name = GUILayout.TextField(EditedCurrency.Name, GUILayout.Width(125));
                        GUILayout.EndHorizontal();

                     

                        EditorUtility.SetDirty(_database);
                        CurrencyDependencyList.DoLayoutList();
                        EditorUtility.SetDirty(_database);              
                }

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            _serializedObject.Update();
            _serializedObject.ApplyModifiedProperties();
        }
        if (BluePrintToRemove != null)
        {
            _database.BluePrints.Remove(BluePrintToRemove);
            BluePrintToRemove = null;
        }
        if (CurrencyToRemove != null)
        {
            _database.Currencies.Remove(CurrencyToRemove);
            CurrencyToRemove = null;
            EditedCurrency = null;
        }
        if (CraftDataToRemove != null)
        {
            EditedRecipe.RequiredData.Remove(CraftDataToRemove);
            CraftDataToRemove = null;
        }
        if (RecipeToRemove != null)
        {
            _database.Recipes.Remove(RecipeToRemove);
            RecipeToRemove = null;
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
        EditorUtility.SetDirty(_database);
    }

    private void AddProperty(object userData)
    {
        var data = (ItemProperty) userData;
        ItemProperty property = (ItemProperty)data.Clone();

        property = new ItemProperty(property.PropertyName,property.PropertyValue);

        EditedItem.ItemProperties.Add(property);
        var index = EditedItem.ItemProperties.Count;
    }

    void OnDisable()
    {
        //  ReinitializeDatabase();
        _serializedObject.Update();
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

    private void ReinitializeProperties()
    {
        PropertyStrings = new string[_database.ItemProperties.Count];
        for (int i = 0; i < _database.ItemProperties.Count; i++)
        {
            PropertyStrings[i] = _database.ItemProperties[i].PropertyName;
        }
    }

}