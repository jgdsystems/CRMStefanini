using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSoccer.DAO;
using WebSoccer.Models;


namespace WebSoccer.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        UnitOfWork uow = new UnitOfWork();

        // GET: User
        public ActionResult Index()
        {
             
            var itens = uow.UserRepositorio.GetAll().ToList();
            return View(itens);
            
        }

        // GET: User/Item
        public ActionResult Item(string id)
        {
            var itens = uow.UserRepositorio.GetAll().Where(x => x.Id_users.Equals(id)).FirstOrDefault();
            return View(itens);
        }


        public ActionResult AddUser(string email, string password)
        {
            return View();
        }

              

    }
}