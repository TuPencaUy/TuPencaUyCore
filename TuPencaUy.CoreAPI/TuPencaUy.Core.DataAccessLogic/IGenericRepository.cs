using System.Linq.Expressions;
using TuPencaUy.Platform.DAO;

namespace TuPencaUy.Core.DataAccessLogic
{
  public interface IGenericRepository<TEntity> where TEntity : class
  {
    IQueryable<TEntity> Get(
      List<Expression<Func<TEntity, bool>>>? conditions = null,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      EnumDataStatus dataStatus = EnumDataStatus.Active);

    void Insert(TEntity entity);
    void Delete(object id);
    void Delete(TEntity entity);
    void Update(TEntity entity);
    void SaveChanges();
  }
}
