using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PA.Reflect.Daily.Core.Interfaces;
using PA.Reflect.Daily.Core.ProjectAggregate;

namespace PA.Reflect.Daily.Infrastructure.Data;
public class UnitOfWork : IUnitOfWork
{
  private readonly AppDbContext _context;
  private IDbContextTransaction? _transaction;
  private IRepository<User> _userRepository;
  private IRepository<Reflection> _reflectionRepository;
  private IFileStorageRepository _fileStorageRepository;

  public IRepository<User> UserRepository => _userRepository;
  public IRepository<Reflection> ReflectionRepository => _reflectionRepository;
  public IFileStorageRepository FileStorageRepository => _fileStorageRepository;

  public UnitOfWork(IDbContextFactory<AppDbContext> dbContextFactory)
  {
    _context = dbContextFactory.CreateDbContext();
    _userRepository = new EfRepository<User>(_context);
    _reflectionRepository = new EfRepository<Reflection>(_context);
    _fileStorageRepository = new FileStorageRepository("");
  }

  public void BeginTransaction()
  {
    if (_transaction != null)
    {
      return;
    }

    _transaction = _context.Database.BeginTransaction();
  }

  public Task<int> SaveChangesAsync()
  {
    return _context.SaveChangesAsync();
  }

  public void Commit()
  {
    if (_transaction == null)
    {
      return;
    }

    _transaction.Commit();
    _transaction.Dispose();
    _transaction = null;
  }

  public async Task SaveAndCommitAsync()
  {
    await SaveChangesAsync();
    Commit();
  }

  public void Rollback()
  {
    if (_transaction == null)
    {
      return;
    }

    _transaction.Rollback();
    _transaction.Dispose();
    _transaction = null;
  }

  public void Dispose()
  {
    if (_transaction == null)
    {
      return;
    }

    _transaction.Dispose();
    _transaction = null;
  }
}
