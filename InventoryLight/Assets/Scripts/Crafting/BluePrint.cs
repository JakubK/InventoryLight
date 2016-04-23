using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Crafting
{
    [Serializable]
    public class BluePrint
    {
        public string x1y1 = string.Empty;
        public string x2y1 = string.Empty;
        public string x3y1 = string.Empty;
        public string x1y2 = string.Empty;
        public string x2y2 = string.Empty;
        public string x3y2 = string.Empty;
        public string x1y3 = string.Empty;
        public string x2y3 = string.Empty;
        public string x3y3 = string.Empty;
        public int OutputID;

        public int blueprintColumns;
        public int blueprintRows;

        public BluePrint(int ID)
        {
            this.OutputID = ID;

            x1y1 = string.Empty;
            x2y1 = string.Empty;
            x3y1 = string.Empty;
            x1y2 = string.Empty;
            x2y2 = string.Empty;
            x3y2 = string.Empty;
            x1y3 = string.Empty;
            x2y3 = string.Empty;
            x3y3 = string.Empty;
        }
    }
}
