using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.Platform.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformAnalyticsService : IAnalyticsService
  {
    private readonly IServiceFactory _serviceFactory;
    private readonly IGenericRepository<Event> _eventDAL;

    private int _page = 1;
    private int _pageSize = 10;

    public PlatformAnalyticsService(IServiceFactory serviceFactory, IGenericRepository<Event> eventDAL)
    {
      _serviceFactory = serviceFactory;
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

      var sites = _serviceFactory.GetService<ISiteService>().GetSites(); 

      List<TuPencaUy.Site.DAO.Models.Bet> bets = new List<TuPencaUy.Site.DAO.Models.Bet>();

      foreach (var site in sites)
      {
        _serviceFactory.CreateTenantServices(site.ConnectionString);
        var betDAL = _serviceFactory.GetService<IGenericRepository<TuPencaUy.Site.DAO.Models.Bet>>();
        bets.AddRange(betDAL.Get([bet => bet.Event.RefEvent == eventId && bet.Match.Finished]).ToList());
      }

      var betUsers = bets.GroupBy(bet => new { bet.User.Name, bet.User_email })
        .Select(x => new BetUserDTO
        {
          Name = x.Key.Name,
          Email = x.Key.User_email,
          PredictedMatches = x.Count(),
          Points = x.Sum(bet => bet.Points ?? 0),
          Hits = x.Count(bet => bet.Points == points.ExactPoints),
          PartialHits = x.Count(bet => bet.Points == points.PartialPoints),
        }).ToList();

      count = betUsers.Count;

      return betUsers.OrderByDescending(x => x.Points).Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();
    }

    public List<BetEventDTO> GetEventBets(int? eventId)
    {
      throw new NotImplementedException();
    }

    public List<BetMatchDTO> GetMatchBets(int? matchId)
    {
      var conditions = new List<Expression<Func<TuPencaUy.Site.DAO.Models.Bet, bool>>>();

      if (matchId != null) conditions.Add(x => x.Match_id == matchId);

      var sites = _serviceFactory.GetService<ISiteService>().GetSites();

      List<TuPencaUy.Site.DAO.Models.Bet> bets = new List<TuPencaUy.Site.DAO.Models.Bet>();

      foreach (var site in sites)
      {
        _serviceFactory.CreateTenantServices(site.ConnectionString);
        var betDAL = _serviceFactory.GetService<IGenericRepository<TuPencaUy.Site.DAO.Models.Bet>>();
        bets.AddRange(betDAL.Get(conditions).ToList());
      }

      var matchBets = bets
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
