using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ItemCategory
    {
        public string Category;

        public ItemCategory(string category)
        {
            this.Category = category;
        }

        public ItemCategory()
        {
            Category = string.Empty;
        }
    }
}
