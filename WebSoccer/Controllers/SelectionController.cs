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
    public class SelectionController : Controller
    {

        UnitOfWork uow = new UnitOfWork();
        UnitOfWork uowValida = new UnitOfWork();

        private Int32 idUser;
        private int countPlayer;


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

        // GET: Selection
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

                    int itensPorPagina = 7;

                    var totalSelection = uowValida.SelectionRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && x.Active == 1).Count();
                    var selectionsDaPagina = uow.SelectionRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && x.Active == 1).OrderBy(x => x.Id_selection).Skip((page - 1) * itensPorPagina).Take(itensPorPagina).ToList();
                    
                        //Preenche com nome da posição conforme seu id
                        foreach (var item in selectionsDaPagina)
                    {
                        Models.Positions positionTemp = uowValida.PositionsRepositorio.GetAll().Where(x => x.Id_Positions.Equals(item.Positions_id_positions)).FirstOrDefault();
                        item.Positions = positionTemp;

                        Models.Team teamTemp = uowValida.TeamRepositorio.GetAll().Where(x => x.Id_team.Equals(item.Team_id_team)).FirstOrDefault();
                        item.Team = teamTemp;
                        
                        Models.Players playerTemp = uowValida.PlayersRepositorio.GetAll().Where(x => x.Id_player.Equals(item.Players_id_player)).FirstOrDefault();
                        item.Players = playerTemp;
                    }

                    return View(new ListaPaginada<Selection>(selectionsDaPagina, totalSelection, itensPorPagina, page, false));
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
        public ActionResult AddSelections()
        {
            if (ValidIdUser())
            {
                    //Preenchendo listas de seleção
                    Selection model = new Selection();
                    model.ListTeam = uow.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ToList();
                    model.ListPlayer = uow.PlayersRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && (x.Active.Equals(1))).ToList();
                    model.ListPosition = uow.PositionsRepositorio.GetAll().Where(x => x.Id_Users.Equals(idUser)).ToList();
                    return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }
        }

        [HttpPost]
        public ActionResult AddSelections(Selection selection)
        {

            if (ValidIdUser())
            {
                
                if (ModelState.IsValid)
                {

                    selection.Users_id_users = idUser;
                    selection.Date = DateTime.Now;
                    selection.Active = 1;
                    uow.SelectionRepositorio.Adicionar(selection);
                    uow.Commit();

                    return RedirectToAction("Index", "Selection");
                }
                else
                {
                    return View(selection);
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
                Selection model = uow.SelectionRepositorio.GetAll().Where(x => x.Id_selection.Equals(id)).FirstOrDefault<Selection>();
                model.ListTeam = uow.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ToList();
                model.ListPosition = uow.PositionsRepositorio.GetAll().Where(x => x.Id_Users.Equals(idUser)).ToList();

                //Retira da lista jogadores já utilizados em outras posições
                List<long?> listId = new List<long?>();
                
                //Verifica a lista de jogadores já utilizados
                foreach (var item in uowValida.SelectionRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)))
                {
                    if ((item.Players_id_player != null) && (model.Players_id_player != item.Players_id_player))
                    {
                        listId.Add(item.Players_id_player);
                    }
                }
                //Clausula IN foi substituida por Contains
                model.ListPlayer = uowValida.PlayersRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && (x.Active == 1) && (listId.Contains(x.Id_player) == false)).ToList();
                
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }

        }

        [HttpPost]
        public ActionResult Edit(Selection selection)
        {

            if (ValidIdUser())
            {
                
                    selection.Users_id_users = idUser;
                    selection.Active = 1;
                    selection.Date = DateTime.Now;
                    uow.SelectionRepositorio.Atualizar(selection);
                    uow.Commit();
                    
                    return RedirectToAction("Index", "Selection");
               
            }
            else
            {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }


        }


        /// <summary>
        /// Sorteia posições
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult DrawPositions()
        {

            if (ValidIdUser())
            {


                //1º limpa todas as posições

                List<Selection> selection = uowValida.SelectionRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ToList<Selection>();
                foreach (var item in selection)
                {
                    item.Players_id_player = null;
                    uow.SelectionRepositorio.Atualizar(item);
                    uow.Commit();
                }

                //2º define jogadores nas posições por sorteio
                //Retira da lista jogadores já utilizados em outras posições
                List<long?> listId = new List<long?>();
                var rnd = new Random();

                foreach (var item in selection)
                {
                    
                    //Clausula IN foi substituida por Contains
                    List<Players> listPlayer = uowValida.PlayersRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && (x.Active == 1) && (listId.Contains(x.Id_player) == false)).ToList();
                    long? valorAleatorio = 0;

                    if (listPlayer.Count == 0)
                    {
                        valorAleatorio = null;
                    }
                    else {
                        valorAleatorio = listPlayer[rnd.Next(listPlayer.Count)].Id_player;
                    }

                    item.Players_id_player = valorAleatorio;
                    uow.SelectionRepositorio.Atualizar(item);
                    uow.Commit();

                    listId.Add(item.Players_id_player);

                }

                return RedirectToAction("Index", "Selection");
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }

        }


        /// <summary>
        /// Limpa todas as posições
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ClearAll()
        {

            if (ValidIdUser())
            {

                List<Selection> selection = uowValida.SelectionRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ToList<Selection>();
                foreach (var item in selection)
                {
                    item.Players_id_player = null;
                    uow.SelectionRepositorio.Atualizar(item);
                    uow.Commit();
                }

                return RedirectToAction("Index", "Selection");
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }

        }

        /// <summary>
        /// Envia email para informar as posições dos jogadores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoPositions()
        {

            if (ValidIdUser())
            {

                //Instância objeto de usuário
                Models.Users user = uowValida.UserRepositorio.GetAll().Where(x => x.Id_users.Equals(idUser)).FirstOrDefault();

                //Instância das seleções
                List<Selection> selection = uowValida.SelectionRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ToList<Selection>();

                
                //Preparação de conteudo do email.
                //1º Preparar assunto e texto do email.
                string subject = "Soccer Play - Sorteio das seleções da partida de futebol 7.";

                //Criando html para o corpo do email
                //-----------------------------------------
                string bodyEmail = string.Empty;
                bodyEmail += bodyEmail = "<html>";
                bodyEmail += bodyEmail = "<head>";
                bodyEmail += bodyEmail = "<meta http-equiv='Content-Type' content='text/html;charset=utf-8'>";
                bodyEmail += bodyEmail = "<STYLE type='text/css'>";
                bodyEmail += bodyEmail = "a:link {text-decoration: none;color: #000099}";
                bodyEmail += bodyEmail = "a:active { text - decoration: none; }";
                bodyEmail += bodyEmail = "a:visited { text - decoration: none; color: #000099}";
                bodyEmail += bodyEmail = "a:visited {text-decoration: none;color: #000099}";
                bodyEmail += bodyEmail = "a:hover { text - decoration: underline; color: #000099}";
                bodyEmail += bodyEmail = "BODY{font-family:arial;}";
                bodyEmail += bodyEmail = "</STYLE>";
                bodyEmail += bodyEmail = "</head>";
                bodyEmail += bodyEmail = "<body>";
                bodyEmail += bodyEmail = "Soccer Play <br><br>";
                bodyEmail += bodyEmail = "Sorteio das seleções da partida:<br><br>";
                bodyEmail += bodyEmail = "Time - Posição - Jogador<br>";

                string destinatarios = string.Empty; 

                //Processar informações das seleções
                foreach (var item in selection)
                {
                    if (item.Players_id_player.HasValue)
                    {
                        item.Players = uowValida.PlayersRepositorio.GetAll().Where(x => x.Id_player.Equals(item.Players_id_player)).FirstOrDefault();
                        item.Positions = uowValida.PositionsRepositorio.GetAll().Where(x => x.Id_Positions.Equals(item.Positions_id_positions)).FirstOrDefault();
                        item.Team = uowValida.TeamRepositorio.GetAll().Where(x => x.Id_team.Equals(item.Team_id_team)).FirstOrDefault();

                        bodyEmail += bodyEmail = item.Team.Name +" - "+ item.Positions.Name + " - "+ item.Players.Name +  "<br>";

                        if (item.Players.Email.Length > 0) {
                            destinatarios += item.Players.Email+";";
                        }
                        
                    }
                }

                bodyEmail += bodyEmail = "<br>A partida está confirmada:<br><br>";

                Match match = uowValida.MatchRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && (x.Status.Equals("Partida Confirmada"))).FirstOrDefault();

                bodyEmail += bodyEmail = "Data e horário: " + match.Confirmation.ToString() + "<br>";
                bodyEmail += bodyEmail = "Endereço: " + match.Address + "<br>";
                bodyEmail += bodyEmail = "<br><a href='http://www.google.com/maps/place/" + match.Latitude + "," + match.Longitude + "' target='_blank'>Clique aqui para ver o endereço no Google Maps</a>";

                bodyEmail += bodyEmail = "</body></html>";

                destinatarios = destinatarios.Remove(destinatarios.Length - 1);

                Services.EmailService.SendEmail(destinatarios, bodyEmail, subject);
                
                return RedirectToAction("Index", "Selection");

            }
            else
            {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }


        }



    }
}