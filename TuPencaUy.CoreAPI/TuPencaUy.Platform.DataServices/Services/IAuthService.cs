﻿namespace TuPencaUy.Platform.DataServices.Services
{
  using TuPencaUy.DTOs;

  public interface IAuthService
  {
    UserDTO? Authenticate(string email, string password);
    UserDTO? Authenticate(string token);
    Tuple<string, DateTime> GenerateToken(UserDTO user);
  }
}
