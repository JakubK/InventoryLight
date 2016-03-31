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

        public ItemProperty GetByNameProperty(string name)
        {
            ItemProperty property = new ItemProperty();
            for (int i = 0; i < ItemProperties.Count; i++)
            {
                if (ItemProperties[i].PropertyName == name)
                {
                    property = ItemProperties[i];
                    break;
                }
            }
            return property;
        }

        public Item ItemByID(int ID)
        {
            Item result = null;
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].ID == ID)
                {
                    result = ItemList[i];
                    break;
                }
            }
            return result;
        }

        public Item ItemByName(string Name)
        {
            Item result = null;
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].Name == Name)
                {
                    result = ItemList[i];
                    break;
                }
            }
            return result;
        }
    }
}
