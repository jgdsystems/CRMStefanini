using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;

namespace CRM.DAO
{
    public interface IUnitOfWork
    {
        IRepositorio<Users> UserRepositorio { get; }
        IRepositorio<Logs> LogsRepositorio { get; }
        IRepositorio<Customers> CustomerRepositorio { get; }
        IRepositorio<Citys> CityRepositorio { get; }
        IRepositorio<Regions> RegionRepositorio { get; }

        void Commit();
    }
}
