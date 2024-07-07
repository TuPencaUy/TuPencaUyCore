using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.Site.DAO.Models;

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
        .Get([ev => ev.RefEvent == eventId])?
        .Select(x => new
        {
          x.Sports.FirstOrDefault().PartialPoints,
          x.Sports.FirstOrDefault().ExactPoints,
        })?.FirstOrDefault() ?? throw new EventNotFoundException($"Event not found with id {eventId}");

      var sites = _serviceFactory.GetService<ISiteService>().GetSites(); 

      List<Bet> bets = new List<Bet>();

      foreach (var site in sites)
      {
        _serviceFactory.CreateTenantServices(site.ConnectionString);
        var betDAL = _serviceFactory.GetService<IGenericRepository<Bet>>();
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

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }
  }
}
