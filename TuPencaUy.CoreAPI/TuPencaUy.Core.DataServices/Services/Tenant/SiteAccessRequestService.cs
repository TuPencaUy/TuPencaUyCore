using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.DTOs;
using TuPencaUy.Site.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Tenant
{
  public class SiteAccessRequestService(IGenericRepository<AccessRequest> accessRequestDAL) : IAccessRequestService
  {
    private readonly IGenericRepository<AccessRequest> _accessRequestDAL = accessRequestDAL;

    public AccessRequestDTO ChangeState(AccessStatusEnum accessStatusEnum, string email)
    {
      var accessRequest = _accessRequestDAL.Get(new List<System.Linq.Expressions.Expression<Func<AccessRequest, bool>>> { x => x.User_email == email })
        .FirstOrDefault() ?? throw new AccessRequestNotFoundException($"Access request not found for {email}");

      accessRequest.AccessStatus = accessStatusEnum;

      _accessRequestDAL.Update(accessRequest);
      _accessRequestDAL.SaveChanges();

      return new AccessRequestDTO
      {
        AccessStatusCode = accessStatusEnum,
        AccessStatus = Enum.GetName(typeof(AccessStatusEnum), accessStatusEnum),
        RequestTime = accessRequest.RequestTime,
        User = new UserDTO
        {
          Email = email,
          Id = accessRequest.User.Id,
          Name = accessRequest.User.Name,
        }
      };
    }

    public List<AccessRequestDTO> GetAccessRequests(out int count, string? email, AccessStatusEnum? accessStatusEnum, int? page, int? pageSize)
    {
      var conditions = new List<Expression<Func<AccessRequest, bool>>>();

      if (email != null) conditions.Add(x => x.User_email == email);
      if (accessStatusEnum != null) conditions.Add(x => x.AccessStatus == accessStatusEnum);

      IQueryable<AccessRequestDTO> query = _accessRequestDAL.Get(conditions)
        .Select(x => new AccessRequestDTO
        {
          AccessStatusCode = x.AccessStatus,
          // AccessStatus = Enum.GetName(typeof(AccessStatusEnum), x.AccessStatus),
          RequestTime = x.RequestTime,
          User = new UserDTO
          {
            Email = x.User_email,
            Id = x.User.Id,
            Name = x.User.Name,
          }
        });

      count = query.Count();

      List<AccessRequestDTO> result = new List<AccessRequestDTO>();
      if (pageSize == int.MaxValue && page == int.MaxValue)
      {
        result = query.ToList();
        result.ForEach(x =>
        {
          x.AccessStatus = Enum.GetName(typeof(AccessStatusEnum), x.AccessStatusCode);
        });

        return result;
      }

      if (pageSize == null || pageSize == 0) pageSize = 10;
      if (page == null || page == 0) page = 1;

      result = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
      result.ForEach(x =>
      {
        x.AccessStatus = Enum.GetName(typeof(AccessStatusEnum), x.AccessStatusCode);
      });

      return result;
    }
  }
}
