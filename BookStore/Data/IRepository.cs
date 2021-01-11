using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BookStore.Data
{
    public interface IRepository<T>
    {
        T GetById(int id);
        List<T> GetAll();
        List<T> FindBy(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        void Insert(T t);
        void Delete(T t);
        void Delete(int id);
        void Update(T t);
        void Update(Expression<Func<T, bool>> filter,
        IEnumerable<object> updatedSet,
        IEnumerable<object> availableSet, 
        string propertyName);

    }
}