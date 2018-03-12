using StokTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakip.Controllers
{
    public class RafController : Controller
    {
        StokTakipEntities db = new StokTakipEntities();
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

                return View();
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

                var admins = db.Raf.ToList();
                return View(admins);
            }
            else
            {
                return RedirectToAction("Giris", "Admin");


            }
            

        }
        [HttpPost]
        public ActionResult Ekle(FormCollection form)
        {
            Raf model = new Raf();
            model.Aciklama = form["Aciklama"].ToString();
            model.RafAdi= form["RafAdi"].ToString();
            db.Raf.Add(model);
            db.SaveChanges();
            return RedirectToAction("Ekle");
        }
        public ActionResult Duzenle(int id)
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

                Session["RafID"] = id.ToString();
                var admins = db.Raf.Find(id);
                return View(admins);
            }
            else
            {
                return RedirectToAction("Giris", "Admin");


            }
            Session["RafID"] = null;
        }
        [HttpPost]
        public ActionResult Duzenle(FormCollection form)
        {
            Raf model = db.Raf.Find(Convert.ToInt32(Session["RafID"]));
            
            model.Aciklama = form["Aciklama"].ToString();
            model.RafAdi = form["RafAdi"].ToString();
            db.SaveChanges();
            return RedirectToAction("Ekle");
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