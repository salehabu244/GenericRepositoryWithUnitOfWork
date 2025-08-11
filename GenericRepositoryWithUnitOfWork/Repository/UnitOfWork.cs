using GenericRepositoryWithUnitOfWork.DAL;
using GenericRepositoryWithUnitOfWork.Repository.Interface;
using GenericRepositoryWithUnitOfWork.Repository.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepositoryWithUnitOfWork.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyAppDbContext _context;
        private readonly Dictionary<Type, object> _repositoris;
        private IDbContextTransaction _transaction;
        public UnitOfWork(MyAppDbContext myAppDbContext)
        {
            _context = myAppDbContext;
            _repositoris = new Dictionary<Type, object>();
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //_transaction?.Dispose();
                    _context?.Dispose();
                }
                disposed = true;
            }
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if(_repositoris.ContainsKey(typeof(T)))
            {
                return _repositoris[typeof(T)] as IRepository<T>;
            }
            var repository = new Repository<T>(_context);
            _repositoris.Add(typeof(T), repository);
            return repository;
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync(); 
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
