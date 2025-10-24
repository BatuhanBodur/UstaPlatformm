using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UstaPlatformm.Domain
{
    public class IsEmri
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Talep KaynakTalep { get; init; }
        
        public Usta AtananUsta { get; set; } = null!; 
        public DateTime PlanlananZaman { get; set; }
        public decimal TemelUcret { get; set; } = 100.0m;
        public decimal NihaiUcret { get; set; }
        public List<string> UygulananKurallar { get; set; } = new();

        public IsEmri(Talep kaynakTalep)
        {
            KaynakTalep = kaynakTalep;
        }
    }
}
