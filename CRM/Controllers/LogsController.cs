using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CRM.DAO;
using CRM.Models;

namespace CRM.Controllers
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