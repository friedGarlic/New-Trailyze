using System.Linq.Expressions;

namespace ML_ASP.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
		void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        public T GetFirstOrDefault();

	}
}