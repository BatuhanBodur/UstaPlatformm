using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UstaPlatformm.Domain; 

namespace UstaPlatformm.Rules.Loyalty
{
    public class LoyaltyDiscountRule : IPricingRule
    {
        public string RuleName => "Sadakat İndirimi (%10)";

        public decimal ApplyRule(IsEmri workOrder, decimal currentPrice)
        {
            // Demoda, adı 'Ahmet' olan müşteriyi sadık kabul edelim.
            bool isLoyalCustomer = workOrder.KaynakTalep.TalepEden?.Ad == "Ahmet";

            if (isLoyalCustomer)
            {
                // %10 indirim uygula
                return currentPrice * 0.90m;
            }

            return currentPrice;
        }
    }
}
