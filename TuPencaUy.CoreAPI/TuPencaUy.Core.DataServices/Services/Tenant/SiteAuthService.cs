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
    public UserDTO? Authenticate(string email, string password)
    {
      var user = _userDAL
        .Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })
        .Select( user => new UserDTO
        {
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

      if (user == null) throw new InvalidCredentialsException();

      return user.Password.Equals(_authLogic.HashPassword(password, user.Password.Split('$')[0])) ? user : null;

    }
    public UserDTO? Authenticate(string token)
    {
      var tokenHandler = new JwtSecurityTokenHandler();

      try
      {
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

        var user = _userDAL
        .Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail })
        .Select(user => new UserDTO
        {
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

        if (user == null)
        {
          var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
          return CreateNewUser(userEmail, userName);
        }

        return user;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public UserDTO? SignUp(string email, string password, string name)
    {
      var existingUser = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email });
      if (existingUser.Any()) return null;

      return CreateNewUser(email, name, _authLogic.HashPassword(password));
    }
    private UserDTO CreateNewUser(string email, string name, string password = null)
    {
      Role role = _roleDAL
        .Get(new List<Expression<Func<Role, bool>>> { x => x.Id == (int)UserRoleEnum.BasicUser })
        .FirstOrDefault();

      _userDAL.Insert(new User
      {
        Email = email,
        Name = name,
        Role = role,
        Password = password
      });

      _userDAL.SaveChanges();

      var userDTO = new UserDTO
      {
        Email = email,
        Name = name
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
