using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UstaPlatformm.Domain
{
    public class Talep
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Vatandas TalepEden { get; init; }
        public string Aciklama { get; set; } = string.Empty; // UYARI DÜZELTMESİ
        public (int X, int Y) Konum { get; set; }
        public DateTime KayitZamani { get; init; } = DateTime.Now;

        public Talep(Vatandas talepEden)
        {
            TalepEden = talepEden;
        }
    }
}
