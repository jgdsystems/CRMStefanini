using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSoccer.DAO;
using WebSoccer.Models;
using WebSoccer.Services;
using WebSoccer.ViewModel;

namespace WebSoccer.Controllers
{

    public class MatchController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        UnitOfWork uowValida = new UnitOfWork();

        private Int32 idUser;

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

        [Authorize]
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

                    //Lista partidas com controle de paginação

                    var totalMatch = uowValida.MatchRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).Count();
                    var matchsDaPagina = uow.MatchRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).OrderBy(x => x.Status).Skip((page - 1) * itensPorPagina).Take(itensPorPagina).ToList();

                    //Preenche com nome da posição conforme seu id
                    //foreach (var item in matchsDaPagina)
                    //{
                    //    Positions positionTemp = uowValida.PositionsRepositorio.GetAll().Where(x => x.Id_Positions.Equals(item.Id_Positions)).FirstOrDefault();
                    //    item.Positions = positionTemp;
                    //}

                    //Somente criar uma nova partida após conclusão da partida anterior.    
                    bool newMatch = true;
                    int countMatch = uowValida.MatchRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && ((x.Status.Equals("Em elaboração")) || (x.Status.Equals("Partida Confirmada")))).ToList().Count();
                    if (countMatch > 0)
                    {
                        newMatch = false;
                    }

                    return View(new ListaPaginada<Match>(matchsDaPagina, totalMatch, itensPorPagina, page, newMatch));
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
        [Authorize]
        [HttpGet]
        public ActionResult AddMatch()
        {

            if (ValidIdUser())
            {
                int countMatchs = uowValida.MatchRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).Count();
                Match model = new Match();
                model.Confirmation = DateTime.Now;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }

        }

        //Add Match
        [Authorize]
        [HttpPost]
        public ActionResult AddMatch(Match match)
        {

            if (ValidIdUser())
            {

                //Lista auxiliar
                //match.ListPositions = uow.PositionsRepositorio.GetAll().Where(x => x.Id_Users.Equals(idUser)).ToList();

                //Validação
                //ValidaPlayerAdd(player);

                if (match.Address == null || match.Address.Trim().Length == 0)
                {
                    ModelState.AddModelError("Address", "O campo endereço é obrigatório");
                }

                if (match.Confirmation == null)
                {
                    ModelState.AddModelError("Confirmation", "O campo data e horário é obrigatório");
                }


                if (ModelState.IsValid)
                {
                    //Preenchendo valores iniciais dos campos
                    //-----------------------------------------------  
                    //match.Id_match Auto incremento

                    Team teamA = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ElementAt(0);
                    Team teamB = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ElementAt(1);

                    match.Team_id_team_A = teamA.Id_team;
                    match.Team_id_team_B = teamB.Id_team;
                    match.Score_a = 0;
                    match.Score_b = 0;
                    match.Winner = 0;
                    //match.Address Preenchido pela view
                    match.Date = DateTime.Now;
                    //match.Confirmation Preenchido pela view
                    match.Status = "Em elaboração";
                    match.Users_id_users = idUser;

                    //-----------------------------------------------  

                    uow.MatchRepositorio.Adicionar(match);
                    uow.Commit();

                    return RedirectToAction("Index", "Match");
                }
                else
                {
                    return View(match);
                }

            }
            else
            {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }


        }

        //Edit Match
        // Edit player
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {

            if (ValidIdUser())
            {
                Match match = uowValida.MatchRepositorio.GetAll().Where(x => x.Id_match.Equals(id) && x.Users_id_users.Equals(idUser)).FirstOrDefault();

                Team teamA = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ElementAt(0);
                Team teamB = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ElementAt(1);

                match.Team_id_team_A = teamA.Id_team;
                match.Team_id_team_B = teamB.Id_team;
                //match.Score_a = 0;
                //match.Score_b = 0;
                //match.Winner = 0;
                //match.Address Preenchido pela view
                //match.Date = DateTime.Now;
                //match.Confirmation Preenchido pela view
                if (match.Status.Length.Equals(0))
                {
                    match.Status = "Em elaboração";
                }
                match.Users_id_users = idUser;

                return View(match);
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }

        }

        /// <summary>
        /// Save changes in Match
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Match match)
        {

            if (ValidIdUser())
            {
                //Lista auxiliar
                //player.ListPositions = uow.PositionsRepositorio.GetAll().Where(x => x.Id_Users.Equals(idUser)).ToList();

                //Validação para tipo
                //ValidaPlayerEdit(player);
                

                if (match.Address == null || match.Address.Trim().Length == 0)
                {
                    ModelState.AddModelError("Address", "O campo endereço é obrigatório");
                }

                if (match.Confirmation == null)
                {
                    ModelState.AddModelError("Confirmation", "O campo data e horário é obrigatório");
                }
                               

                if (ModelState.IsValid)
                {
                    match.Users_id_users = idUser;

                    Team teamA = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ElementAt(0);
                    Team teamB = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ElementAt(1);

                    match.Team_id_team_A = teamA.Id_team;
                    match.Team_id_team_B = teamB.Id_team;
                    //match.Active = 1;

                    uow.MatchRepositorio.Atualizar(match);
                    uow.Commit();

                    //Clear valid Fields
                    //ValidFields();

                    return RedirectToAction("Index", "Match");
                }
                else
                {
                    return View(match);
                }
            }
            else
            {
                return RedirectToAction("Login", "Default", new { @status = 1 });
            }


        }

        /// <summary>
        /// Rotina para envio de convites da partida por email.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult Invitation(Match match)
        {

            if (ValidIdUser())
            {
                //Instância objeto de usuário
                Models.Users user = uowValida.UserRepositorio.GetAll().Where(x => x.Id_users.Equals(idUser)).FirstOrDefault();

                int countPlayers = uowValida.SelectionRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser) && x.Players_id_player > 0).Count();
                
                if (countPlayers.Equals(14))
                {

                    //Preparação de convite por email de jogadores definidos da seleção.
                    //1º Preparar assunto e texto do email.
                    string subject = "Soccer Play - Convite para partida de futebol 7.";

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
                    bodyEmail += bodyEmail = user.Name + " convidou você para uma partida de futebol 7.<br><br>";
                    bodyEmail += bodyEmail = "Confira os detalhes do seu convite abaixo:<br><br>";
                    bodyEmail += bodyEmail = "Data e horário: " + match.Confirmation.ToString() + "<br>";
                    bodyEmail += bodyEmail = "Endereço: " + match.Address + "<br>";
                    bodyEmail += bodyEmail = "<br><a href='http://www.google.com/maps/place/" + match.Latitude + "," + match.Longitude + "' target='_blank'>Clique aqui para ver o endereço no Google Maps</a>";
                    bodyEmail += bodyEmail = "<br><br>Você poderá comparecer nesse jogo?<br><br>";
                    //-----------------------------------------

                    //2º Obter lista de destinatários
                    List<Players> playersList = uowValida.PlayersRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ToList();
                    string destinatarios = string.Empty;
                    string bodyEmailMessage = "";

                    foreach (var item in playersList)
                    {
                        string urlYes = "http://localhost:51858/Match/Confirm?idMatch=" + match.Id_match + "&idPlayer=" + item.Id_player + "&idUser=" + idUser + "&resp=1";
                        string urlNo = "http://localhost:51858/Match/Confirm?idMatch=" + match.Id_match + "&idPlayer=" + item.Id_player + "&idUser=" + idUser + "&resp=0";

                        string htmlLinks = "<a href = '" + urlYes + "' > [ SIM ] </a> ou <a href = '" + urlNo + "' > [ NÃO ] </a>";

                        bodyEmailMessage = bodyEmail + htmlLinks + "</body></html>";

                        if (item.Email.Length > 0)
                        {
                            EmailService.SendEmail(item.Email, bodyEmailMessage, subject);
                        }

                    }

                    //Atualiza status da partida
                    match.Status = "Aguardando resposta dos jogadores";
                    uow.MatchRepositorio.Atualizar(match);
                    uow.Commit();
                    return RedirectToAction("Index", "Match");
                }
                else {

                    return RedirectToAction("Edit", "Match", new { @id = match.Id_match, @Status = 1 });
                }
               
            }
            else {
                return RedirectToAction("Index", "Match");
            }
        }


        /// <summary>
        /// Processa a confirmação de um jogador.
        /// </summary>
        /// <param name="idMatch"></param>
        /// <param name="idPlayer"></param>
        /// <param name="idUser"></param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Confirm(int idMatch, int idPlayer, int idUser, int resp)
        {

            //Exemplo de chamada:
            //http://localhost:51858/Match/Confirm?idMatch=2&idPlayer=1&idUser=1&resp=1

            Match match = uowValida.MatchRepositorio.GetAll().Where(x => x.Id_match.Equals(idMatch) && x.Users_id_users.Equals(idUser)).FirstOrDefault();

            Team teamA = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ElementAt(0);
            Team teamB = uowValida.TeamRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ElementAt(1);

            match.Team_id_team_A = teamA.Id_team;
            match.Team_id_team_B = teamB.Id_team;

            //match.Score_a = 0;
            //match.Score_b = 0;
            //match.Winner = 0;
            //match.Address Preenchido pela view
            //match.Date = DateTime.Now;
            //match.Confirmation Preenchido pela view
            //match.Status = "Em elaboração";

            match.Users_id_users = idUser;

            //Atualizando confirmação do jogador
            Players player = uowValida.PlayersRepositorio.GetAll().Where(x => x.Id_player.Equals(idPlayer) && x.Users_id_users.Equals(idUser)).FirstOrDefault();
            if (resp == 1)
            {
                player.Status = "Confirmado";
            }

            if (resp == 2)
            {
                player.Status = "Não poderá jogar";
            }

            if (resp.Equals(1) || resp.Equals(2))
            {
                uowValida.PlayersRepositorio.Atualizar(player);
                uowValida.Commit();
            }

            //Verifica se todos os jogadores já confirmaram a partida
            //-------------------------------------------------------
            List<Players> listPlayers = uow.PlayersRepositorio.GetAll().Where(x => x.Users_id_users.Equals(idUser)).ToList();
            //Verifica a quantidade de jogadores
            bool allConfirm = true;

            //Definir quantidade de jogadores como parâmetro, 2 para teste e 14 para futebol 7.
            if (listPlayers.Count.Equals(2))
            {
                foreach (var item in listPlayers)
                {
                    if (!item.Status.Equals("Confirmado"))
                    {
                        allConfirm = false;
                    }
                }

                if (allConfirm.Equals(true))
                {
                    //1° Atualiza status da partida
                    match.Status = "Partida Confirmada";
                    uow.MatchRepositorio.Atualizar(match);
                    uow.Commit();

                    //2º Processar novo sorteio
                    //Já implementado na tela de seleções

                    //3° Notificar todos os participantes com a seleção:



                }
            }

            //-------------------------------------------------------

            return View(match);

        }


        [Authorize]
        [HttpGet]
        public ActionResult EndMatch(Match match)
        {
            if (ValidIdUser())
            {
                //Pega lista de todos os jogadores ativos do usuário corrente para indicar novamente que eles estão pendentes.
                List<Players> listPlayers = uow.PlayersRepositorio.GetAll(x => x.Users_id_users.Equals(idUser) && x.Active.Equals(1)).ToList();

                foreach (var item in listPlayers)
                {
                    item.Status = "Pendente";
                    uowValida.PlayersRepositorio.Atualizar(item);
                    uowValida.Commit();
                }
                
                match.Status = "Partida Finalizada";
                uow.MatchRepositorio.Atualizar(match);
                uow.Commit();

                return RedirectToAction("Index", "Match");
            }
            else
            {
                return RedirectToAction("Login", "Default");
            }

        }
    }
}