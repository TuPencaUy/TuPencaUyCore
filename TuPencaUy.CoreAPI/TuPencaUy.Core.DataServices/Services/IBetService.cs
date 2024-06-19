﻿using TuPencaUy.Core.DTOs;

namespace TuPencaUy.Core.DataServices.Services
{
  public interface IBetService
  {
    List<BetDTO> GetBets(out int count, string? userEmail, int? matchId, int? eventId, int? page, int? pageSize);

    BetDTO CreateBet(string userEmail, int matchId, int eventId, int firstTeamScore, int secondTeamScore);

    BetDTO ModifyBet(string? userEmail, int? matchId, int? eventId, int? firstTeamScore, int? secondTeamScore);

    BetDTO DeleteBet(string userEmail, int matchId, int eventId);
  }
}