using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OperationAdminRepository.Interface
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> AsQueryable();
        void Delete(T Entity);
        T DeleteByKey(int PrymaryKey);

        T GetById(int id);

        void Insert(T Entity);
      
        void Update(T Entity);

      
        void Save();
        Task SaveAsync();
    }
}
