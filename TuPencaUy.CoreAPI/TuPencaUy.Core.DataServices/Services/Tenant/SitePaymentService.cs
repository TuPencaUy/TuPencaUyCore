using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.DTOs;
using TuPencaUy.Exceptions;
using TuPencaUy.Site.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Tenant
{
  public class SitePaymentService : IPaymentService
  { 
    private readonly IGenericRepository<Payment> _paymentDAL;
    private readonly IGenericRepository<Event> _eventDAL;
    private readonly IGenericRepository<User> _userDAL;

    private int _page = 1;
    private int _pageSize = 10;

    public SitePaymentService(
      IGenericRepository<Payment> paymentDAL,
      IGenericRepository<Event> eventDAL,
      IGenericRepository<User> userDAL
      )
    {
      _paymentDAL = paymentDAL;
      _eventDAL = eventDAL;
      _userDAL = userDAL;
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
            ReferenceEvent = x.Event.RefEvent,
            Instantiable = x.Event.Instantiable,
            TeamType = x.Event.TeamType,
            Sport = new SportDTO
            {
              Name = x.Event.Sports.FirstOrDefault().Name,
              Id = x.Event.Sports.FirstOrDefault().Id,
              ReferenceSport = x.Event.Sports.FirstOrDefault().RefSport,
              ExactPoints = x.Event.Sports.FirstOrDefault().ExactPoints,
              PartialPoints = x.Event.Sports.FirstOrDefault().PartialPoints,
              Tie = x.Event.Sports.FirstOrDefault().Tie,
            }
          },
          User = new UserDTO
          {
            Email = userEmail,
            Name = x.User.Name,
          }
        });

      count = paymentsQuery.Count();

      return paymentsQuery.ToList();
    }

    public PaymentDTO CreatePayment(string userEmail, int eventId, decimal amount, string transactionID)
    {
      var existingPayment = _paymentDAL.Get(new List<Expression<Func<Payment, bool>>> { x => x.Event_id == eventId && x.User_email == userEmail }).Any();
      if (existingPayment) throw new PaymentAlreadyExists($"Payment already exists with event_id: {eventId}, user_email: {userEmail}");

      var @event = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId })
        .FirstOrDefault() ?? throw new EventNotFoundException($"Event not found with id {eventId}");

      var user = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail })
        .FirstOrDefault() ?? throw new UserNotFoundException($"User not found with email {userEmail}");

      var payment = new Payment
      {
        Event = @event,
        User = user,
        Amount = amount,
        TransactionID = transactionID
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
          ReferenceEvent = payment.Event.RefEvent,
          Instantiable = payment.Event.Instantiable,
          TeamType = payment.Event.TeamType,
          Sport = new SportDTO
          {
            Name = payment.Event.Sports.FirstOrDefault().Name,
            Id = payment.Event.Sports.FirstOrDefault().Id,
            ReferenceSport = payment.Event.Sports.FirstOrDefault().RefSport,
            ExactPoints = payment.Event.Sports.FirstOrDefault().ExactPoints,
            PartialPoints = payment.Event.Sports.FirstOrDefault().PartialPoints,
            Tie = payment.Event.Sports.FirstOrDefault().Tie,
          }
        },
        User = new UserDTO
        {
          Email = userEmail,
          Name = payment.User.Name,
        }
      };
    }

    public PaymentDTO ModifyPayment(string userEmail, int eventId, int? amount, string? transactionID)
    {
      var payment = _paymentDAL.Get(new List<Expression<Func<Payment, bool>>> { x => x.Event_id == eventId && x.User_email == userEmail })
        .FirstOrDefault() ?? throw new PaymentNotFoundException($"Payment not found with event_id: {eventId}, user_email: {userEmail}");

      bool changes = false;

      if (amount != null && amount != payment.Amount)
      {
        payment.Amount = amount.Value;
        changes = changes || true;
      }

      if (transactionID != null && transactionID != payment.TransactionID)
      {
        payment.TransactionID = transactionID;
        changes = changes || true;
      }

      if (changes)
      {
        _paymentDAL.Update(payment);
        _paymentDAL.SaveChanges();
      }

      var @event = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => eventId == x.Id })
        .Select(x => new EventDTO
        {
          Name = x.Name,
          Comission = x.Comission,
          MatchesCount = x.Matches.Count(),
          EndDate = x.EndDate,
          StartDate = x.StartDate,
          Id = x.Id,
          ReferenceEvent = x.RefEvent,
          Instantiable = x.Instantiable,
          TeamType = x.TeamType,
          Sport = new SportDTO
          {
            Name = x.Sports.FirstOrDefault().Name,
            Id = x.Sports.FirstOrDefault().Id,
            ReferenceSport = x.Sports.FirstOrDefault().RefSport,
            ExactPoints = x.Sports.FirstOrDefault().ExactPoints,
            PartialPoints = x.Sports.FirstOrDefault().PartialPoints,
            Tie = x.Sports.FirstOrDefault().Tie,
          }
        })
        .FirstOrDefault();
      var user = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => userEmail == x.Email })
        .Select(x => new UserDTO
        {
          Email = userEmail,
          Name = x.Name,
        })
        .FirstOrDefault();

      return new PaymentDTO
      {
        Id = payment.Id,
        Amount = payment.Amount,
        TransactionID = payment.TransactionID,
        Event = @event,
        User = user
      };
    }

    public void DeletePayment(int id)
    {
      var payment = _paymentDAL.Get(new List<Expression<Func<Payment, bool>>> { x => x.Id == id })
        .FirstOrDefault() ?? throw new PaymentNotFoundException($"Payment not found with id: {id}");

      _paymentDAL.Delete(payment);
      _paymentDAL.SaveChanges();
    }

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }
  }
}
