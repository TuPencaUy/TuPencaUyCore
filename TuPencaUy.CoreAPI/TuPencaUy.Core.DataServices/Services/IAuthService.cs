﻿namespace TuPencaUy.Core.DataServices.Services
{
  using TuPencaUy.DTOs;

  public interface IAuthService
  {
    UserDTO? Authenticate(string email, string password);
    UserDTO? Authenticate(string token);
    Tuple<string, DateTime> GenerateToken(UserDTO user, string? currentTenant = null);
  }
}