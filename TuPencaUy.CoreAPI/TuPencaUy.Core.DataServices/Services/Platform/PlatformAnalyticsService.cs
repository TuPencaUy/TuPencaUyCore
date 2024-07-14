﻿using Microsoft.Extensions.Configuration;
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
    private readonly IGenericRepository<Payment> _paymentDAL;
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<TuPencaUy.Platform.DAO.Models.Site> _siteDAL;

    private int _page = 1;
    private int _pageSize = 10;

    public PlatformAnalyticsService(
      IConfiguration configuration,
      IGenericRepository<Event> eventDAL,
      IGenericRepository<Payment> paymentDAL,
      IGenericRepository<User> userDAL,
      IGenericRepository<TuPencaUy.Platform.DAO.Models.Site> siteDAL)
    {
      _serviceFactory = new ServiceFactory(configuration);
      _paymentDAL = paymentDAL;
      _eventDAL = eventDAL;
      _siteDAL = siteDAL;
      _userDAL = userDAL;
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

      var sites = _siteDAL.Get().ToList(); 

      List<TuPencaUy.Site.DAO.Models.Bet> bets = new List<TuPencaUy.Site.DAO.Models.Bet>();
      List<TuPencaUy.Site.DAO.Models.User> users = new List<Site.DAO.Models.User>();

      foreach (var site in sites)
      {
        _serviceFactory.CreateTenantServices(site.ConnectionString);
        var betDAL = _serviceFactory.GetService<IGenericRepository<TuPencaUy.Site.DAO.Models.Bet>>();
        var userDAL = _serviceFactory.GetService<IGenericRepository<TuPencaUy.Site.DAO.Models.User>>();
        bets.AddRange(betDAL.Get([bet => bet.Event.RefEvent == eventId && bet.Match.Finished]).ToList());


        users.AddRange(userDAL.Get([x => x.Events.Any(x => x.RefEvent == eventId)]).ToList());

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

      betUsers = users.GroupBy(x => new { x.Name,  x.Email })
        .Select(x => new BetUserDTO
         {
           Name = x.Key.Name,
           Email = x.Key.Email,
           PredictedMatches = 0,
           Points = 0,
           Hits = 0,
           PartialHits = 0,
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

      var sites = _siteDAL.Get().ToList();

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

    public PlatformFinancesAnalyticsDTO GetFinances()
    {
      var sitesFinances = _paymentDAL.Get()
        .GroupBy(x => x.Site_id)
        .Select(x => new SiteFinanceDTO
        {
          SiteId = x.Key.Value,
          SiteName = x.First().Site.Name,
          TotalRaised = x.Sum(p => p.Amount) * (x.First().Event.Comission ?? 0),
        }).ToList();

      var eventFinances = _paymentDAL.Get()
        .GroupBy(x => new { x.Event_id, x.Event.Comission, x.Event.Name, x.Event.Id })
        .Select(x => new EventFinanceDTO
        {
          EventId = x.Key.Id,
          EventName = x.Key.Name,
          TotalRaised = x.Sum(p => p.Amount) * (x.First().Event.Comission ?? 0),
        }).ToList();

      return new PlatformFinancesAnalyticsDTO
      {
        Events = eventFinances,
        Sites = sitesFinances,
      };
    }

    public List<SiteDTO> GetSitesAnalytics()
    {
      var sites = _siteDAL.Get()
        .Select(x => new SiteDTO
        {
          Id = x.Id,
          PaypalEmail = x.PaypalEmail,
          AccessType = x.AccessType,
          Color = x.Color,
          Domain = x.Domain,
          Name = x.Name,
        }).ToList();
      foreach(var site in sites)
      {
        _serviceFactory.CreateTenantServices(site.ConnectionString);
        site.TotalUsers = _serviceFactory.GetService<IGenericRepository<TuPencaUy.Site.DAO.Models.User>>().Get().Count();
      }

      return sites;
    }
    public List<EventDTO> GetEventsAnalytics()
    {
      var sites = _siteDAL.Get()
        .Select(x => new SiteDTO
        {
          Id = x.Id,
          PaypalEmail = x.PaypalEmail,
          AccessType = x.AccessType,
          Color = x.Color,
          Domain = x.Domain,
          Name = x.Name,
        }).ToList();

      var globalEvents = new List<EventDTO>();

      foreach (var site in sites)
      {
        _serviceFactory.CreateTenantServices(site.ConnectionString);
        var events = _serviceFactory.GetService<IGenericRepository<TuPencaUy.Site.DAO.Models.Event>>().Get()
          .Select(x => new EventDTO
          {
            Name = x.Name,
            Id = x.RefEvent,
            TotalUsers = x.Users.Count(u => !u.Inactive)
          });

        globalEvents.AddRange(events);
      }

      globalEvents = globalEvents.GroupBy(x => new { x.Id, x.Name, x.TotalUsers }).Select(x => new EventDTO
      {
        Name = x.Key.Name,
        Id = x.Key.Id,
        TotalUsers = x.Sum(e => e.TotalUsers)
      }).ToList();

      return globalEvents;
    }

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }

  }
}
