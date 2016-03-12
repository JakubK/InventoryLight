using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    }
}
