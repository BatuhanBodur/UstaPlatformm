using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatformm.Domain
{
    public class Usta
    {
        public int Id { get; init; }
        public string Ad { get; set; } = string.Empty; // UYARI DÜZELTMESİ
        public string UzmanlikAlani { get; set; } = string.Empty; // UYARI DÜZELTMESİ
        public double Puan { get; set; }
        public Cizelge Cizelgesi { get; set; } = new Cizelge();
    }
}
