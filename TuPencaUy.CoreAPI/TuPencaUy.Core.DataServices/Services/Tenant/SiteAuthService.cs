namespace TuPencaUy.Core.DataServices.Services.Platform
{
  using System.Linq.Expressions;
  using System.IdentityModel.Tokens.Jwt;
  using TuPencaUy.DTOs;
  using TuPencaUy.Site.DAO.Models;
  using TuPencaUy.Core.DataAccessLogic;
  using TuPencaUy.Core.Enums;
  using TuPencaUy.Exceptions;
  using TuPencaUy.Core.DataServices.Services.CommonLogic;
  using TuPencaUy.Core.DTOs;
  using TuPencaUy.Core.Exceptions;

  public class SiteAuthService : IAuthService
  {
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<Role> _roleDAL;
    private readonly IAuthLogic _authLogic;
    public SiteAuthService(
      IGenericRepository<User> userDAL,
      IGenericRepository<Role> roleDAL,
      IAuthLogic authLogic)
    {
      _userDAL = userDAL;
      _roleDAL = roleDAL;
      _authLogic = authLogic;
    }
    public UserDTO? Authenticate(string email, string password, SiteAccessTypeEnum siteAccess)
    {
      bool auth = siteAccess != SiteAccessTypeEnum.Open && siteAccess != SiteAccessTypeEnum.Closed;
      if (auth)
      {
        var accessStatus = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email }).Select(x => x.AccessRequest.AccessStatus)
          .FirstOrDefault();
        if (accessStatus != AccessStatusEnum.Accepted) throw new UserNotAdmitedException($"User {email} not admited.");
      }

      var user = _userDAL
        .Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })
        .Select(user => new UserDTO
        {
          PaypalEmail = user.PaypalEmail,
          AccessStatus = user.AccessRequest.AccessStatus,
          Password = user.Password,
          Email = user.Email,
          Id = user.Id,
          Name = user.Name,
          Role = user.Role != null ? new RoleDTO
          {
            Name = user.Role.Name,
            Id = user.Role.Id,
            Permissions = user.Role.Permissions
             .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
             .ToList() ?? new List<PermissionDTO>()
          } : null,
          Events = user.Events != null ? user.Events.Select(ev =>
           new EventDTO
           {
             Finished = ev.Finished,
             Id = ev.Id,
             ReferenceEvent = ev.RefEvent,
             Name = ev.Name,
             Comission = ev.Comission,
             EndDate = ev.EndDate,
             StartDate = ev.StartDate,
             Instantiable = ev.Instantiable,
             MatchesCount = ev.Matches.Count(),
             TeamType = ev.TeamType,
             Sport = ev.Sports.Any() ?  new SportDTO
             {
               Id = ev.Sports.FirstOrDefault().Id,
               ReferenceSport = ev.Sports.FirstOrDefault().RefSport,
               Name = ev.Sports.FirstOrDefault().Name,
               Tie = ev.Sports.FirstOrDefault().Tie,
               PartialPoints = ev.Sports.FirstOrDefault().PartialPoints,
               ExactPoints = ev.Sports.FirstOrDefault().ExactPoints,
             } : null
           }).ToList() : new List<EventDTO>()
        })
        .FirstOrDefault();

      if (user == null) throw new InvalidCredentialsException();

      return user.Password.Equals(_authLogic.HashPassword(password, user.Password.Split('$')[0])) ? user : null;
    }
    public UserDTO? Authenticate(string token, SiteAccessTypeEnum siteAccess, bool allowedRegister)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      bool auth = siteAccess != SiteAccessTypeEnum.Open && siteAccess != SiteAccessTypeEnum.Closed;

      var jwtToken = tokenHandler.ReadJwtToken(token);
      var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

      var user = _userDAL
      .Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail })
      .Select(user => new UserDTO
      {
        PaypalEmail = user.PaypalEmail,
        AccessStatus = user.AccessRequest.AccessStatus,
        Email = user.Email,
        Id = user.Id,
        Name = user.Name,
        Role = user.Role != null ? new RoleDTO
        {
          Name = user.Role.Name,
          Id = user.Role.Id,
          Permissions = user.Role.Permissions
           .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
           .ToList() ?? new List<PermissionDTO>()
        } : null,
        Events = user.Events != null ? user.Events.Select(ev =>
         new EventDTO
         {
           Finished = ev.Finished,
           Id = ev.Id,
           ReferenceEvent = ev.RefEvent,
           Name = ev.Name,
           Comission = ev.Comission,
           EndDate = ev.EndDate,
           StartDate = ev.StartDate,
           Instantiable = ev.Instantiable,
           MatchesCount = ev.Matches.Count(),
           TeamType = ev.TeamType,
           Sport = new SportDTO
           {
             Id = ev.Id,
             ReferenceSport = ev.Sports.FirstOrDefault().Id,
             Name = ev.Sports.FirstOrDefault().Name,
             Tie = ev.Sports.FirstOrDefault().Tie,
             PartialPoints = ev.Sports.FirstOrDefault().PartialPoints,
             ExactPoints = ev.Sports.FirstOrDefault().ExactPoints,
           }
         }).ToList() : new List<EventDTO>()
      })
      .FirstOrDefault();

      if(user == null && (siteAccess == SiteAccessTypeEnum.Closed || (siteAccess == SiteAccessTypeEnum.ByInvite && !allowedRegister))) throw new UserNotAdmitedException($"User {userEmail} not admited.");

      if (user == null)
      {
        var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        return CreateNewUser(siteAccess, userEmail, userName, null, auth);
      }

      if (auth && user.Role.Id != (int)UserRoleEnum.Admin)
      {
        var accessStatus = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail }).Select(x => x.AccessRequest.AccessStatus)
          .FirstOrDefault();
        if (accessStatus != AccessStatusEnum.Accepted) throw new UserNotAdmitedException($"User {userEmail} not admited.");
      }

      return user;
    }
    public UserDTO? SignUp(string email, string password, string name, SiteAccessTypeEnum siteAccess)
    {
      bool auth = siteAccess != SiteAccessTypeEnum.Open && siteAccess != SiteAccessTypeEnum.Closed;
      var existingUser = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email });
      if (existingUser.Any()) return null;

      return CreateNewUser(siteAccess, email, name, _authLogic.HashPassword(password), auth);
    }
    private UserDTO CreateNewUser(SiteAccessTypeEnum siteAccess, string email, string name, string password = null, bool? auth = false)
    {
      Role role = _roleDAL
        .Get(new List<Expression<Func<Role, bool>>> { x => x.Id == (int)UserRoleEnum.BasicUser })
        .FirstOrDefault();

      User user = new User
      {
        Email = email,
        Name = name,
        Role = role,
        Password = password
      };

      user.AccessRequest = new AccessRequest
      {
        User_email = email,
        AccessStatus = siteAccess == SiteAccessTypeEnum.Open ? AccessStatusEnum.Accepted : AccessStatusEnum.Pending,
        RequestTime = DateTime.Now,
      };

      _userDAL.Insert(user);
      _userDAL.SaveChanges();

      var userDTO = new UserDTO
      {
        Id = user.Id,
        Email = email,
        Name = name,
        AccessStatus = user.AccessRequest.AccessStatus,
        PaypalEmail = user.PaypalEmail,
      };

      if (role != null)
      {
        userDTO.Role = new RoleDTO
        {
          Name = role.Name,
          Id = role.Id
        };

        if (role.Permissions != null)
        {
          userDTO.Role.Permissions = role.Permissions
            .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
            .ToList() ?? new List<PermissionDTO>();
        }
      }

      return userDTO;
    }
  }
}
