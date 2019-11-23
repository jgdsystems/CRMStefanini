using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebSoccer.DAO;
using WebSoccer.Models;

namespace WebSoccer.Controllers
{
    [Authorize]
    public class LogsController : Controller
    {

        UnitOfWork uow = new UnitOfWork();

        // GET: Logs
        public ActionResult Index()
        {
            
                return View();
            
                        
        }
        
    }
}