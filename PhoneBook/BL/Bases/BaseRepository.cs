using BL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BL.Bases
{
   public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbContext DbContext { get; set; }
        protected DbSet<T> DbSet { get; set; }
        public BaseRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }
        public virtual IQueryable<T> GetAll()
        {
            var dd = DbSet;
            return dd;
        }
        public IQueryable<T> GetAllSorted<TKey>(Expression<Func<T, TKey>> sortingExpression)
        {
            return DbSet.OrderBy<T, TKey>(sortingExpression);
        }
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query;
        }
        public bool GetAny(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = DbSet;
            bool result = false;
            if (filter != null)
            {
                result = query.AsNoTracking().Any(filter);
            }
            return result;
        }
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return DbSet.AsNoTracking().FirstOrDefault(filter);
            }
            return null;
        }
        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }
        public virtual T GetById(long id)
        {
            return DbSet.Find(id);
        }
        public virtual bool Insert(T entity)
        {
            bool returnVal = false;
            EntityEntry<T> dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
            returnVal = true;
            return returnVal;
        }

        public virtual void InsertList(List<T> entityList)
        {
            DbSet.AddRange(entityList);
        }

        public virtual void Update(T entity)
        {
            EntityEntry<T> dbEntityEntry = DbContext.Entry(entity);


            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void UpdateList(List<T> entityList)
        {
            foreach (T item in entityList)
            {
                Update(item);
            }
        }
        public virtual T GetById(string id)
        {
            return DbSet.Find(id);
        }
        public virtual void Delete(string id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }
        public virtual void Delete(T entity)
        {
            EntityEntry<T> dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }
        public virtual void DeleteList(List<T> entityList)
        {
            foreach (T item in entityList)
            {
                Delete(item);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }
        public virtual int CountEntity()
        {
            return DbSet.Count();
        }
       
    }
}

