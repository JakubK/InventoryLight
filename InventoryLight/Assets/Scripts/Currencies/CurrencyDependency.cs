using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Currencies
{
    [Serializable]
    public class CurrencyDependency
    {
        public string FirstCurrency;
        public string SecondCurrency;

        public int FirstCurrencyCount;
        public int SecondCurrencyCount;
    }
}
