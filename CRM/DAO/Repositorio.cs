﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CRM.DAO
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private AppContexto m_Context = null;
        DbSet<T> m_DbSet;
        
        public Repositorio(AppContexto context)
        {
            m_Context = context;
            m_DbSet = m_Context.Set<T>();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return m_DbSet.AsNoTracking().Where(predicate);
            }
            return m_DbSet.AsEnumerable();
        }
        public T Get(Expression<Func<T, bool>> predicate)
        {
            return m_DbSet.FirstOrDefault(predicate);
        }
        public void Adicionar(T entity)
        {
            m_DbSet.Add(entity);
        }
        public void Atualizar(T entity)
        {
            m_DbSet.Attach(entity);
            ((IObjectContextAdapter)m_Context).ObjectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
        }
        public void Deletar(T entity)
        {
            m_DbSet.Remove(entity);
        }
        public int Contar()
        {
            return m_DbSet.Count();
        }
    }

}