using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> DbSet;
        public Repository(ApplicationDBContext db)
        {
            
            _db = db;
            this.DbSet = _db.Set<T>();
            _db.Products.Include(u => u.Category).Include(u=>u.CategoryId);   //displaying category in products table
        }
        void IRepository<T>.Add(T entity)
        {
           DbSet.Add(entity);
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> Filter, string? includeProperties = null, bool tracked = false)
        {
            if (tracked)
            {
                IQueryable<T> query = DbSet;
                query = query.Where(Filter);
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return query.FirstOrDefault();
            }
            else
            {
                IQueryable<T> query = DbSet.AsNoTracking();
                query = query.Where(Filter);
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }
                return query.FirstOrDefault();
            }
        }

        IEnumerable<T> IRepository<T>.GetAll(Expression<Func<T, bool>>? Filter, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if(Filter != null)
            {
                query = query.Where(Filter);
            }
             if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
                return query.ToList();
        }

        void IRepository<T>.Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        void IRepository<T>.RemoveRange(IEnumerable<T> entity)
        {
            DbSet.RemoveRange(entity);
        }
    }
}
