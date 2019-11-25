using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.DAO;
using CRM.Services;

namespace CRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        UnitOfWork uow = new UnitOfWork();

        public ActionResult Index()
        {
                return View();

        }

       
        public ActionResult About()
        {
            string nome = "[]";

            if (User.Identity.IsAuthenticated)
            {
                nome = User.Identity.Name;
            }

            ViewBag.Message = "Usuário autenticado: "+ nome;

            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}