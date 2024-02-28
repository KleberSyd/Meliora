namespace Meliora.Repository.Repositories;

public interface IRepository<T>
{
    Task<T?> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task Add(T entity);
    Task Update(T entity);
    Task Remove(int id);
}