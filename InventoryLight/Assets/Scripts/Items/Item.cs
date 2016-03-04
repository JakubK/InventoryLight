using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Items
{
    public class Item
    {
        public int ID;
        public string Name;
        public string Description;

        public List<ItemProperty> ItemProperties;
        public ItemCategory itemCategory;

        public Item()
        {

        }
    }
}
