namespace GenericRepositoryWithUnitOfWork.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetAll();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteEntity(T entity);
        Task DeleteAsync(int id);
    }
}
