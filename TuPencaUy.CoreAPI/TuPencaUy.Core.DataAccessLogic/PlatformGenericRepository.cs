using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TuPencaUy.Platform.DAO.Models.Base;
using TuPencaUy.Platform.DAO.Models.Data;

namespace TuPencaUy.Core.DataAccessLogic
{
  public class PlatformGenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : LogicDelete
  {
    internal PlatformDbContext _context;
    internal DbSet<TEntity> _dbSet;

    public PlatformGenericRepository(PlatformDbContext context)
    {
      _context = context;
      _context.Database.SetCommandTimeout(3600);
      _dbSet = _context.Set<TEntity>();
    }

  public IQueryable<TEntity> Get(
    List<Expression<Func<TEntity, bool>>>? conditions = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    EnumDataStatus dataStatus = EnumDataStatus.Active)
  {
    var query = FilterRegistersByStatus(dataStatus);

    if (conditions is not null)
    {
      foreach (Expression<Func<TEntity, bool>> condition in conditions)
      {
        query = query.Where(condition);
      }
    }

    return orderBy is not null ? orderBy(query) : query;
  }

  public void Insert(TEntity entity)
  {
    if (entity is ControlDate controlDateEntity)
    {
      controlDateEntity.CreationDate = DateTime.Now;
      controlDateEntity.LastModificationDate = DateTime.Now;
    }

    _dbSet.Add(entity);
  }

  public void Delete(object id)
  {
    TEntity entityToDelete = _dbSet.Find(id);
    this.Delete(entityToDelete);
  }
  public void Delete(TEntity entity)
  {
    if (_context.Entry(entity).State == EntityState.Detached)
    {
      _dbSet.Attach(entity);
    }

    _dbSet.Remove(entity);
  }
  public void Update(TEntity entity)
  {
    if (entity is ControlDate controlDateEntity)
    {
      controlDateEntity.LastModificationDate = DateTime.Now;
    }

    _dbSet.Attach(entity);
    _context.Entry(entity).State = EntityState.Modified;
  }

  public void SaveChanges()
  {
    try
    {
      _context.SaveChanges();
    }
    catch (Exception)
    {
      throw;
    }
  }

  private IQueryable<TEntity> FilterRegistersByStatus(EnumDataStatus status)
  {
    IQueryable<TEntity> query;

    switch (status)
    {
      case EnumDataStatus.Active:
        query = _dbSet.Where(q => !q.Inactive);
        break;
      case EnumDataStatus.Inactive:
        query = _dbSet.Where(q => q.Inactive);
        break;
      default:
        query = _dbSet;
        break;
    }

    return query;
  }
}
}
