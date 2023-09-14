using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> DbSet;
        public Repository(ApplicationDBContext db)
        {
            
            _db = db;
            this.DbSet = _db.Set<T>();
        }
        void IRepository<T>.Add(T entity)
        {
           DbSet.Add(entity);
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> Filter)
        {
            IQueryable<T> query = DbSet;
            query = query.Where(Filter);
            return query.FirstOrDefault();
        }

        IEnumerable<T> IRepository<T>.GetAll()
        {
            IQueryable<T> query = DbSet;
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
