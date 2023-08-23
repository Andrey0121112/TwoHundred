using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using TwoHundred.Server.Abstractions;
using TwoHundred.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace TwoHundred.Server.Services;

public class Repository : IRepository
{
    private readonly IMainDbContext _dbContext;

    public Repository(IMainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetById<T>(Guid id) where T : IEntity
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public IQueryable<T> FindQueryable<T>(Expression<Func<T, bool>> expression,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null) where T : IEntity
    {
        var query = _dbContext.Set<T>().Where(expression);
        return orderBy != null ? orderBy(query) : query;
    }

    public Task<List<T>> FindAllAsync<T>(CancellationToken cancellationToken) where T : IEntity
    {
        return _dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public T Add<T>(T entity) where T : IEntity
    {
        return _dbContext.Set<T>().Add(entity).Entity;
    }

    public void Update<T>(T entity) where T : IEntity
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete<T>(T entity) where T : IEntity
    {
        _dbContext.Set<T>().Remove(entity);
    }
}
