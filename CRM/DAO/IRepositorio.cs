﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAO
{
    public interface IRepositorio<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);

        T Get(Expression<Func<T, bool>> predicate);

        void Adicionar(T entity);

        void Atualizar(T entity);

        void Deletar(T entity);

        int Contar();
    }

}
