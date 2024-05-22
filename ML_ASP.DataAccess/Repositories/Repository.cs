using Microsoft.EntityFrameworkCore;
using ML_ASP.DataAccess.Repositories.IRepositories;
using System.Linq.Expressions;

namespace ML_ASP.DataAccess.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDBContext _dbContext;
		internal DbSet<T> _dbSet;

        public Repository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
			this._dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
		{
			_dbSet.Add(entity);
		}

		public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null)
		{
			IQueryable<T> query = _dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}
            return query.ToList();
		}

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = _dbSet;
			query = query.Where(filter);

			return query.FirstOrDefault();
		}

		public T GetFirstOrDefault()
		{
			IQueryable<T> query = _dbSet;

			return query.FirstOrDefault();
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}

        public void RemoveRange(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
		}
	}
}
