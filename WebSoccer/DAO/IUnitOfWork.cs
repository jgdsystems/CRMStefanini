using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSoccer.Models;

namespace WebSoccer.DAO
{
    public interface IUnitOfWork
    {
        IRepositorio<Users> UserRepositorio { get; }
        IRepositorio<Logs> LogsRepositorio { get; }
        IRepositorio<Players> PlayersRepositorio { get; }
        IRepositorio<Positions> PositionsRepositorio { get; }
        IRepositorio<Team> TeamRepositorio { get; }
        IRepositorio<Match> MatchRepositorio { get; }
        IRepositorio<Selection> SelectionRepositorio { get; }

        void Commit();
    }
}
