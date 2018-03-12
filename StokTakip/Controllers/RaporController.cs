using StokTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakip.Controllers
{
    public class RaporController : Controller
    {
        StokTakipEntities db = new StokTakipEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TarihArasi(FormCollection form)
        {
            int stok = 0;
            decimal alis = 0;
            decimal satis = 0;
            decimal kar = 0;
            DateTime BaslangicTarihi = Convert.ToDateTime(form["BaslangicTarihi"]);
            DateTime BitisTarihi = Convert.ToDateTime(form["BitisTarihi"]);

            var result = db.Urun.Where(p => p.GirisTarhi >= BaslangicTarihi && p.GirisTarhi < BitisTarihi && p.Durum == true).ToList();

            
            foreach (var item in result)
            {
                stok += Convert.ToInt32(item.StokMiktar);
                alis += Convert.ToDecimal(item.AlisFiyat);
                satis += Convert.ToDecimal(item.SatisFiyat);
            }

            kar = satis - alis;
            ViewBag.Stok = stok;
            ViewBag.Alis = alis;
            ViewBag.Satis = satis;
            ViewBag.Kar = kar;

            return View(result);
        }
        public ActionResult TarihArasi()
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