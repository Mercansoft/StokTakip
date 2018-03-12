using StokTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakip.Controllers
{
    public class AdminController : Controller
    {
        StokTakipEntities db = new StokTakipEntities();
        string TamYolYeri;
        string DosyaAdi;
        string uzanti;
        public ActionResult Index()
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
                return View();
            }
            else
            {
                return RedirectToAction("Giris", "Admin");
            }
        }
        public ActionResult Giris()
        {
            Session.Abandon();
            CookieSil("id");
            CookieSil("isim");
            CookieSil("_id");
            CookieSil("_isim");
            CookieSil("resim");
            CookieSil("_resim");
            return View();
        }
        [HttpPost]
        public ActionResult Giris(FormCollection form)
        {
            string kullanci = form["email"].ToString();
            string sifre = form["sifre"].ToString();
            using (StokTakipEntities db = new StokTakipEntities())
            {

                try
                {
                    var admins = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == kullanci && x.Sifre == sifre && x.Yetki==true);
                    if (admins != null)
                    {
                        Session["id"] = admins.KullaniciID.ToString();
                        Session["isim"] = admins.AdSoyad.ToString();
                        //TempData["AdSoyad"] = admins.AdSoyad.ToString();
                        Session["_id"] = null;
                        Session["_isim"] = null;

                        CookieEkle("id", admins.KullaniciID.ToString());
                        CookieEkle("isim", admins.AdSoyad.ToString());

                        try
                        {
                            Session["resim"] = admins.Resim.ToString();
                            CookieEkle("resim", admins.Resim.ToString());
                        }
                        catch (Exception)
                        {
                            
                        }

                        return RedirectToAction("Index", "Admin");
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı Adınız veya Parolanız Yanlıştır.");
                        ViewBag.Hata = "Kullanıcı Adınız veya Parolanız Yanlıştır.";
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    var admins = db.Kullanici.FirstOrDefault(x => x.KullaniciAdi == kullanci && x.Sifre == sifre && x.Yetki == false);
                    if (admins != null)
                    {
                        Session["_id"] = admins.KullaniciID.ToString();
                        Session["_isim"] = admins.AdSoyad.ToString();

                        Session["id"] = null;
                        Session["isim"] = null;

                        CookieEkle("_id", admins.KullaniciID.ToString());
                        CookieEkle("_isim", admins.AdSoyad.ToString());

                        try
                        {
                            Session["_resim"] = admins.Resim.ToString();
                            CookieEkle("_resim", admins.Resim.ToString());
                        }
                        catch (Exception)
                        {
                            
                        }

                        return RedirectToAction("Panel", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı Adınız veya Parolanız Yanlıştır.");
                        ViewBag.Hata = "Kullanıcı Adınız veya Parolanız Yanlıştır.";
                    }
                }
                catch (Exception)
                {
                    
                }
            }
            return View();

        }
        public ActionResult Ekle()
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
                var admins = db.Kullanici.ToList();
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
            Kullanici admin = new Kullanici();
            admin.AdSoyad = form["AdSoyad"].ToString();
            admin.Email = form["Email"].ToString();
            admin.KullaniciAdi = form["KullaniciAdi"].ToString();
            admin.Sifre = form["Sifre"].ToString();
            if (form["Yetki"]==null)
            {
                admin.Yetki = false;
            }
            else
            {
                admin.Yetki = true;
            }
            //string durum = form["Yetki"].ToString();
            //if (durum == "on")
            //{
            //    admin.Yetki = true;
            //}
            //else
            //{
            //    admin.Yetki = false;
            //}
          //  admin.Yetki = Convert.ToBoolean(form["Yetki"]);
            db.Kullanici.Add(admin);
            db.SaveChanges();
            return RedirectToAction("Ekle");
        }
        public void Sil(int id)
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
                Kullanici admin = new Kullanici();
                admin = db.Kullanici.Find(id);
                db.Kullanici.Remove(admin);
                db.SaveChanges();
                Response.Redirect("/Admin/Ekle");
            }
            else
            {
                Response.Redirect("/Admin/Giris");
            }
        }
        public ActionResult Panel()
        {
            string value = CookieSorgula("_id");
            if (value != null)
            {
                Session["_id"] = CookieSorgula("_id");
                Session["_isim"] = CookieSorgula("_isim");
                try
                {
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
        public ActionResult Profil(int id)
        {
            string value = CookieSorgula("id");
            if (value != null)
            {
                Session["id"] = CookieSorgula("id");
                Session["isim"] = CookieSorgula("isim");
                Session["id"] = id.ToString();
                Session["resimid"] = id.ToString();
                var admins = db.Kullanici.Find(id);
                return View(admins);
            }
            else
            {
                return RedirectToAction("Giris", "Admin");
            }

        }
        public ActionResult Bilgilerim(int id)
        {
            //var admins = db.Kullanici.Find(id);
            //return View(admins);
            string value = CookieSorgula("_id");
            if (value != null)
            {
                Session["_id"] = CookieSorgula("_id");
                Session["_isim"] = CookieSorgula("_isim");
                Session["resimid"] = id.ToString();
                try
                {
                    Session["_resim"] = CookieSorgula("_resim");
                }
                catch (Exception)
                {

                }
               // Session["id"] = id.ToString();
                var admins = db.Kullanici.Find(id);
                return View(admins);
            }
            else
            {
                return RedirectToAction("Giris", "Admin");
            }
        }
        [HttpPost]
        public ActionResult Guncelle(FormCollection form)
        {
            string url = string.Empty;
            string resim = string.Empty;
            string durum = string.Empty;
            int id = Convert.ToInt32(Session["resimid"]);
            Kullanici model = db.Kullanici.Find(id);
            model.AdSoyad = form["AdSoyad"].ToString();
            model.Email = form["Email"].ToString();
            model.KullaniciAdi = form["KullaniciAdi"].ToString();
            model.Sifre = form["Sifre"].ToString();
            try
            {
                durum = form["Yetki"].ToString();
                if (durum == "off")
                {
                    model.Yetki = true;
                }
                else
                {
                    model.Yetki = false;
                }
            }
            catch (Exception)
            {
               // url="/Admin/Profil/" + Session["_id"].ToString();
            }

           // model.Yetki = Convert.ToBoolean(form["Yetki"]);
            try
            {
                resim = _fncResimYukle();
                if (resim != null)
                {
                    model.Resim = resim.ToString();
                }
            }
            catch (Exception)
            {
                
            }
            db.SaveChanges();

            try
            {
                if (model.Yetki==true)
                {
                    url = "/Admin/Profil/" + Session["id"].ToString();
                    Session["resim"] = resim.ToString();
                    Session["isim"] = form["AdSoyad"].ToString();
                    Session["id"] = id.ToString();

                    Request.Cookies["resim"].Value = resim.ToString();
                    Request.Cookies["isim"].Value = form["AdSoyad"].ToString();
                    Request.Cookies["id"].Value = id.ToString();
                }
                else
                {
                    url = "/Admin/Bilgilerim/" + Session["_id"].ToString();
                    Session["_resim"] = resim.ToString();
                    Session["_isim"] = form["AdSoyad"].ToString();
                    Session["_id"]= id.ToString();

                    Request.Cookies["_resim"].Value = resim.ToString();
                    Request.Cookies["_isim"].Value = form["AdSoyad"].ToString();
                    Request.Cookies["_id"].Value = id.ToString();
                }
            }
            catch (Exception)
            {
                
            }
            return Redirect(url);
        }
        [HttpPost]
        public ActionResult Sorgu(FormCollection form)
        {
            string barcod = form["Barcod"].ToString();
            KategoriUrun katurun = new KategoriUrun();
            katurun.Urun = db.Urun.Where(x => x.Barcod == barcod.ToString()).FirstOrDefault();
            katurun.UrunK = db.Kategori.Where(a => a.Barcod == barcod.ToString()).FirstOrDefault();
            //if (katurun.Urun != null)
            //{
                return View("Index", katurun);
            //}
            //else
            //{
            //    return View("Index", katurun);
            //}

        }
        public void Cikis()
        {
            Session.Abandon();
            Session.Remove("id");
            Session.Remove("isim");
            Session.Remove("_id");
            Session.Remove("_isim");
            Session.Remove("resim");
            Session.Remove("_resim");
            CookieSil("id");
            CookieSil("isim");
            CookieSil("_id");
            CookieSil("_isim");
            CookieSil("resim");
            CookieSil("_resim");
            Response.Redirect("/Admin/");
        }
        public void CookieEkle(string cookieName, string cookieValue)
        {
            // Cookie Ekleme
            //cookieName : Cookie adı
            //cookieValue : Cookie değeri
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.Expires = DateTime.Now.AddHours(1); // Süre(1 yıl)
            HttpContext.Response.Cookies.Add(cookie);
        }
        public void CookieSil(string userCookieName)
        {
            //Cookie Silme
            HttpCookie myCookie = new HttpCookie(userCookieName);
            myCookie.Expires = DateTime.Now.AddHours(-1);// Süreyi  1 sene azaltıyoruz
            Response.Cookies.Add(myCookie);
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
        private string _fncResimYukle()
        {
            DosyaAdi = Guid.NewGuid().ToString().Replace("-", "");
            uzanti = System.IO.Path.GetExtension(Request.Files[0].FileName);
            if (uzanti != "")
            {
                TamYolYeri = "/Uploads/" + DosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(TamYolYeri));

            }
            return TamYolYeri;
        }

        [HttpPost]
        public ActionResult StokGuncelle(FormCollection form)
        {
            int id = Convert.ToInt32(form["StokID"]);
            Urun model = db.Urun.Find(id);
            model.StokMiktar = Convert.ToInt32(form["StokMiktar"]);
            db.SaveChanges();
            return Redirect("/Admin/");
        }

    }
}