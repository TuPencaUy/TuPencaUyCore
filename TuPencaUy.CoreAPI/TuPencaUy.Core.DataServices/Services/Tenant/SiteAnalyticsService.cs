﻿using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.Site.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Tenant
{
  public class SiteAnalyticsService : IAnalyticsService
  {
    private readonly IGenericRepository<Bet> _betDAL;
    private readonly IGenericRepository<Event> _eventDAL;

    private int _page = 1;
    private int _pageSize = 10;

    public SiteAnalyticsService(IGenericRepository<Bet> betDAL, IGenericRepository<Event> eventDAL)
    {
      _betDAL = betDAL;
      _eventDAL = eventDAL;
    }

    public List<BetUserDTO> GetLeaderboard(out int count, int eventId, int? page = null, int? pageSize = null)
    {
      SetPagination(page, pageSize);

      var points = _eventDAL
        .Get([ev => ev.Id == eventId])?
        .Select(x => new
        {
          x.Sports.FirstOrDefault().PartialPoints,
          x.Sports.FirstOrDefault().ExactPoints,
        })?.FirstOrDefault() ?? throw new EventNotFoundException($"Event not found with id {eventId}");

      IQueryable<BetUserDTO> betUsers = _betDAL
        .Get([bet => bet.Event_id == eventId && bet.Match.Finished])
        .GroupBy(bet => new { bet.User.Name , bet.User_email })
        .Select(x => new BetUserDTO
        {
          Name = x.Key.Name,
          Email = x.Key.User_email,
          PredictedMatches = x.Count(),
          Points = x.Sum(bet => bet.Points ?? 0),
          Hits = x.Count(bet => bet.Points == points.ExactPoints),
          PartialHits = x.Count(bet => bet.Points == points.PartialPoints),
        });

      count = betUsers.Count();

      return betUsers.OrderByDescending(x => x.Points).Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();
    }
    public List<BetMatchDTO> GetMatchBets(int? matchId)
    {
      var conditions = new List<Expression<Func<Bet, bool>>>();

      if(matchId != null) conditions.Add(x => x.Match_id == matchId);

      var matchBets = _betDAL.Get(conditions)
        .GroupBy(bet => new
        {
          eventName = bet.Match.Event.Name,
          bet.Match.Date,
          firstTeamName = bet.Match.FirstTeam.Name,
          secondTeamName = bet.Match.SecondTeam.Name,
          firstTeamLogo = bet.Match.FirstTeam.Logo,
          secondTeamLogo = bet.Match.SecondTeam.Logo
        })
        .Select(x => new BetMatchDTO
        {
          EventName = x.Key.eventName,
          MatchDate = x.Key.Date.Value,
          FirstTeam = x.Key.firstTeamName,
          SecondTeam = x.Key.secondTeamName,
          FirstTeamLogo = x.Key.firstTeamLogo,
          SecondTeamLogo = x.Key.secondTeamLogo,
          TotalBets = x.Count(),
          FirstTeamWinnerBets = x.Count(x => x.ScoreFirstTeam > x.ScoreSecondTeam),
          SecondTeamWinnerBets = x.Count(x => x.ScoreFirstTeam < x.ScoreSecondTeam),
          TieBets = x.Count(x => x.ScoreFirstTeam == x.ScoreSecondTeam),
          PopularBets = x
            .GroupBy(g => new { g.ScoreFirstTeam, g.ScoreSecondTeam })
            .Select(s => new BetScoreDTO
            {
              FirstTeamScore = s.Key.ScoreFirstTeam,
              SecondTeamScore = s.Key.ScoreSecondTeam,
              TotalBets = s.Count(),
            })
            .OrderByDescending(o => o.TotalBets).Take(3)
            .ToList(),
        }).ToList();

      matchBets.ForEach(mb => mb.PopularBets.ForEach(pb => pb.BetPercentage = (decimal)pb.TotalBets / (decimal)mb.TotalBets));

      return matchBets;
    }
    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }
  }
}
