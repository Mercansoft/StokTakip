using StokTakip.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakip.Controllers
{
    public class UrunController : Controller
    {
        StokTakipEntities db = new StokTakipEntities();
        string TamYolYeri;
        string DosyaAdi;
        string uzanti;
        string barcode = string.Empty;
        public ActionResult Index()
        {
            string Kullanici = CookieSorgula("_id");
            string Yonetici = CookieSorgula("id");
            if (Yonetici != null || Kullanici != null)
            {
                Session["id"] = CookieSorgula("id");
                Session["isim"] = CookieSorgula("isim");
                Session["_id"] = CookieSorgula("_id");
                Session["_isim"] = CookieSorgula("_isim");
                try
                {
                    Session["resim"] = CookieSorgula("resim");
                    Session["_resim"] = CookieSorgula("_resim");
                }
                catch (Exception)
                {
                    
                }
                var sayi = db.Urun.ToList().Sum(x => x.StokMiktar);
                Session["ToplamStokMiktar"] = sayi.Value.ToString();
                var kat = db.Urun.OrderByDescending(x => x.UrunID).ToList();
                return View(kat);
                
             }
            else
            {
                return RedirectToAction("Giris", "Admin");


            }

        }
        public ActionResult Duzenle(int id)
        {

            string value = CookieSorgula("id");
            if (value != null)
            {
                Session["id"] = CookieSorgula("id");
                Session["isim"] = CookieSorgula("isim");
                try
                {
                    Session["resim"] = CookieSorgula("resim");
                }
                catch (Exception)
                {
                    
                }
                Raflar rafs = new Raflar();
                //Kategori kategori = new Kategori();
                Session["UrunID"] = id.ToString();
                rafs.YariMamul = db.Urun.FirstOrDefault(x => x.UrunID == id);

                rafs.RafList = db.Raf.ToList();
                //rafs.MamulList = db.Kategori.ToList();
                var rafss = db.RafUrun.Where(a => a.UrunID == id).ToList();
                // var katss = db.KatUrun.Where(b => b.UrunID == id).ToList();
                List<Raf> model = new List<Raf>();
                foreach (var item in rafss)
                {
                    Raf secilen = new Raf();
                    secilen.RafID = Convert.ToInt32(item.RafID);
                    secilen.RafAdi = item.Raf.RafAdi;
                    model.Add(secilen);
                }

                //List<Kategori> modelKat = new List<Kategori>();
                //foreach (var item in katss)
                //{
                //    Kategori sec = new Kategori();
                //    sec.KategoriID = Convert.ToInt32(item.KategoriID);
                //    sec.UrunAdi = item.Kategori.UrunAdi;
                //    modelKat.Add(sec);
                //}
                //rafs.MamulM = modelKat;
                rafs.RafM = model;

                return View(rafs);
            }
            else
            {
                return RedirectToAction("Giris", "Admin");
            }




        }
        public ActionResult Ekle()
        {

            string Kullanici = CookieSorgula("_id");
            string Yonetici = CookieSorgula("id");
            if (Yonetici != null || Kullanici != null)
            {
                Session["id"] = CookieSorgula("id");
                Session["isim"] = CookieSorgula("isim");
                Session["_id"] = CookieSorgula("_id");
                Session["_isim"] = CookieSorgula("_isim");

                try
                {
                    Session["resim"] = CookieSorgula("resim");
                    Session["_resim"] = CookieSorgula("_resim");
                }
                catch (Exception)
                {
                    
                }

                _fncDropDoldur();
                return View();
            }
            else
            {
                return RedirectToAction("Giris", "Admin");


            }

        }
        [HttpPost]
        public ActionResult Ekle(FormCollection form, int[] KategoriID, int[] RafID)
        {
            DateTime tarih = DateTime.Now;
            Urun urun = new Urun();
            barcode = tarih.Year.ToString() + tarih.Month.ToString() + tarih.Day.ToString() + tarih.Hour.ToString() + tarih.Minute.ToString() + tarih.Second.ToString();
            urun.UrunAdi = form["UrunAdi"].ToString();
            urun.Barcod = barcode;
            urun.Resim = _fncResimYukle();
            urun.UrunKodu = form["UrunKodu"].ToString();
            urun.StokMiktar = Convert.ToInt32(form["StokMiktar"]);
            urun.SatisFiyat = Convert.ToDecimal(form["SatisFiyat"]);
            urun.AlisFiyat = Convert.ToDecimal(form["AlisFiyat"]);
            urun.GirisTarhi= Convert.ToDateTime(DateTime.Now);
            urun.Durum = true;
            db.Urun.Add(urun);
            db.SaveChanges();

            var uruns = db.Urun.OrderByDescending(x=>x.UrunID).FirstOrDefault();
            KatUrun katurun = new KatUrun();
            foreach (var item in KategoriID)
            {
                //string id = item.ToString();
                katurun.KategoriID = Convert.ToInt32(item.ToString());
                katurun.UrunID = uruns.UrunID;
                db.KatUrun.Add(katurun);
                db.SaveChanges();
            }
            RafUrun rafurun = new RafUrun();
            foreach (var item2 in RafID)
            {
                //string id = item.ToString();
                rafurun.RafID = Convert.ToInt32(item2.ToString());
                rafurun.UrunID = uruns.UrunID;
              //  rafurun.KategoriID = 0;
                db.RafUrun.Add(rafurun);
                db.SaveChanges();
            }
            
            ViewBag.Durum = "Başarıyla Kayıt Edildi.";
            _fncDropDoldur();
            return View();
        }
        public ActionResult Detay(int id)
        {

            string Kullanici = CookieSorgula("_id");
            string Yonetici = CookieSorgula("id");
            if (Yonetici != null || Kullanici != null)
            {
                Session["id"] = CookieSorgula("id");
                Session["isim"] = CookieSorgula("isim");
                Session["_id"] = CookieSorgula("_id");
                Session["_isim"] = CookieSorgula("_isim");
                try
                {
                    Session["resim"] = CookieSorgula("resim");
                    Session["_resim"] = CookieSorgula("_resim");
                }
                catch (Exception)
                {
                    
                }

                var detay = db.Urun.Find(id);
                return View(detay);
            }
            else
            {
                return RedirectToAction("Giris", "Admin");


            }
            
        }
        public ActionResult Barcode(int id)
        {

            string Kullanici = CookieSorgula("_id");
            string Yonetici = CookieSorgula("id");
            if (Yonetici != null || Kullanici != null)
            {
                Session["id"] = CookieSorgula("id");
                Session["isim"] = CookieSorgula("isim");
                Session["_id"] = CookieSorgula("_id");
                Session["_isim"] = CookieSorgula("_isim");
                try
                {
                    Session["resim"] = CookieSorgula("resim");
                    Session["_resim"] = CookieSorgula("_resim");
                }
                catch (Exception)
                {
                    
                }

                var urun = db.Urun.FirstOrDefault(x => x.UrunID == id);
                return View(urun);
            }
            else
            {
                return RedirectToAction("Giris", "Admin");


            }

        }
        protected void _fncDropDoldur()
        {

            var query = db.Kategori.Select(c => new { c.KategoriID, c.UrunAdi });
            ViewBag.Kategori = new SelectList(query.AsEnumerable(), "KategoriID", "UrunAdi");

            var raf = db.Raf.Select(a => new { a.RafID, a.RafAdi });
            ViewBag.Raf = new SelectList(raf.AsEnumerable(), "RafID", "RafAdi");
        }
        public ActionResult Borcode(string kod)
        {
            System.IO.MemoryStream MemStream = new System.IO.MemoryStream();
            try
            {

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                int genislik = 300;
                int yukseklik = 100;
                b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;
                type = BarcodeLib.TYPE.CODE128;
                Image bar = b.Encode(type, kod, genislik, yukseklik);
                bar.Save(MemStream, ImageFormat.Jpeg);
                Response.ContentType = "image/Jpeg";
                //  MemStream.WriteTo(Response.OutputStream);

            }
            catch (Exception)
            {

            }
            return File(MemStream.ToArray(), "image/Jpeg");
        }
        public ActionResult Sorgu()
        {

            string Kullanici = CookieSorgula("_id");
            string Yonetici = CookieSorgula("id");
            if (Yonetici != null || Kullanici != null)
            {
                Session["id"] = CookieSorgula("id");
                Session["isim"] = CookieSorgula("isim");
                Session["_id"] = CookieSorgula("_id");
                Session["_isim"] = CookieSorgula("_isim");
                try
                {
                    Session["resim"] = CookieSorgula("resim");
                    Session["_resim"] = CookieSorgula("_resim");
                }
                catch (Exception)
                {
                    
                }

                return View();
            }
            else
            {
                return RedirectToAction("Giris", "Admin");


            }
        }
        [HttpPost]
        public ActionResult Sorgu(FormCollection form)
        {
            string barcod = form["Barcod"].ToString();
            var sorgu = db.Urun.Where(x => x.Barcod == barcod.ToString()).FirstOrDefault();
            return View(sorgu);
        }
        public ActionResult Cikis(int id)
        {
            Session["UrunID"] = id.ToString();
            var urun = db.Urun.FirstOrDefault(x => x.UrunID == id);
            return View(urun);
        }
        [HttpPost]
        public ActionResult Cikis(FormCollection form)
        {
            int id = Convert.ToInt32(Session["UrunID"]);
            Urun kat = db.Urun.Where(x => x.UrunID == id).FirstOrDefault();
            int stok = Convert.ToInt32(kat.StokMiktar);
            int StokMiktar = Convert.ToInt32(form["StokMiktar"]);
            int kalan = stok - StokMiktar;
            kat.StokMiktar = kalan;
            kat.Aciklama = form["Aciklama"].ToString();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //public ActionResult Cikis(int id)
        //{
        //    Urun uruns = db.Urun.Find(id);
        //    int stok = Convert.ToInt32(uruns.StokMiktar);
        //    int kalan = stok - 1;
        //    uruns.StokMiktar = kalan;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");

        //}
        private string _fncResimYukle()
        {
            DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
            uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
            if (uzanti != "")
            {
                TamYolYeri = "/Uploads/Urun/" + barcode + "_" + DosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));

            }
            return TamYolYeri;
        }
        public string CookieSorgula(string cookieName)
        {
            // Cookie adıyla değeri çekme
            string cookie = "";
            try
            {
                cookie = HttpContext.Request.Cookies[cookieName].Value;

            }
            catch (Exception)
            {
                cookie = null;
            }
            return cookie;
        }
    }
}