using StokTakip.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakip.Controllers
{
    public class KategoriController : Controller
    {
        StokTakipEntities db = new StokTakipEntities();
        string TamYolYeri;
        string DosyaAdi;
        string uzanti;
        string barcode = string.Empty;

        public ActionResult Index()
        {
            var kat = db.Kategori.OrderByDescending(x => x.KategoriID).ToList();
            return View(kat);
        }
        public ActionResult Ekle()
        {
            _fncDropDoldur();
            return View();
        }
        [HttpPost]
        public ActionResult Ekle(FormCollection form, int[] RafID)
        {
            DateTime tarih = DateTime.Now;
            Kategori urun = new Kategori();
            barcode = tarih.Year.ToString() + tarih.Month.ToString() + tarih.Day.ToString() + tarih.Hour.ToString() + tarih.Minute.ToString() + tarih.Second.ToString();
            urun.UrunAdi = form["UrunAdi"].ToString();
            urun.Barcod = barcode;
            urun.Resim = _fncResimYukle();
            urun.UrunKodu = form["UrunKodu"].ToString();
            urun.StokMiktar = Convert.ToInt32(form["StokMiktar"]);
            urun.SatisFiyat = Convert.ToDecimal(form["SatisFiyat"]);
            urun.AlisFiyat = Convert.ToDecimal(form["AlisFiyat"]);
            urun.GirisTarhi = Convert.ToDateTime(DateTime.Now);
            urun.Durum = true;
            db.Kategori.Add(urun);
            db.SaveChanges();
            var kats = db.Kategori.OrderByDescending(x => x.KategoriID).FirstOrDefault();

            RafUrun rafurun = new RafUrun();
            foreach (var item2 in RafID)
            {
                //string id = item.ToString();
                rafurun.RafID = Convert.ToInt32(item2.ToString());
                rafurun.KategoriID = kats.KategoriID;
                db.RafUrun.Add(rafurun);
                db.SaveChanges();
            }
            ViewBag.Durum = "Başarıyla Kayıt Edildi.";
            _fncDropDoldur();
            return View();
        }
        protected void _fncDropDoldur()
        {

            var raf = db.Raf.Select(a => new { a.RafID, a.RafAdi });
            ViewBag.Raf = new SelectList(raf.AsEnumerable(), "RafID", "RafAdi");
        }
        public ActionResult Detay(int id)
        {

            var detay = db.Kategori.Where(x => x.KategoriID == id).FirstOrDefault(); ;
            return View(detay);
        }
        public ActionResult Barcode(int id)
        {
            var urun = db.Kategori.FirstOrDefault(x => x.KategoriID == id);
            return View(urun);

        }
        public ActionResult Cikis(int id)
        {
            Session["KatID"] = id.ToString();
            var urun = db.Kategori.FirstOrDefault(x => x.KategoriID == id);
            return View(urun);
        }
        public PartialViewResult _KategoriList()
        {
            var raf = db.Raf.Select(a => new { a.RafID, a.RafAdi });
            ViewBag.Raf = new SelectList(raf.AsEnumerable(), "RafID", "RafAdi");
            return PartialView();
        }
        public ActionResult Duzenle(int id)
        {
                Raflar rafs = new Raflar();
                // _fncDropDoldur();
                // RafKategori rafurun = new RafKategori();
                Session["KatID"] = id.ToString();
                rafs.Mamul = db.Kategori.FirstOrDefault(x => x.KategoriID == id);

                rafs.RafList = db.Raf.ToList();
                var rafss = db.RafUrun.Where(a => a.KategoriID == id).ToList();

                List<Raf> model = new List<Raf>();
                foreach (var item in rafss)
                {
                    Raf secilen = new Raf();
                    secilen.RafID = Convert.ToInt32(item.RafID);
                    secilen.RafAdi = item.Raf.RafAdi;
                    model.Add(secilen);
                }

                rafs.RafM = model;
                // rafs.RafM = db.RafUrun.Where(a => a.KategoriID == id).ToList();
                //  ViewBag.Raf = new MultiSelectList(raf, "RafID", "RafAdi");
                //IList<SelectListItem> items = new List<SelectListItem>();
                //foreach (var item in raf)
                //{
                //    items.Add(new SelectListItem { Text = item.Raf.RafAdi, Value = item.RafID.ToString() });
                //}
                //rafurun.RafM = items;

                return View(rafs);


            
        }
        [HttpPost]
        public ActionResult Duzenle(FormCollection form, int[] RafID)
        {
            int id = Convert.ToInt32(Session["KatID"]);
            Kategori urun = db.Kategori.Find(id);
            urun.UrunAdi = form["UrunAdi"].ToString();
            urun.Barcod = form["Barcod"].ToString(); ;
            urun.Resim = _fncResimYukle();
            urun.UrunKodu = form["UrunKodu"].ToString();
            urun.StokMiktar = Convert.ToInt32(form["StokMiktar"]);
            urun.SatisFiyat = Convert.ToDecimal(form["SatisFiyat"]);
            urun.AlisFiyat = Convert.ToDecimal(form["AlisFiyat"]);
            urun.Durum = true;
            db.Entry(urun).State = EntityState.Modified;
            db.SaveChanges();
           // var rafurun = db.RafUrun.Where(z => z.KategoriID == id).FirstOrDefault();
            List<RafUrun> list = db.RafUrun.Where(z => z.KategoriID == id).ToList();
            foreach (var item in list)
            {
                RafUrun model = new RafUrun();
                model = db.RafUrun.Find(item.RafUrunID);
                db.RafUrun.Remove(model);
                db.SaveChanges();
            }
            foreach (var item2 in RafID)
            {
                int rafid= Convert.ToInt32(item2.ToString());
                RafUrun model2 = new RafUrun();
               // model2 = db.RafUrun.Find(item2.ToString());
                model2.RafID = rafid;
                model2.KategoriID = id;
                db.RafUrun.Add(model2);
                db.SaveChanges();
                //string id = item.ToString();
                //rafurun.RafID = rafid;
                //rafurun.KategoriID = id;
                //db.Entry(rafurun).State = EntityState.Modified;
                //db.SaveChanges();
            }
            ViewBag.Durum = "Başarıyla Güncellendi.";
            _fncDropDoldur();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Cikis(FormCollection form)
        {
            int id = Convert.ToInt32(Session["KatID"]);
            Kategori kat = db.Kategori.Where(x => x.KategoriID == id).FirstOrDefault();
            int stok = Convert.ToInt32(kat.StokMiktar);
            int StokMiktar = Convert.ToInt32(form["StokMiktar"]);
            int kalan = stok - StokMiktar;
            kat.StokMiktar = kalan;
            kat.Aciklama = form["Aciklama"].ToString();
            db.SaveChanges();
            _fncYariMamulStokGuncelle(id, StokMiktar);
            return RedirectToAction("Index");
        }
        //public ActionResult Cikis(int id)
        //{
        //    using (StokTakipEntities db2 = new StokTakipEntities())
        //    {
        //        Kategori kat = db2.Kategori.Where(x => x.KategoriID == id).FirstOrDefault();
        //        int stok = Convert.ToInt32(kat.StokMiktar);
        //        int kalan = stok - 1;
        //        kat.StokMiktar = kalan;
        //        db2.SaveChanges();
        //    }

        //    _fncYariMamulStokGuncelle(id);
        //    return RedirectToAction("Index");
        //}
        private void _fncYariMamulStokGuncelle(int id,int stokMiktar)
        {
            using (StokTakipEntities db3 = new StokTakipEntities())
            {
                KatUrun UrunKat = db3.KatUrun.Where(o => o.KategoriID == id).FirstOrDefault();
               var Mamul = db3.KatUrun.Where(y => y.UrunID == UrunKat.UrunID).ToList();
              //  Urun uruns = new Urun();
                foreach (var item in Mamul)
                {
                    int stokk = Convert.ToInt32(item.Urun.StokMiktar);
                    int kalann = stokk - stokMiktar;
                    db3.Database.ExecuteSqlCommand("UPDATE Urun SET StokMiktar="+ kalann+" WHERE UrunID="+ item.UrunID);
                }
            }
        }
        public PartialViewResult _YariMamul(int id)
        {
            var kat = db.KatUrun.Where(x => x.KategoriID == id).ToList();
            return PartialView(kat);
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
            return View();
        }
        [HttpPost]
        public ActionResult Sorgu(FormCollection form)
        {
            string barcod = form["Barcod"].ToString();
            var sorgu = db.Kategori.Where(x => x.Barcod == barcod.ToString()).FirstOrDefault();
            return View(sorgu);
        }
        private string _fncResimYukle()
        {
            DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
            uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
            if (uzanti != "")
            {
                TamYolYeri = "/Uploads/Kategori/" + barcode + "_" + DosyaAdi + uzanti;
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