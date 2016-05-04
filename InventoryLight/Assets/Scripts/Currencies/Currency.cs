using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Currencies
{
    [Serializable]
    public class Currency
    {
        public string Name;
        public List<CurrencyDependency> Dependencies;
    }
}
