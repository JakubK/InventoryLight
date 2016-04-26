using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using UnityEngine.UI;

public class RequirementDisplayer : MonoBehaviour 
{

	// Use this for initialization
    public ItemDatabase database;

    [SerializeField]
    Transform recipeButtonPrefab;

    [SerializeField]
    Transform Grid;

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
	void Update () {
	
	}
}
