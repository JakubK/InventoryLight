using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.Crafting
{
    public class OutputSlot : MonoBehaviour
    {
        protected List<InputSlot> InputSlots;
        ItemDatabase _database;

        void Start()
        {

        }

        public void Call()
        {
            for (int i = 0; i < _database.Recipes.Count; i++)
            {

            }
        }
    }
}
