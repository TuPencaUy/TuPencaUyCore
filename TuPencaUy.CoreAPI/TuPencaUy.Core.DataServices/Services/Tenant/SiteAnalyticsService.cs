﻿using TuPencaUy.Core.DataAccessLogic;
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

      return betUsers.Skip((_page - 1) * _pageSize).Take(_pageSize).ToList();
    }
    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }
  }
}