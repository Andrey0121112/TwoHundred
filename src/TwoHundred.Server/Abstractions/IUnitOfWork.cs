using System.Threading.Tasks;
using System.Threading;
using System;

namespace TwoHundred.Server.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IRepository Repository();
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
