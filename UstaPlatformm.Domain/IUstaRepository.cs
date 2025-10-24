using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatformm.Domain
{
    public interface IUstaRepository
    {
        Usta? GetById(int id); 
        IEnumerable<Usta> GetAll();
        Usta FindAvailableUsta();
    }
}
