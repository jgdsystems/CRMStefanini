using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Models;

namespace CRM.DAO
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private AppContexto _contexto = null;

        private Repositorio<Users> userRepositorio = null;
        private Repositorio<Logs> logsRepositorio = null;
        private Repositorio<Customers> customerRepositorio = null;
        private Repositorio<Citys> cityRepositorio = null;
        private Repositorio<Regions> regionRepositorio = null;


        public UnitOfWork()
        {
            _contexto = new AppContexto();
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

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

        public IRepositorio<Customers> CustomerRepositorio
        {
            get
            {
                if (customerRepositorio == null)
                {
                    customerRepositorio = new Repositorio<Customers>(_contexto);
                }
                return customerRepositorio;
            }
        }

        public IRepositorio<Citys> CityRepositorio
        {
            get
            {
                if (cityRepositorio == null)
                {
                    cityRepositorio = new Repositorio<Citys>(_contexto);
                }
                return cityRepositorio;
            }
        }

        public IRepositorio<Regions> RegionRepositorio
        {
            get
            {
                if (regionRepositorio == null)
                {
                    regionRepositorio = new Repositorio<Regions>(_contexto);
                }
                return regionRepositorio;
            }
        }

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