using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwoHundred.Server.Entities;

namespace TwoHundred.Server.Abstractions;

public interface IRepository
{
    Task<T?> GetById<T>(Guid id) where T : IEntity;
    IQueryable<T> FindQueryable<T>(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : IEntity;
    Task<List<T>> FindAllAsync<T>(CancellationToken cancellationToken) where T : IEntity;
    T Add<T>(T entity) where T : IEntity;
    void Update<T>(T entity) where T : IEntity;
    void Delete<T>(T entity) where T : IEntity;
}
