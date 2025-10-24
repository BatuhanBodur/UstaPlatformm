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
        public string Ad { get; set; } = string.Empty; 
        public string Soyad { get; set; } = string.Empty; 
        public string Telefon { get; set; } = string.Empty;
        public (int X, int Y) Konum { get; set; }
    }
}
