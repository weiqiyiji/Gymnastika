using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gymnastika.Services.Models;
using Gymnastika.Data;

namespace Gymnastika.Tests.Support
{
    public class InMemoryRepository<T> : Repository<T>
    {
        private IDictionary<int, T> _db;

        public InMemoryRepository() : base(null, null) 
        {
            _db = new Dictionary<int, T>();
        }

        private int GetEntityId<T>(T entity)
        {
            return int.Parse(
                entity.GetType().GetProperty("Id").GetValue(entity, null).ToString());
        }

        #region IRepository<T> Members

        public override void CreateOrUpdate(T entity)
        {
            _db.Add(GetEntityId(entity), entity);
        }

        public override void Create(T entity)
        {
            _db.Add(GetEntityId(entity), entity);
        }

        public override void Update(T entity)
        {
            _db.Add(GetEntityId(entity), entity);
        }

        public override void Delete(T entity)
        {
            _db.Remove(GetEntityId(entity));
        }

        public override void Copy(T source, T target)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override T Get(int id)
        {
            return _db[id];
        }

        public override T Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).SingleOrDefault();
        }

        public override IQueryable<T> Table
        {
            get { return _db.Values.AsQueryable(); }
        }

        public override int Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Table.Count();
        }

        public override IQueryable<T> Fetch(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate);
        }

        public override IQueryable<T> Fetch(System.Linq.Expressions.Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            var orderable = new Orderable<T>(Fetch(predicate));
            order(orderable);
            return orderable.Queryable;
        }

        public override IQueryable<T> Fetch(System.Linq.Expressions.Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count)
        {
            return Fetch(predicate, order).Skip(skip).Take(count);
        }

        #endregion
    }
}
