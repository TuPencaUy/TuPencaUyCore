using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DataServices.Services.CommonLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Enums;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.DTOs;
using TuPencaUy.Exceptions;
using TuPencaUy.Site.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class SiteUserService : IUserService
  {
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<Role> _roleDAL;
    private readonly IGenericRepository<Event> _eventDAL;
    private readonly IAuthLogic _authLogic;
    public SiteUserService(
      IGenericRepository<User> userDAL,
      IGenericRepository<Role> roleDAL,
      IGenericRepository<Event> eventDAL,
      IAuthLogic authLogic)
    {
      _userDAL = userDAL;
      _roleDAL = roleDAL;
      _eventDAL = eventDAL;
      _authLogic = authLogic;
    }
    public Tuple<UserDTO, EventDTO> SuscribeUser(int userId, int eventId)
    {
      Event @event = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId })
        .FirstOrDefault() ?? throw new EventNotFoundException($"Event not found with id: {eventId}");

      User user = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Id == userId})
        .FirstOrDefault() ?? throw new UserNotFoundException($"User not found with id: {userId}");

      if(@event.Users == null) @event.Users = new List<User>();

      @event.Users.Add(user);
      _eventDAL.Update(@event);
      _eventDAL.SaveChanges();

      return new Tuple<UserDTO, EventDTO>(
        new UserDTO
        {
          Id = userId,
          Email = user.Email,
          Role = user.Role == null ? null : new RoleDTO
          {
            Name = user.Role.Name,
            Id = user.Role.Id,
            Permissions = user.Role.Permissions == null ? null :
              user.Role.Permissions
              .ToList()
              .Select(p => new PermissionDTO { Name = p.Name, Id = p.Id })
              .ToList()
          },
        },
        new EventDTO
        {
          Name = @event.Name,
          Id= @event.Id,
          ReferenceEvent = @event.RefEvent,
          StartDate = @event.StartDate,
          EndDate = @event.EndDate,
          Comission = @event.Comission,
          Instantiable = @event.Instantiable,
          TeamType = @event.TeamType,
          MatchesCount = @event.Matches.Count(),
          Sport = @event.Sports.Select(x => new SportDTO
          {
            ReferenceSport = x.RefSport,
            Name = x.Name,
            Tie = x.Tie,
            Id = x.Id,
            ExactPoints = x.ExactPoints,
            PartialPoints = x.PartialPoints,
          }).FirstOrDefault(),
        });
    }
    public UserDTO GetUserById(int id)
    {
      return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Id == id })
        .Select(x => new UserDTO
        {
          Name = x.Name,
          Email = x.Email,
          Id = id,
          Password = x.Password,
          Role = x.Role == null ? null : new RoleDTO
          {
            Name = x.Role.Name,
            Id = x.Role.Id,
            Permissions = x.Role.Permissions == null ? null :
              x.Role.Permissions
              .ToList()
              .Select(p => new PermissionDTO { Name = p.Name, Id = p.Id })
              .ToList()
          },
        })
        .FirstOrDefault() ?? throw new UserNotFoundException();
    }
    public List<UserDTO> GetUsersByEvent(int eventId)
    {
      if(!_eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId }).Any())
      {
        throw new EventNotFoundException($"Event not found with id: {eventId}");
      }

      var ev = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId })
        .FirstOrDefault() ?? throw new EventNotFoundException($"Event not found with id: {eventId}");


      ICollection<User> users = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId }).Select(x => x.Users).FirstOrDefault();
      List<UserDTO> usersDTO = users.Select(x => new UserDTO
      {
        Email = x.Email,
        Name = x.Name,
        Id = x.Id,
        Role = x.Role != null ? new RoleDTO
        {
          Id = x.Role.Id,
          Name = x.Role.Name,
          Permissions = x.Role.Permissions?
            .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
            .ToList() ?? new List<PermissionDTO>()
        } : null
      }
      ).ToList() ?? new List<UserDTO>();

      return usersDTO;
    }

    public UserDTO GetUserByEmail(string email)
    {
      return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })
        .Select(x => new UserDTO
        {
          Name = x.Name,
          Email = email,
          Id = x.Id,
          Password = x.Password,
          Role = x.Role == null ? null : new RoleDTO
          {
            Name = x.Role.Name,
            Id = x.Role.Id,
            Permissions = x.Role.Permissions == null ? null :
              x.Role.Permissions
              .ToList()
              .Select(p => new PermissionDTO { Name = p.Name, Id = p.Id })
              .ToList()
          },
        })
        .FirstOrDefault() ?? throw new UserNotFoundException();
    }

    public RoleDTO GetRolesByUser(string email)
    {
      return _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == email })?
        .Select(x => x.Role != null ? new RoleDTO
        {
          Id = x.Role.Id,
          Name = x.Role.Name,
          Permissions = x.Role.Permissions
            .Select(y => new PermissionDTO { Id = y.Id, Name = y.Name })
            .ToList() ?? new List<PermissionDTO>()
        } : null)
        .FirstOrDefault() ?? throw new UserNotFoundException();
    }

    public bool CreateUser(string email, string name, string? password, UserRoleEnum role)
    {
      Role userRole = _roleDAL.Get(new List<Expression<Func<Role, bool>>>
        {
          x => x.Id == (int)role
        }).FirstOrDefault();

      var newUser = new User
      {
        Email = email,
        Name = name,
        Password = password,
      };
      if (userRole is not null) newUser.Role = userRole;

      _userDAL.Insert(newUser);
      _userDAL.SaveChanges();

      return true;
    }

    public UserDTO ModifyUser(int userId, string? email, string? name, string? password)
    {
      var dbUser = _userDAL.Get(new List<Expression<Func<User, bool>>> { user => user.Id == userId })
        .FirstOrDefault() ?? throw new UserNotFoundException();

      if (email is not null && email != dbUser.Email)
      {
        var userWithEmail = _userDAL.Get(new List<Expression<Func<User, bool>>>
        {
          user => user.Email == user.Email
        }).Any();
        if (userWithEmail)
        {
          throw new EmailAlreadyInUseException($"The email {email} is already in use");
        }
      }

      if (email is not null) dbUser.Email = email;
      if (name is not null) dbUser.Name = name;
      if (password is not null) dbUser.Password = _authLogic.HashPassword(password);

      if ((email is not null && email != dbUser.Email)
        || (name is not null && name != dbUser.Name)
        || (password is not null))
      {
        _userDAL.Update(dbUser);
        _userDAL.SaveChanges();
      }

      return new UserDTO
      {
        Name = dbUser.Name,
        Email = dbUser.Email,
        Id = dbUser.Id,
        Password = dbUser.Password,
        Role = dbUser.Role == null ? null : new RoleDTO
        {
          Name = dbUser.Role.Name,
          Id = dbUser.Role.Id,
          Permissions = dbUser.Role.Permissions == null ? null :
              dbUser.Role.Permissions
              .ToList()
              .Select(p => new PermissionDTO { Name = p.Name, Id = p.Id })
              .ToList()
        }
      };
    }
  }
}
