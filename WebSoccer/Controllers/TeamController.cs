using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSoccer.DAO;
using WebSoccer.Models;
using WebSoccer.ViewModel;

namespace WebSoccer.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        UnitOfWork uowValida = new UnitOfWork();
        private Int32 idUser;
        private string currentName;
        private int countName;


        // GET: Team
        public ActionResult Index()
        {
            try
            {
                if (ValidIdUser())
                {

                    int page = 1;

                    if (Request["page"] != null)
                    {
                        page = Int32.Parse(Request["page"]);
                    }

                    int itensPorPagina = 12;

                    //Lista jogadores com controle de paginação

                    var totalTeams = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).Count();
                    var teamsDaPagina = uow.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).OrderBy(x => x.Name).Skip((page - 1) * itensPorPagina).Take(itensPorPagina).ToList();

                    return View(new ListaPaginada<Team>(teamsDaPagina, totalTeams, itensPorPagina, page, false));
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
        public ActionResult CreateTeam() {

            if (ValidIdUser())
            {

                int countTeam = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).Count();
                if (countTeam < 2)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Team");
                }

            }
            else {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }


        }

        [HttpPost]
        public ActionResult CreateTeam(Team team)
        {
            if (ValidIdUser())
            {

               
                    ValidTeam(team);

                    if (ModelState.IsValid)
                    {
                        team.Users_id_users = idUser;
                        uow.TeamRepositorio.Adicionar(team);
                        uow.Commit();
                        return RedirectToAction("Index", "Team");
                    }
                    else
                    {
                        return View(team);
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
                Team model = uow.TeamRepositorio.GetAll().Where(x => x.Id_team.Equals(id)).FirstOrDefault<Team>();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }

        }


        [HttpPost]
        public ActionResult Edit(Team team)
        {
            if (ValidIdUser())
            {
                ValidTeam(team);

                if (ModelState.IsValid)
                {
                    team.Users_id_users = idUser;
                    uow.TeamRepositorio.Atualizar(team);
                    uow.Commit();

                    return RedirectToAction("Index", "Team");
                }
                else
                {
                    return View(team);
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
                var itens = uowValida.UserRepositorio.GetAll().Where(x => x.Email.Equals(User.Identity.Name)).ToList();
                if (itens.Count().Equals(1))
                {
                    foreach (var item in itens)
                    {
                        idUser = item.Id_users;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


        }

        private void ValidTeam(Team team) {

            #region Validações
            if (team.Name == null || team.Name.Trim().Length == 0)
            {
                ModelState.AddModelError("Name", "Informe o nome do time.");
            }
            else
            {
                //Verificar registro que está salvo no banco de dados
                IEnumerable<Team> teamTempList = uowValida.TeamRepositorio.GetAll().Where(x => x.Id_team.Equals(team.Id_team) && (x.Users_id_users.Equals(idUser)));
                foreach (var item in teamTempList)
                {
                    currentName = item.Name;
                }

                if (team.Name != currentName)
                {
                    countName = uowValida.TeamRepositorio.GetAll().Where(x => x.Name.Equals(team.Name.Trim()) && (x.Users_id_users.Equals(idUser)) && (x.Id_team != team.Id_team)).Count();
                    if (countName > 0)
                    {
                        ModelState.AddModelError("Name", "Já existe um time utilizando o nome: " + team.Name);
                    }
                }

            }
            #endregion

        }


    }
}