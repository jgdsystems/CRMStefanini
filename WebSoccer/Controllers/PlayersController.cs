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
    public class PlayersController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        UnitOfWork uowValida = new UnitOfWork();
        
        private int countName;
        private int countEmail;
        private int countPhonenumber;
        private Int32 idUser; 
        
        public string currentEmail;
        public string currentName;
        public string currentPhoneNumber;

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
                else {
                    return false;
                }
            }
            else {
                return false;
            }


        }

        // GET: Players
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

                    int itensPorPagina = 10;

                    //Lista jogadores com controle de paginação

                    var totalPlayers = uowValida.PlayersRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && x.Active == 1).Count();
                    var playersDaPagina = uow.PlayersRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && x.Active == 1).OrderBy(x => x.Name).Skip((page - 1) * itensPorPagina).Take(itensPorPagina).ToList();

                    //Preenche com nome da posição conforme seu id
                    foreach (var item in playersDaPagina)
                    {
                        Positions positionTemp = uowValida.PositionsRepositorio.GetAll().Where(x => x.Id_Positions.Equals(item.Id_Positions)).FirstOrDefault();
                        item.Positions = positionTemp;
                    }
                    
                    return View(new ListaPaginada<Players>(playersDaPagina, totalPlayers, itensPorPagina, page, false));
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

       


        // Add a new player
        [HttpGet]
        public ActionResult AddPlayer()
        {
            
            if (ValidIdUser())
            {
                int countPlayers = uowValida.PlayersRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && (x.Active == 1)).Count();

                if (countPlayers < 20)
                {
                    Players model = new Players();
                    model.ListPositions = uow.PositionsRepositorio.GetAll().Where(x => x.Id_Users.Equals(idUser)).ToList();
                    return View(model);
                }
                else {
                    return RedirectToAction("Index", "Players");
                }
            }
            else {
                return RedirectToAction("Login", "Default");
            }
            
        }
        [HttpPost]
        public ActionResult AddPlayer(Players player)
        {

            if (ValidIdUser())
            {

                //Lista auxiliar
                player.ListPositions = uow.PositionsRepositorio.GetAll().Where(x => x.Id_Users.Equals(idUser)).ToList();

                //Validação
                ValidaPlayerAdd(player);

                if (ModelState.IsValid)
                {
                    player.Users_id_users = idUser;
                    player.Status = "Pendente";
                    player.Active = 1;
                    uow.PlayersRepositorio.Adicionar(player);
                    uow.Commit();
                    return RedirectToAction("Index", "Players");
                }
                else
                {
                    return View(player);
                }

            }
            else
            {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }

                
        }


        // Edit player
        [HttpGet]
        public ActionResult Edit(long id)
        {

            if (ValidIdUser())
            {
                Players model = uow.PlayersRepositorio.GetAll().Where(x => x.Id_player.Equals(id)).FirstOrDefault<Players>();
                model.ListPositions = uow.PositionsRepositorio.GetAll().Where(x => x.Id_Users.Equals(idUser)).ToList();

                //ValidFields(model);
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }

        }
        
        /// <summary>
        /// Save changes in player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Players player)
        {

            if (ValidIdUser())
            {
                //Lista auxiliar
                player.ListPositions = uow.PositionsRepositorio.GetAll().Where(x => x.Id_Users.Equals(idUser)).ToList();

                //Validação para tipo
                ValidaPlayerEdit(player);

                if (ModelState.IsValid)
                {
                    player.Users_id_users = idUser;
                    player.Active = 1;
                    uow.PlayersRepositorio.Atualizar(player);
                    uow.Commit();

                    //Clear valid Fields
                    //ValidFields();

                    return RedirectToAction("Index", "Players");
                }
                else
                {
                    return View(player);
                }
            }
            else
            {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }
            
            
        }

        [HttpGet]
        public ActionResult Delete() {
            
            return View();
            
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {

            
                Players player = uow.PlayersRepositorio.GetAll().Where(x => x.Id_player.Equals(id)).FirstOrDefault<Players>();
                player.Active = 0;
                uow.PlayersRepositorio.Atualizar(player);
                uow.Commit();

                return RedirectToAction("Index", "Players");
            

        }


        #region Validações

        private void ValidaPlayerEdit(Players player)
        {


            if (ValidIdUser())
            {


                //campos obrigatórios
                if (player.Name == null || player.Name.Trim().Length == 0)
                {
                    ModelState.AddModelError("Name", "O campo nome é obrigatório");
                }
                else
                {
                    //Verificar registro que está salvo no banco de dados
                    IEnumerable<Players> playerTempList = uowValida.PlayersRepositorio.GetAll().Where(x => x.Id_player.Equals(player.Id_player) && (x.Users_id_users.Equals(idUser)) && x.Active == 1);
                    foreach (var item in playerTempList)
                    {
                        currentName = item.Name;
                    }
                    
                    if (player.Name != currentName)
                    {
                        countName = uowValida.PlayersRepositorio.GetAll().Where(x => x.Name.Equals(player.Name.Trim()) && (x.Users_id_users.Equals(idUser)) && (x.Id_player != player.Id_player) && x.Active == 1).Count();
                        if (countName > 0)
                        {
                            ModelState.AddModelError("Name", "Já existe um jogador utilizando o nome: " + player.Name);
                        }
                    }

                }

                if (player.Email == null || player.Email.Trim().Length == 0)
                {
                    ModelState.AddModelError("Email", "O campo email é obrigatório");
                }
                else
                {

                    //Verificar registro que está salvo no banco de dados
                    IEnumerable<Players> playerTempList = uowValida.PlayersRepositorio.GetAll().Where(x => x.Id_player.Equals(player.Id_player) && (x.Users_id_users.Equals(idUser)) && x.Active == 1);
                    foreach (var item in playerTempList)
                    {
                        currentEmail = item.Email;
                    }

                    if (player.Email != currentEmail)
                    {
                        countEmail = uowValida.PlayersRepositorio.GetAll().Where(x => x.Email.Equals(player.Email.Trim()) && (x.Users_id_users.Equals(idUser)) && x.Active == 1).Count();
                        if (countEmail > 0)
                        {
                            ModelState.AddModelError("Email", "Já existe um jogador utilizando o email: " + player.Email);
                        }
                    }
                }

                //Se preenchido validar se o celular já foi cadastrado
                if (player.Phonenumber != null && player.Phonenumber.Trim().Length > 0)
                {

                    //Verificar registro que está salvo no banco de dados
                    IEnumerable<Players> playerTempList = uowValida.PlayersRepositorio.GetAll().Where(x => x.Id_player.Equals(player.Id_player) && (x.Users_id_users.Equals(idUser)) && x.Active == 1);
                    foreach (var item in playerTempList)
                    {
                        currentPhoneNumber = item.Phonenumber;
                    }

                    if (player.Phonenumber != currentPhoneNumber)
                    {
                        countPhonenumber = uowValida.PlayersRepositorio.GetAll().Where(x => x.Phonenumber != null && x.Users_id_users.Equals(idUser) && x.Phonenumber == player.Phonenumber.Trim() && x.Active == 1).Count();
                        if (countPhonenumber > 0)
                        {
                            ModelState.AddModelError("Phonenumber", "Já existe um jogador utilizando o celular: " + player.Phonenumber);
                        }
                    }
                }



                if (player.Type == null || player.Type.Trim().Length == 0)
                {
                    ModelState.AddModelError("Type", "Defina o tipo do jogador.");
                }

                if (player.Id_Positions == 0)
                {
                    ModelState.AddModelError("Id_Positions", "Defina a posição.");
                }


            }
        }

        private void ValidaPlayerAdd(Players player)
        {

            if (ValidIdUser())
            {

                //Validações
                //---------------------------------------------------------
                //campos obrigatórios
                if (player.Name == null || player.Name.Trim().Length == 0)
                {
                    ModelState.AddModelError("Name", "O campo nome é obrigatório");
                }
                else
                {
                    countName = uow.PlayersRepositorio.GetAll().Where(x => x.Name.Equals(player.Name.Trim()) && (x.Users_id_users.Equals(idUser)) && x.Active == 1).Count();
                    if (countName > 0)
                    {
                        ModelState.AddModelError("Name", "Já existe um jogador utilizando o nome: " + player.Name);
                    }
                }

                if (player.Email == null || player.Email.Trim().Length == 0)
                {
                    ModelState.AddModelError("Email", "O campo email é obrigatório");
                }
                else
                {
                    countEmail = uow.PlayersRepositorio.GetAll().Where(x => x.Email.Equals(player.Email.Trim()) && (x.Users_id_users.Equals(idUser)) && x.Active == 1).Count();
                    if (countEmail > 0)
                    {
                        ModelState.AddModelError("Email", "Já existe um jogador utilizando o email: " + player.Email);
                    }
                }

                //Se preenchido validar se o celular já foi cadastrado
                if (player.Phonenumber != null && player.Phonenumber.Trim().Length > 0)
                {
                    countPhonenumber = uow.PlayersRepositorio.GetAll().Where(x => x.Phonenumber != null && x.Users_id_users.Equals(idUser) && x.Phonenumber == player.Phonenumber.Trim() && x.Active == 1).Count();
                    if (countPhonenumber > 0)
                    {
                        ModelState.AddModelError("Phonenumber", "Já existe um jogador utilizando o celular: " + player.Phonenumber);
                    }
                }

                if (player.Type == null || player.Type.Trim().Length == 0)
                {
                    ModelState.AddModelError("Type", "Defina o tipo do jogador.");
                }

                if (player.Id_Positions == 0)
                {
                    ModelState.AddModelError("Id_Positions", "Defina a posição.");
                }

            }
            //---------------------------------------------------------
        }

        #endregion

        

       }
}