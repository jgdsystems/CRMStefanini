using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.DAO;
using CRM.Models;

namespace CRM.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        UnitOfWork uowValida = new UnitOfWork();
        private Int32 idUser;
        private string currentName;
        private int countName;
        
        // GET: Customer
        public ActionResult Index()
        {
            try
            {
                if (ValidIdUser())
                {
                    ViewBag.IsAdmin = false;
                    BuildLists();

                    var customersList = uowValida.CustomerRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ToList();
                    if (UserIsAdmin())
                    {
                        ViewBag.IsAdmin = true;
                        customersList = uowValida.CustomerRepositorio.GetAll().ToList();
                    }
                    return View(customersList);
                }
                else
                {
                    return RedirectToAction("Login", "Default");
                }
            }
            catch (Exception ex)
            {
                Common.SendLogError(idUser, ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult FillRegion(int City)
        {
            var regions = uowValida.RegionRepositorio.GetAll().Where(r => r.Citys_IdCity == City);
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        private void BuildLists()
        {
            var citysList = uowValida.CityRepositorio.GetAll();
            var regionsList = uowValida.RegionRepositorio.GetAll();

            ViewBag.citys = citysList;
            ViewBag.regions = regionsList;
        }

        [HttpGet]
        public ActionResult Create() {

            if (ValidIdUser())
            {
                BuildLists();

                return View();
            }
            else {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }
        }
        
        [HttpPost]
        public ActionResult Create(Customers Customer)
        {
            if (ValidIdUser())
            {
                    BuildLists();
                    ValidCustomer(Customer);

                    if (ModelState.IsValid)
                    {
                        Customer.Users_id_users = idUser;
                        uow.CustomerRepositorio.Adicionar(Customer);
                        uow.Commit();
                        return RedirectToAction("Index", "Customer");
                    }
                    else
                    {
                        return View(Customer);
                    }
            }
            else
            {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            if (ValidIdUser())
            {
                BuildLists();
                Customers model = uow.CustomerRepositorio.GetAll().Where(x => x.Id_customer.Equals(id)).FirstOrDefault<Customers>();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }
        }

        [HttpPost]
        public ActionResult Edit(Customers Customer)
        {
            
            if (ValidIdUser())
            {
                BuildLists();
                ValidCustomer(Customer);

                if (ModelState.IsValid)
                {
                    Customer.Users_id_users = idUser;
                    uow.CustomerRepositorio.Atualizar(Customer);
                    uow.Commit();

                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    return View(Customer);
                }
            }
            else
            {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }
        }

        /// <summary>
        /// Controle de variavel de sessão idUser
        /// </summary>
        /// <returns>bool</returns>
        private bool ValidIdUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var users = uowValida.UserRepositorio.GetAll().Where(x => x.Email.Equals(User.Identity.Name)).ToList();
                if (users.Count().Equals(1))
                {
                    var user = users.FirstOrDefault();
                    idUser = user.Id_user;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private bool UserIsAdmin()
        {
            var user = uowValida.UserRepositorio.GetAll().Where(x => x.Email.Equals(User.Identity.Name)).FirstOrDefault();
            if (user.Type == "2" && user.Profile == "2")
                return true;
            else
                return false;
        }
        
        private void ValidCustomer(Customers Customer) {
            #region Validações
            if (Customer.Name == null || Customer.Name.Trim().Length == 0)
            {
                ModelState.AddModelError("Name", "Informe o nome do time.");
            }
            else
            {
                //Verificar registro que está salvo no banco de dados
                IEnumerable<Customers> CustomerTempList = uowValida.CustomerRepositorio.GetAll().Where(x => x.Id_customer.Equals(Customer.Id_customer) && (x.Users_id_users.Equals(idUser)));
                foreach (var item in CustomerTempList)
                {
                    currentName = item.Name;
                }

                if (Customer.Name != currentName)
                {
                    countName = uowValida.CustomerRepositorio.GetAll().Where(x => x.Name.Equals(Customer.Name.Trim()) && (x.Users_id_users.Equals(idUser)) && (x.Id_customer != Customer.Id_customer)).Count();
                    if (countName > 0)
                    {
                        ModelState.AddModelError("Name", "Já existe um time utilizando o nome: " + Customer.Name);
                    }
                }

            }
            #endregion
        }


    }
}