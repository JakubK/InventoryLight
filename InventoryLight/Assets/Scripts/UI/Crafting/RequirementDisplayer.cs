using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using UnityEngine.UI;
using Assets.Scripts.Crafting;
using Assets.Scripts.UI;
using System.Collections.Generic;

public class RequirementDisplayer : MonoBehaviour 
{

	// Use this for initialization
    public ItemDatabase database;

    [SerializeField]
    Transform recipeButtonPrefab;

    [SerializeField]
    Transform Grid;

    [SerializeField]
    Transform ItemPrefab;

	public Transform InventoryHolder;

	public Recipe recipeToMake = null;

    int lastCalledRecipeID = -1;

	void Start () 
    {
        if (recipeButtonPrefab != null)
        {
            for (int i = 0; i < database.Recipes.Count; i++)
            {
                GameObject recipeButtonInstance = Instantiate(recipeButtonPrefab).gameObject;
                recipeButtonInstance.transform.SetParent(Grid.transform);

                RecipeButton rb = recipeButtonInstance.AddComponent<RecipeButton>();
                recipeButtonInstance.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(rb.CallDisplayer));

                if (rb.holdedRecipe == null)
                {
                    rb.holdedRecipe = database.Recipes[rb.transform.GetSiblingIndex()];
                }
                recipeButtonInstance.transform.GetChild(0).GetComponent<Text>().text = database.ItemByID(rb.holdedRecipe.OutputID).Name.ToString();
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public void Call(Recipe rec)
    {
        if (rec.OutputID != lastCalledRecipeID)
        {
            if (transform.childCount != 0)
            {
                foreach (Transform t in transform)
                {
                    Destroy(t.gameObject);
                }
            }

            Dictionary<int, int> IdsDictionary = new Dictionary<int, int>();
            if (transform.childCount == 0)
            {
                foreach (Item i in rec.RequiredData)
                {
                    if (!IdsDictionary.ContainsKey(i.ID))
                    {
                        int count = 0;
                        for (int j = 0; j < rec.RequiredData.Count; j++)
                        {
                            if (rec.RequiredData[j].ID == i.ID)
                            {
                                count++;
                            }
                        }
						IdsDictionary.Add(i.ID, count);
                    }
                }

                foreach (var value in IdsDictionary)
                {
                    GameObject itemInstance = Instantiate(ItemPrefab).gameObject;
                    itemInstance.transform.GetComponent<Image>().sprite = database.ItemByID(value.Key).Icon;
					itemInstance.transform.GetChild(0).GetComponent<Text>().text = InventoryHolder.GetComponent<Inventory>().GetItemCount((int)value.Key) + " / " + value.Value.ToString();
                    Destroy(itemInstance.GetComponent<ItemData>());
                    Destroy(itemInstance.GetComponent<LayoutElement>());

                    itemInstance.transform.SetParent(transform);
                    itemInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				}
                lastCalledRecipeID = rec.OutputID;
				recipeToMake = rec;
            }
        }
    }
    
}