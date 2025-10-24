using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using UstaPlatformm.Domain; 

namespace UstaPlatformm.Rules.Haftasonu
{
    
    public class HaftasonuEkUcretiKurali : IPricingRule
    {
        public string RuleName => "Hafta Sonu Ek Ücreti (+75 TL)";

        public decimal ApplyRule(IsEmri workOrder, decimal currentPrice)
        {
            var planlananGun = workOrder.PlanlananZaman.DayOfWeek;

            if (planlananGun == DayOfWeek.Saturday || planlananGun == DayOfWeek.Sunday)
            {
                return currentPrice + 75.0m;
            }

            return currentPrice;
        }
    }
}