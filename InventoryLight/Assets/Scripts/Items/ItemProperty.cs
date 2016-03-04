using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Items
{
    public class ItemProperty
    {
        public string PropertyName;
        public string PropertyValue;

        public ItemProperty()
        {

        }

        public ItemProperty(string name, string value)
        {
            this.PropertyName = name;
            this.PropertyValue = value;
        }
    }
}
