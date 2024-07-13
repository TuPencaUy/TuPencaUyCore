using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.Platform.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Platform
{
  public class PlatformPaymentService : IPaymentService
  { 
    private readonly IGenericRepository<Payment> _paymentDAL;
    private readonly IGenericRepository<Event> _eventDAL;

    private int _page = 1;
    private int _pageSize = 10;

    public PlatformPaymentService(
      IGenericRepository<Payment> paymentDAL,
      IGenericRepository<Event> eventDAL
      )
    {
      _paymentDAL = paymentDAL;
      _eventDAL = eventDAL;
    }

    public List<PaymentDTO> GetPayments(out int count, string? userEmail, int? eventId, string? transactionId, int? page, int? pageSize)
    {
      SetPagination(page, pageSize);
      var conditions = new List<Expression<Func<Payment, bool>>>();

      if (userEmail != null) conditions.Add(x => x.User_email == userEmail);
      if (eventId != null) conditions.Add(x => x.Event_id == eventId);
      if (transactionId != null) conditions.Add(x => x.TransactionID == transactionId);

      IQueryable<PaymentDTO> paymentsQuery = _paymentDAL.Get(conditions)
        .Select(x => new PaymentDTO
        {
          Id = x.Id,
          Amount = x.Amount,
          TransactionID = x.TransactionID,
          Event = new EventDTO
          {
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
          },
          User_email = x.User_email
        });

      count = paymentsQuery.Count();

      return paymentsQuery.ToList();
    }

    public PaymentDTO CreatePayment(string userEmail, int eventId, decimal amount, string transactionID, int? siteId)
    {
      var existingPayment = _paymentDAL.Get(new List<Expression<Func<Payment, bool>>> { x => x.Event_id == eventId && x.User_email == userEmail }).Any();
      if (existingPayment) throw new PaymentAlreadyExists($"Payment already exists with event_id: {eventId}, user_email: {userEmail}");

      var @event = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId })
        .FirstOrDefault() ?? throw new EventNotFoundException($"Event not found with id {eventId}");

      var payment = new Payment
      {
        Event = @event,
        User_email = userEmail,
        Amount = amount,
        TransactionID = transactionID,
        Site_id = siteId.Value,
      };

      _paymentDAL.Insert(payment);
      _paymentDAL.SaveChanges();

      return new PaymentDTO
      {
        Id = payment.Id,
        Amount = payment.Amount,
        TransactionID = payment.TransactionID,
        Event = new EventDTO
        {
          Name = payment.Event.Name,
          Comission = payment.Event.Comission,
          MatchesCount = payment.Event.Matches.Count(),
          EndDate = payment.Event.EndDate,
          StartDate = payment.Event.StartDate,
          Id = payment.Event.Id,
          Instantiable = payment.Event.Instantiable,
          TeamType = payment.Event.TeamType,
          Sport = new SportDTO
          {
            Name = payment.Event.Sports.FirstOrDefault().Name,
            Id = payment.Event.Sports.FirstOrDefault().Id,
            ExactPoints = payment.Event.Sports.FirstOrDefault().ExactPoints,
            PartialPoints = payment.Event.Sports.FirstOrDefault().PartialPoints,
            Tie = payment.Event.Sports.FirstOrDefault().Tie,
          }
        },
        User_email = payment.User_email,
      };
    }

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }

    public PaymentDTO ModifyPayment(string userEmail, int eventId, int? amount, string? transactionID)
    {
      throw new NotImplementedException();
    }

    public void DeletePayment(int id)
    {
      throw new NotImplementedException();
    }
  }
}
