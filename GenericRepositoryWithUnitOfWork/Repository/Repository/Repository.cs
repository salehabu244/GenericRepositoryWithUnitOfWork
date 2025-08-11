using GenericRepositoryWithUnitOfWork.DAL;
using GenericRepositoryWithUnitOfWork.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryWithUnitOfWork.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly MyAppDbContext _myAppDbContext;
        public Repository(MyAppDbContext myAppDbContext)
        {
            _dbSet = myAppDbContext.Set<T>();
            _myAppDbContext = myAppDbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _myAppDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _dbSet.Remove(_dbSet.Find(id));
            await _myAppDbContext.SaveChangesAsync();
        }
        public async Task DeleteEntity(T entity)
        {
            _dbSet.Remove(entity);
            await _myAppDbContext.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _myAppDbContext.Entry(entity).State = EntityState.Modified;
            await _myAppDbContext.SaveChangesAsync();
        }
    }
}
