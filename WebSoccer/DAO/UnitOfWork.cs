using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSoccer.Models;

namespace WebSoccer.DAO
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private AppContexto _contexto = null;

        private Repositorio<Users> userRepositorio = null;
        private Repositorio<Logs> logsRepositorio = null;
        private Repositorio<Players> playersRepositorio = null;
        private Repositorio<Positions> positionsRepositorio = null;
        private Repositorio<Team> teamRepositorio = null;
        private Repositorio<Match> matchRepositorio = null;
        private Repositorio<Selection> selectionRepositorio = null;

        public UnitOfWork()
        {
            _contexto = new AppContexto();
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        //-----------------------------------------------
        // Definição de repositórios
        //Referencia para solução do erro de stackoverflow
        //https://stackoverflow.com/questions/9798697/system-stackoverflowexception-when-get-set-properties-are-used

        public IRepositorio<Users> UserRepositorio
        {
            get
            {
                if (userRepositorio == null)
                {
                    userRepositorio = new Repositorio<Users>(_contexto);
                }
                return userRepositorio;
            }
        }
        public IRepositorio<Players> PlayersRepositorio
        {
            get
            {
                if (playersRepositorio == null)
                {
                    playersRepositorio = new Repositorio<Players>(_contexto);
                }
                return playersRepositorio;
            }
        }

        public IRepositorio<Logs> LogsRepositorio
        {
            get
            {
                if (logsRepositorio == null)
                {
                    logsRepositorio = new Repositorio<Logs>(_contexto);
                }
                return logsRepositorio;
            }
        }

        public IRepositorio<Positions> PositionsRepositorio
        {
            get
            {
                if (positionsRepositorio == null)
                {
                    positionsRepositorio = new Repositorio<Positions>(_contexto);
                }
                return positionsRepositorio;
            }
        }


        public IRepositorio<Team> TeamRepositorio
        {
            get
            {
                if (teamRepositorio == null)
                {
                    teamRepositorio = new Repositorio<Team>(_contexto);
                }
                return teamRepositorio;
            }
        }

        public IRepositorio<Match> MatchRepositorio
        {
            get
            {
                if (matchRepositorio == null)
                {
                    matchRepositorio = new Repositorio<Match>(_contexto);
                }
                return matchRepositorio;
            }
        }

        public IRepositorio<Selection> SelectionRepositorio
        {
            get
            {
                if (selectionRepositorio == null)
                {
                    selectionRepositorio = new Repositorio<Selection>(_contexto);
                }
                return selectionRepositorio;
            }
        }



        //-----------------------------------------------

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _contexto.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}