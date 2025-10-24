using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatformm.Domain
{
    public interface IWorkOrderRepository
    {
        void Add(IsEmri workOrder);
        IsEmri GetById(Guid id);
    }
}
