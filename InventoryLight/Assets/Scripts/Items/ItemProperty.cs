using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ItemProperty
    {
        public string PropertyName;
        public object PropertyValue;

        public ItemProperty()
        {
            this.PropertyName = string.Empty;
            this.PropertyValue = string.Empty;
        }

        public ItemProperty(string name, object value)
        {
            this.PropertyName = name;
            this.PropertyValue = value;
        }
    }
}
