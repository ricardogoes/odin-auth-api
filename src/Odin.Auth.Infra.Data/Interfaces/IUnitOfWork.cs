namespace Odin.Auth.Infra.Data.EF.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {        
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
