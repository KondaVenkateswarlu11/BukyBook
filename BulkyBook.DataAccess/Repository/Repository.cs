using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Dataccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _Db;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext db)
        {
            _Db = db;
            this.dbset = _Db.Set<T>();
        }
    
        public void Add(T entity)
        {
           dbset.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
            {
                query = query.Where(filter);

            }
           
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T GetFirstorDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            if(includeProperties != null)
            {
                foreach(var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();

        }

        public void Remove(T entity)
        {
           dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
           dbset.RemoveRange(entities);
        }
    }
}
