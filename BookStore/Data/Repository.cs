using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BookStore.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {

        internal BookStoreContext context;
        internal DbSet<T> dbSet;

        public Repository(BookStoreContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }


        public void Delete(T t)
        {
            dbSet.Remove(t);
        }

        public void Delete(int id)
        {
            dbSet.Remove(dbSet.Find(id));
        }

        public List<T> FindBy(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            } 
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
            
        }

        
        
        public void Update(T t)
        {
            dbSet.Attach(t);
            context.Entry(t).State = EntityState.Modified;
        }

        public void Update (Expression<Func<T, bool>> filter,
        IEnumerable<object> updatedSet, // Updated many-to-many relationships
        IEnumerable<object> availableSet, // Lookup collection
        string propertyName) // The name of the navigation property
        {
            // Get the generic type of the set
            var type = updatedSet.GetType().GetGenericArguments()[0];

            // Get the previous entity from the database based on repository type
            var previous = context
                .Set<T>()
                .Include(propertyName)
                .FirstOrDefault(filter);

            /* Create a container that will hold the values of
                * the generic many-to-many relationships we are updating.
                */
            var values = CreateList(type);

            /* For each object in the updated set find the existing
                 * entity in the database. This is to avoid Entity Framework
                 * from creating new objects or throwing an
                 * error because the object is already attached.
                 */
            foreach (var entry in updatedSet
                .Select(obj => (int)obj
                    .GetType()
                    .GetProperty("Id")
                    .GetValue(obj, null))
                .Select(value => context.Set(type).Find(value)))
            {
                values.Add(entry);
            }

            /* Get the collection where the previous many to many relationships
                * are stored and assign the new ones.
                */
            context.Entry(previous).Collection(propertyName).CurrentValue = values;
        }

        private IList CreateList(Type type)
        {
            var genericList = typeof(List<>).MakeGenericType(type);
            return (IList)Activator.CreateInstance(genericList);
        }
    }
}