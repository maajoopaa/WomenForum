using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Templates.Models;
using Templates.Repositories.Interfaces;

namespace Templates.Repositories;

public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity> 
    where TEntity : BaseDbEntityWithId
    where TContext : DbContext
{
    private readonly TContext _context;
    protected readonly DbSet<TEntity> DbSet;

    public BaseRepository(TContext context)
    {
        _context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        await DbSet.AddRangeAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        DbSet.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        DbSet.RemoveRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken)
    {
        return await DbSet.Where(predicate ?? (entity => true)).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(
        Expression<Func<TEntity, bool>>? predicate,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = DbSet.Where(predicate ?? (entity => true));
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderBy(e => e.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(items, totalCount, pageNumber, pageSize);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task UpdateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        DbSet.UpdateRange(entities);
        await _context.SaveChangesAsync(cancellationToken);
    }
}