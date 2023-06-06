using System.Linq.Expressions;

namespace SchoolAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task Remove(T entity);
        Task Save();
    }
}
