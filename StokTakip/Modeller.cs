using StokTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakip
{
    public class Modeller
    {
    }
    public class RafKategori
    {
        public Kategori KategoriM { get; set; }
        public IEnumerable<SelectListItem> RafM { get; set; }
    }
    public class Raflar
    {
        public IEnumerable<Raf> RafList { get; set; }
        public IEnumerable<Raf> RafM { get; set; }
        public Kategori Mamul { get; set; }
        public Urun YariMamul { get; set; }
        public IEnumerable<Kategori> MamulM { get; set; }
        public IEnumerable<Kategori> MamulList { get; set; }
    }
}