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
        public string PropertyValue;

        public ItemProperty()
        {
            this.PropertyName = string.Empty;
            this.PropertyValue = string.Empty;
        }

        public ItemProperty(string name, string value)
        {
            this.PropertyName = name;
            this.PropertyValue = value;
        }

        public string GetExisting(string[] array)
        {
            string result = null;
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j] != this.PropertyName)
                {
                    result = array[j];
                    break;
                }
            }
            return result;
        }

    }
}
