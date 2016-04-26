using UnityEngine;
using System.Collections;
using Assets.Scripts.Crafting;

public class RecipeButton : MonoBehaviour 
{
    Transform buttonContainer;
    RequirementDisplayer displayer;
    public Recipe holdedRecipe;

    void Start()
    {
        buttonContainer = transform.parent.parent;

        foreach(Transform t in buttonContainer.parent.transform)
        {
            if(t.GetComponent<RequirementDisplayer>() != null)
            {
                displayer = t.GetComponent<RequirementDisplayer>();
                break;
            }
        }

        if (holdedRecipe == null)
        {
            holdedRecipe = displayer.database.Recipes[transform.GetSiblingIndex()];
        }
    }

    public void CallDisplayer()
    {
        print(displayer.database.ItemByID(holdedRecipe.OutputID).Name);
    }
}
