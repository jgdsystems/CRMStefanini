using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSoccer.DAO;
using WebSoccer.Models;

[Authorize]
public static class Common
{
    
    /// <summary>
    /// Salva log de erro do sistema
    /// </summary>
    /// <param name="idUser">id do usuário</param>
    /// <param name="ex">objeto com a exceção ocorrida</param>
    public static void SendLogError(Object idUser, Exception ex)
    {
        UnitOfWork uow = new UnitOfWork();
        
        Logs log = new Logs()
        {
            Datetime = DateTime.Now,
            Description = "Ocorreu um erro: " + ex.Message + " <br> Detalhes: " + ex.StackTrace,
            Type = 2
        };

        if (idUser != null)
        {
            log.Id_user = Int32.Parse(idUser.ToString());
        }
        

        uow.LogsRepositorio.Adicionar(log);
        uow.Commit();
    }

    



   
}

    
 
