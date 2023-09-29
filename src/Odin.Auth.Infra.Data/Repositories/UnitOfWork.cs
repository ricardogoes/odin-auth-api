using Odin.Auth.Infra.Data.EF.Interfaces;

namespace Odin.Auth.Infra.Data.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OdinMasterDbContext _dbContext;
        private bool _disposed;

        public UnitOfWork(OdinMasterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task CommitAsync(CancellationToken cancellationToken)
            => _dbContext.SaveChangesAsync(cancellationToken);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _dbContext.Dispose();
            _disposed = true;
        }
    }
}
