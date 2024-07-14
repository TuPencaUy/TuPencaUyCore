using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.Platform.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformPayoutService : IPayoutService
  { 
    private readonly IGenericRepository<Payout> _payoutDAL;
    private readonly IGenericRepository<Event> _eventDAL;

    private int _page = 1;
    private int _pageSize = 10;

    public PlatformPayoutService(
      IGenericRepository<Payout> payoutDAL,
      IGenericRepository<Event> eventDAL
      )
    {
      _payoutDAL = payoutDAL;
      _eventDAL = eventDAL;
    }

    public List<PayoutDTO> GetPayouts(out int count, string? paypalEmail, int? eventId, string? transactionId, int? siteId, int? page, int? pageSize)
    {
      SetPagination(page, pageSize);
      var conditions = new List<Expression<Func<Payout, bool>>>();

      if (paypalEmail != null) conditions.Add(x => x.PaypalEmail == paypalEmail);
      if (eventId != null) conditions.Add(x => x.Event_id == eventId);
      if (transactionId != null) conditions.Add(x => x.TransactionID == transactionId);
      if (siteId != null) conditions.Add(x => x.Site_id == siteId);

      IQueryable<PayoutDTO> payoutsQuery = _payoutDAL.Get(conditions)
        .Select(x => new PayoutDTO
        {
          Id = x.Id,
          Amount = x.Amount,
          TransactionID = x.TransactionID,
          PaypalEmail = x.PaypalEmail,
          Event = new EventDTO
          {
            Finished = x.Event.Finished,
            Name = x.Event.Name,
            Comission = x.Event.Comission,
            MatchesCount = x.Event.Matches.Count(),
            EndDate = x.Event.EndDate,
            StartDate = x.Event.StartDate,
            Id = x.Event.Id,
            Instantiable = x.Event.Instantiable,
            TeamType = x.Event.TeamType,
            Sport = new SportDTO
            {
              Name = x.Event.Sports.FirstOrDefault().Name,
              Id = x.Event.Sports.FirstOrDefault().Id,
              ExactPoints = x.Event.Sports.FirstOrDefault().ExactPoints,
              PartialPoints = x.Event.Sports.FirstOrDefault().PartialPoints,
              Tie = x.Event.Sports.FirstOrDefault().Tie,
            }
          }
        });

      count = payoutsQuery.Count();

      return payoutsQuery.ToList();
    }

    public PayoutDTO CreatePayout(string paypalEmail, int eventId, int siteId, decimal amount, string transactionID)
    {
      var @event = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId })
        .FirstOrDefault() ?? throw new EventNotFoundException($"Event not found with id {eventId}");

      var payout = new Payout
      {
        Event = @event,
        PaypalEmail = paypalEmail,
        Amount = amount,
        TransactionID = transactionID,
        Site_id = siteId,
      };

      _payoutDAL.Insert(payout);
      _payoutDAL.SaveChanges();

      return new PayoutDTO
      {
        Id = payout.Id,
        Amount = payout.Amount,
        TransactionID = payout.TransactionID,
        PaypalEmail = payout.PaypalEmail,
        Event = new EventDTO
        {
          Finished = payout.Event.Finished,
          Name = payout.Event.Name,
          Comission = payout.Event.Comission,
          MatchesCount = payout.Event.Matches.Count(),
          EndDate = payout.Event.EndDate,
          StartDate = payout.Event.StartDate,
          Id = payout.Event.Id,
          Instantiable = payout.Event.Instantiable,
          TeamType = payout.Event.TeamType,
          Sport = new SportDTO
          {
            Name = payout.Event.Sports.FirstOrDefault().Name,
            Id = payout.Event.Sports.FirstOrDefault().Id,
            ExactPoints = payout.Event.Sports.FirstOrDefault().ExactPoints,
            PartialPoints = payout.Event.Sports.FirstOrDefault().PartialPoints,
            Tie = payout.Event.Sports.FirstOrDefault().Tie,
          }
        }
      };
    }

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }
  }
}
