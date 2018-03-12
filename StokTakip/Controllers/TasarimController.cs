using StokTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakip.Controllers
{
    public class TasarimController : Controller
    {
        StokTakipEntities db = new StokTakipEntities();
        public ActionResult _Layout(HttpContextBase httpContext)
        {
            return View();
        }
        public ActionResult _LayoutTablo()
        {
            return View();
        }
        public ActionResult _LayoutFirmaTablo()
        {
            return View();
        }
        public ActionResult _LayoutRaporTablo()
        {
            return View();
        }
        public ActionResult _LayoutForm()
        {
            return View();
        }
        public ActionResult _LayoutKullaniciForm()
        {
            return View();
        }
        public ActionResult _LayoutKullanici()
        {
            return View();
        }
        public PartialViewResult _Footer()
        {
            return PartialView();
        }
        public ActionResult _Sohbet()
        {
            return View();
        }
        public ActionResult _SonUrunler()
        {
            var ogrenci = db.Kategori.OrderByDescending(o => o.KategoriID).ToList().Take(10);
            return View(ogrenci);
        }
        public ActionResult _SonUrunlerFirma()
        {
            return View();
        }
        public PartialViewResult _Menu()
        {
            return PartialView();
        }
        public PartialViewResult _MenuUser()
        {
            return PartialView();
        }
        public PartialViewResult _MenuFirma()
        {
            return PartialView();
        }
        public ActionResult _SonGiris()
        {
            return View();
        }
        public ActionResult _SonIslem()
        {
            return View();
        }
        public ActionResult _Istatistik()
        {
            try
            {
                ViewBag.YariMamul = db.Urun.ToList().Count;
                ViewBag.Mamul = db.Kategori.ToList().Count;
               // ViewBag.DepoCikis = db.MY_Urun.Where(x => x.Durum == false).ToList().Count;
            }
            catch (Exception)
            {

            }
            return View();
        }
        public ActionResult _IstatistikFirma()
        {
            return View();
        }
    }
}