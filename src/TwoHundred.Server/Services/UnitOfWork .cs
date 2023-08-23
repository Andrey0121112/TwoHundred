using System.Threading.Tasks;
using System.Threading;
using System;
using TwoHundred.Server.Abstractions;

namespace TwoHundred.Server.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMainDbContext _databaseContext;

    public UnitOfWork(IMainDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IRepository Repository()
    {
        return new Repository(_databaseContext);
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return _databaseContext.SaveChangesAsync(cancellationToken);
    }


    private bool _disposed;

    ~UnitOfWork()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }        

        if (disposing)
        {
            _databaseContext.Dispose();
        }
        
        _disposed = true;        
    }
}
