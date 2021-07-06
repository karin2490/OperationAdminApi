
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OperationAdminRepository.Interface;

namespace OperationAdminRepository.Repository
{
    public abstract class RepositoryBase<T>:IRepositoryBase<T> where T: class
    {
        private readonly DbContext DBCon;
       
        public RepositoryBase(DbContext DBContext)
        {
            this.DBCon = DBContext;
        }

        public System.Linq.IQueryable<T> AsQueryable()
        {
            return this.DBCon.Set<T>().AsQueryable();
        }

        public void Delete (T Entity)
        {
            this.DBCon.Set<T>().Remove(Entity);
        }
        public void Delete<T>(T Entity) where T : class
        {
            this.DBCon.Set<T>().Remove(Entity);
        }
        public T DeleteByKey(int primaryKey)
        {
            var Entity = this.DBCon.Set<T>().Find(primaryKey);
            if (Entity != null)
            {
                this.DBCon.Set<T>().Remove(Entity);
            }
            return Entity;
        }

        public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return this.DBCon.Set<T>().Where(predicate);
        }
        public IEnumerable<T> GetAll()
        {
            return this.DBCon.Set<T>();
        }
        public IEnumerable<T> GetAll<T>() where T : class
        {
            return this.DBCon.Set<T>();
        }


        public T GetById(int Id)
        {
            return this.DBCon.Set<T>().Find(Id);
        }
        public T GetById<T>(int Id) where T : class
        {
            return this.DBCon.Set<T>().Find(Id);
        }
      
        public async Task<T> GetByIdAsync(int Id)
        {
            return await DBCon.Set<T>().FindAsync(Id);
        }
        public async Task<T> GetByIdAsync<T>(int Id) where T : class
        {
            return await DBCon.Set<T>().FindAsync(Id);
        }

        //public T FindOneByCondition(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        //{
        //    return this.DBCon.Set<T>().Where(predicate).FirstOrDefault();
        //}
        //public T FindOneByCondition<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T:class
        //{
        //    return this.DBCon.Set<T>().Where(predicate).FirstOrDefault();
        //}

        public void Insert(T Entity)
        {
            if (this.DBCon.Entry<T>(Entity).State != EntityState.Detached)
                this.DBCon.Entry<T>(Entity).State = EntityState.Added;
            else
                this.DBCon.Set<T>().Add(Entity);
        }
        public void Insert<T>(T Entity)where T:class
        {
            if (this.DBCon.Entry<T>(Entity).State != EntityState.Detached)
                this.DBCon.Entry<T>(Entity).State = EntityState.Added;
            else
                this.DBCon.Set<T>().Add(Entity);
        }

       public void Update(T entity)
        {
            this.DBCon.Set<T>().Update(entity);
        }
        public void Update<T>(T entity)where T:class
        {
            this.DBCon.Set<T>().Update(entity);
        }
        public void Save()
        {
            DBCon.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await DBCon.SaveChangesAsync();
        }

        
    }
}
