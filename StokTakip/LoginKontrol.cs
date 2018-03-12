using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System;

namespace StokTakip
{
    public class LoginKontrol : AuthorizeAttribute
    {

        // Admin giriş bölümünde yazılacak kodlar
        //HttpCookie myCookie = new HttpCookie("Admin");
        //myCookie.Expires = DateTime.Now.AddDays(1);
        // myCookie["AdminID"] = admins.KullaniciID.ToString();
        // myCookie["AdSoyad"] = admins.AdSoyad.ToString();
        // Response.Cookies.Add(myCookie);

        public HttpSessionStateBase Session { get; }
        string Cookie;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if (httpContext.Request.Cookies["Admin"] != null)
            {
                return true;
            }
            else
            {
                httpContext.Response.Redirect("/Admin/Giris");
                return false;
            }
            if (httpContext.Request.Cookies["Kullanici"] != null)
            {

                return true;
            }
            else
            {
                httpContext.Response.Redirect("/Admin/Giris");
                return false;
            }


        }
    }
}