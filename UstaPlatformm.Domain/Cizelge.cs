using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UstaPlatformm.Domain
{
    public class Cizelge
    {
        private readonly Dictionary<DateOnly, List<IsEmri>> _takvim = new();

        public void Ekle(IsEmri isEmri)
        {

            var gun = DateOnly.FromDateTime(isEmri.PlanlananZaman);
            if (!_takvim.ContainsKey(gun))
            {
                _takvim[gun] = new List<IsEmri>();
            }
            _takvim[gun].Add(isEmri);
        }

        // Dizinleyici (Indexer)
        public List<IsEmri> this[DateOnly gun]
        {
            get
            {
                if (_takvim.TryGetValue(gun, out var isler))
                {
                    return isler;
                }
                return new List<IsEmri>();
            }
        }
    }
}
