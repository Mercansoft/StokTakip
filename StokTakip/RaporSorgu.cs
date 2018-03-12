using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StokTakip
{
    public class RaporSorgu
    {
        public string UrunAdi { get; set; }
        public string UrunKod { get; set; }
        public Nullable<int> StokMiktar { get; set; }
        public Nullable<decimal> AlisFiyat { get; set; }
        public Nullable<decimal> SatisFiyat { get; set; }
        public DateTime GirisTarihi { get; set; }
    }
}