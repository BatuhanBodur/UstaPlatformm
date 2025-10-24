using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatformm.Domain
{
    public interface IPricingRule
    {
        string RuleName { get; }
        decimal ApplyRule(IsEmri workOrder, decimal currentPrice);
    }
}
