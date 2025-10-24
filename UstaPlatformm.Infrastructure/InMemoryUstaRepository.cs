using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UstaPlatformm.Domain;

namespace UstaPlatformm.Infrastructure
{
    public class InMemoryUstaRepository : IUstaRepository
    {
        // ... (diğer kodlar aynı) ...
        private static List<Usta> _ustalar = new List<Usta>
        {
            new Usta { Id = 1, Ad = "Ali Veli", UzmanlikAlani = "Tesisat", Puan = 4.5 },
            new Usta { Id = 2, Ad = "Ayşe Yılmaz", UzmanlikAlani = "Elektrik", Puan = 4.8 },
            new Usta { Id = 3, Ad = "Mehmet Demir", UzmanlikAlani = "Tesisat", Puan = 4.2 }
        };

        public Usta? GetById(int id) // DEĞİŞİKLİK BURADA (Usta? oldu)
        {
            return _ustalar.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<Usta> GetAll()
        {
            return _ustalar;
        }

        public Usta FindAvailableUsta()
        {
            return _ustalar.OrderByDescending(u => u.Puan).First();
        }
    }
}
