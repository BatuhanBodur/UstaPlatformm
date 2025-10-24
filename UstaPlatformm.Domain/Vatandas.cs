using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatformm.Domain
{
    public class Vatandas
    {
        public int Id { get; init; }
        public string Ad { get; set; } = string.Empty; // UYARI DÜZELTMESİ
        public string Soyad { get; set; } = string.Empty; // UYARI DÜZELTMESİ
        public string Telefon { get; set; } = string.Empty; // UYARI DÜZELTMESİ
        public (int X, int Y) Konum { get; set; }
    }
}
