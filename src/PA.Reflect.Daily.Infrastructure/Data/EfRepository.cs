using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PA.Reflect.Daily.Core.Interfaces;

namespace PA.Reflect.Daily.Infrastructure.Data;

public class EfRepository<T> : IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  private readonly DbContext dbContext;
  private readonly ISpecificationEvaluator specificationEvaluator;

  public EfRepository(AppDbContext dbContext)
      : this(dbContext, SpecificationEvaluator.Default)
  {
  }

  /// <inheritdoc/>
  public EfRepository(AppDbContext dbContext, ISpecificationEvaluator specificationEvaluator)
  {
    this.dbContext = dbContext;
    this.specificationEvaluator = specificationEvaluator;
  }

  /// <inheritdoc/>
  public virtual T Add(T entity)
  {
    dbContext.Set<T>().Add(entity);

    return entity;
  }

  /// <inheritdoc/>
  public virtual IEnumerable<T> AddRange(IEnumerable<T> entities)
  {
    dbContext.Set<T>().AddRange(entities);

    return entities;
  }

  /// <inheritdoc/>
  public virtual void Update(T entity)
  {
    dbContext.Set<T>().Update(entity);

  }

  /// <inheritdoc/>
  public virtual void UpdateRange(IEnumerable<T> entities)
  {
    dbContext.Set<T>().UpdateRange(entities);

  }

  /// <inheritdoc/>
  public virtual void Delete(T entity)
  {
    dbContext.Set<T>().Remove(entity);

  }

  /// <inheritdoc/>
  public virtual void DeleteRange(IEnumerable<T> entities)
  {
    dbContext.Set<T>().RemoveRange(entities);
  }

  /// <inheritdoc/>
  public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
  {
    return await dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
  }

  /// <inheritdoc/>
  [Obsolete]
  public virtual async Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
  }

  /// <inheritdoc/>
  [Obsolete]
  public virtual async Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<T?> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    return await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
  {
    return await dbContext.Set<T>().ToListAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

    return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
  }

  /// <inheritdoc/>
  public virtual async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    var queryResult = await ApplySpecification(specification).ToListAsync(cancellationToken);

    return specification.PostProcessingAction == null ? queryResult : specification.PostProcessingAction(queryResult).ToList();
  }

  /// <inheritdoc/>
  public virtual async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return await ApplySpecification(specification, true).CountAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
  {
    return await dbContext.Set<T>().CountAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return await ApplySpecification(specification, true).AnyAsync(cancellationToken);
  }

  /// <inheritdoc/>
  public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
  {
    return await dbContext.Set<T>().AnyAsync(cancellationToken);
  }

  /// <summary>
  /// Filters the entities  of <typeparamref name="T"/>, to those that match the encapsulated query logic of the
  /// <paramref name="specification"/>.
  /// </summary>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <returns>The filtered entities as an <see cref="IQueryable{T}"/>.</returns>
  protected virtual IQueryable<T> ApplySpecification(ISpecification<T> specification, bool evaluateCriteriaOnly = false)
  {
    return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
  }

  /// <summary>
  /// Filters all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
  /// <paramref name="specification"/>, from the database.
  /// <para>
  /// Projects each entity into a new form, being <typeparamref name="TResult" />.
  /// </para>
  /// </summary>
  /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
  /// <param name="specification">The encapsulated query logic.</param>
  /// <returns>The filtered projected entities as an <see cref="IQueryable{T}"/>.</returns>
  protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
  {
    return specificationEvaluator.GetQuery(dbContext.Set<T>().AsQueryable(), specification);
  }
}
