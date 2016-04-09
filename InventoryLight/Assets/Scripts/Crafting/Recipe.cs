using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI;
using System;
using Assets.Scripts.Items;

namespace Assets.Scripts.Crafting
{
    [Serializable]
    public class Recipe
    {
        public List<Item> RequiredData;
        public int OutputID;
       
        public Recipe(int ID)
        {
            this.OutputID = ID;
        }
    }
}
