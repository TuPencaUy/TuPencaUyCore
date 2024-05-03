using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Platform.DAO.Models.Logic
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
