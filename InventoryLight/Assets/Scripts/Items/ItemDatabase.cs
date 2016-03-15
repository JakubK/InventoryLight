using UnityEngine;
using System.Collections.Generic;
using System.Deployment.Internal;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(menuName = "ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        [HideInInspector]
        public List<Item> ItemList = new List<Item>();

        [HideInInspector]
        public List<ItemProperty> ItemProperties = new List<ItemProperty>();

        [HideInInspector]
        public List<ItemCategory> ItemCategories = new List<ItemCategory>();

        public bool CategoryExist(string name)
        {
            bool result = false;
                for (int i = 0; i < ItemCategories.Count; i++)
                {
                    if (ItemCategories[i].Category == name)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                    }
                }
            return result;
        }

        public bool PropertyExist(string name)
        {
            bool result = false;
            for (int i = 0; i < ItemProperties.Count; i++)
            {
                if (ItemProperties[i].PropertyName == name)
                {
                    result = true;
                    break;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
