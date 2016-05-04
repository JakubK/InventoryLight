using UnityEngine;
using System.Collections;
using Assets.Scripts.Crafting;
using Assets.Scripts.UI;

public class CraftButton : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	Transform requirementDisplayerHolder;

	public void Craft()
	{
		if (requirementDisplayerHolder.GetComponent<RequirementDisplayer> ().recipeToMake != null) 
		{
			Recipe targetRecipe = requirementDisplayerHolder.GetComponent<RequirementDisplayer> ().recipeToMake;
			if (requirementDisplayerHolder.GetComponent<RequirementDisplayer> ().InventoryHolder.GetComponent<Inventory> ().canAffordRecipe (targetRecipe.OutputID)) {
				foreach (var i in targetRecipe.RequiredData) {
					requirementDisplayerHolder.GetComponent<RequirementDisplayer> ().InventoryHolder.GetComponent<Inventory> ().RemoveItem (i.ID);
				}
				requirementDisplayerHolder.GetComponent<RequirementDisplayer> ().InventoryHolder.GetComponent<Inventory> ().AddItem (targetRecipe.OutputID);

			}
		}
	}

}
