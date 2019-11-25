using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CRM.DAO;
using CRM.Models;
using System.Dynamic;
using System.Configuration;
using Facebook;
using CRM.Services;
using System.Text.RegularExpressions;

namespace CRM.Controllers
{
    public class DefaultController : Controller
    {
        UnitOfWork uow = new UnitOfWork();

        public ActionResult Login()
        {
            ViewBag.status = Request["status"];
            return View();
        }

        [HttpPost]
        public ActionResult ValidaLogin(string email, string password)
        {
            try
            {
                if (email == null || email.Trim().Length == 0)
                {
                    ModelState.AddModelError("Email", "O campo Email é obrigatório");
                }

                if (password == null || password.Trim().Length == 0)
                {
                    ModelState.AddModelError("Password", "O campo senha é obrigatório");
                }
                
                if (ModelState.IsValid)
                {
                    string passwordHash = Services.Md5Hash.CalculaHash(password);

                    var itens = uow.UserRepositorio.GetAll().Where(x => x.Email.Equals(email) && x.Password.Equals(passwordHash)).ToList();

                    if (itens.Count().Equals(1))
                    {
                        //Isto adiciona um novo cookie utilizado para a autenticação do usuário.
                        FormsAuthentication.SetAuthCookie(email, false);
                        return RedirectToAction("Index", "Customer");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Default", new { @status = 1 });
                    }
                }
                else
                {
                    return View("Login");
                }


            }
            catch (Exception ex)
            {
                Common.SendLogError(null, ex);
                return View("Error");
            }
        }

        public ActionResult ExitLogin()
        {
            Session.RemoveAll();
            Session.Abandon();

            //eliminar o cookie de autenticação
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Default");

        }
        
        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(Users user)
        {
            try
            {
                var itens = uow.UserRepositorio.GetAll().Where(x => x.Email.Equals(user.Email)).ToList();
                if (itens.Count().Equals(1))
                {
                    ModelState.AddModelError("Email", "Esse email já foi cadastrado por outro usuário.");
                }

                if (user.Captcha == null)
                {
                    ModelState.AddModelError("Captcha", "O código de verificação deve ser informado.");
                }

                if (Session["imgKey"] != null)
                {
                    String captcha = Session["imgKey"].ToString();
                    if (user.Captcha != captcha)
                    {
                        ModelState.AddModelError("Captcha", "O código de verificação informado está incorreto.");
                    }
                }

                if (user.Password != null)
                {
                    Regex rgx = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$");
                    if (!rgx.IsMatch(user.Password))
                    {
                        ModelState.AddModelError("Password", "A senha deve possuir Mínimo de oito caracteres, pelo menos uma letra, um número e um caractere especial.");
                    }
                }

                if (ModelState.IsValid)
                {
                    //Criando um novo usuário
                    user.Type = "1";
                    user.Create_date = DateTime.Now;
                    user.Last_access = DateTime.Now;
                    user.Active = 1;
                    user.Profile = "1";
                    user.Password = Services.Md5Hash.CalculaHash(user.Password);
                    user.Captcha = string.Empty;
                    user.PasswordRecover = string.Empty;
                    uow.UserRepositorio.Adicionar(user);
                    uow.Commit();

                    return RedirectToAction("NewUserFeedBack", "Default");
                }
                else
                {
                    return View(user);
                }

            }
            catch (Exception ex)
            {
                Common.SendLogError(null, ex);
                return View("Error");
            }

        }
        
        [HttpGet]
        public ActionResult Recover()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Recover(Users user)
        {
            if (user.Email != null && user.Email != "")
            {
                //1º Validar se email existe
                var itens = uow.UserRepositorio.GetAll().Where(x => x.Email.Equals(user.Email) && x.Active.Equals(1)).ToList();

                if (itens.Count().Equals(1))
                {
                    //2º Gerar código hash de acesso para formulário de redefinição de senha
                    string passwordRecover = Md5Hash.CalculaHash(Md5Hash.stringRandom(8));

                    //3º Salvar passwordHash no cadastro do usuário
                    foreach (var item in itens)
                    {
                        item.PasswordRecover = passwordRecover;
                        uow.UserRepositorio.Atualizar(item);
                        uow.Commit();
                    }

                    //4º Preparação de email para recuperação de senha.
                    string subject = "CRM (Teste stefanini) - Novo acesso.";

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
                    bodyEmail += bodyEmail = "Soccer Play<br><br>";
                    bodyEmail += bodyEmail = "Você solicitou a recuperação de seu acesso.<br>";
                    bodyEmail += bodyEmail = "Para continuar clique no link abaixo e defina sua nova senha:<br>";
                    bodyEmail += bodyEmail = "<br><a href='http://localhost:51858/Default/Access/?id=" + passwordRecover + "' target='_blank'>Clique aqui para redefinir sua senha.</a>";
                    bodyEmail += bodyEmail = "</body></html>";
                    
                    //Enviando email
                    EmailService.SendEmail(user.Email, bodyEmail, subject);

                    return RedirectToAction("FeedBack", "Default");
                }
                else {

                    ModelState.AddModelError("Email", "O Email informado não foi encontrado.");

                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Access()
        {
            try
            {
                var codeAccess = Request["id"];
                var itens = uow.UserRepositorio.GetAll().Where(x => x.PasswordRecover.Equals(codeAccess)).ToList();
                
                if (itens.Count().Equals(1))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Default");
                }
            }
            catch (Exception ex)
            {
                Common.SendLogError(0, ex);
                return RedirectToAction("Login", "Default");
            }
        }

        [HttpPost]
        public ActionResult Access(Users user)
        {
            try
            {
                var codeAccess = Request["id"];
                bool Invalid = false;

                #region Validações
                if (user.Password == null)
                {
                    ModelState.AddModelError("Password", "O campo senha deve ser informado.");
                    Invalid = true;
                }

                if (user.PasswordRecover == null)
                {
                    ModelState.AddModelError("PasswordRecover", "O campo Repita nova senha deve ser informado.");
                    Invalid = true;
                }

                if (user.Password != user.PasswordRecover)
                {
                    ModelState.AddModelError("PasswordRecover", "O campo Repita nova senha não é igual ao campo senha.");
                    Invalid = true;
                }
                #endregion

                if (Invalid.Equals(false))
                {
                    if (user.Password.Equals(user.PasswordRecover))
                    {
                        //Recupera objeto user do password Recover
                        List<Users> listUser = uow.UserRepositorio.GetAll().Where(x => x.PasswordRecover.Equals(codeAccess)).ToList();
                        if (listUser.Count().Equals(1))
                        {
                            foreach (var item in listUser)
                            {
                                item.Password = Md5Hash.CalculaHash(user.Password);
                                item.PasswordRecover = null;

                                //Atualiza repositório
                                uow.UserRepositorio.Atualizar(item);
                                uow.Commit();
                            }
                        }

                        return RedirectToAction("AccessSuccess", "Default");
                    }
                }

                return View();
                
            }
            catch (Exception ex)
            {
                Common.SendLogError(0, ex);
                return RedirectToAction("Login", "Default");
            }
        }

        [HttpGet]
        public ActionResult FeedBack()
        {
            return View();
        }


        [HttpGet]
        public ActionResult NewUserFeedBack()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AccessSuccess()
        {
            return View();
        }


    }

        
}