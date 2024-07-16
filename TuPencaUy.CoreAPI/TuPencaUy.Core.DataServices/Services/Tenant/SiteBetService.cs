using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.Core.Exceptions;
using TuPencaUy.DTOs;
using TuPencaUy.Exceptions;
using TuPencaUy.Site.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Tenant
{
  public class SiteBetService : IBetService
  {
    private readonly IGenericRepository<Bet> _betDAL;
    private readonly IGenericRepository<Event> _eventDAL;
    private readonly IGenericRepository<Match> _matchDAL;
    private readonly IGenericRepository<User> _userDAL;
    private readonly IGenericRepository<Payment> _paymentDAL;

    private int _page = 1;
    private int _pageSize = 10;

    public SiteBetService(
      IGenericRepository<Bet> betDAL,
      IGenericRepository<Event> eventDAL,
      IGenericRepository<Match> matchDAL,
      IGenericRepository<User> userDAL,
      IGenericRepository<Payment> paymentDAL
    )
    {
      _betDAL = betDAL;
      _eventDAL = eventDAL;
      _matchDAL = matchDAL;
      _userDAL = userDAL;
      _paymentDAL = paymentDAL;
    }

    public EventPaymentDTO EndEvent(int eventId)
    {
      var ev = _eventDAL.Get([x => x.Id == eventId])
        .FirstOrDefault() ?? throw new EventNotFoundException($"Not found event with id {eventId}");

      var winner = _betDAL.Get([x => x.Event_id == eventId])
        .GroupBy(x => new { x.User_email })
        .Select(bet => new
        {
          UserEmail = bet.Key.User_email,
          TotalPoints = bet.Sum(b => b.Points)
        })
        .OrderByDescending(bet => bet.TotalPoints)
        .FirstOrDefault();

      var payment = _paymentDAL.Get([x => x.Event_id == eventId])
        .Sum(x => x.Amount);

      var user = winner != null ? _userDAL.Get([x => x.Email == winner.UserEmail])
        .Select(x => new UserDTO
        {
          Email = x.Email,
          PaypalEmail = x.PaypalEmail,
          Name = x.Name,
          Id = x.Id,
        }).FirstOrDefault() : null;

      ev.Finished = true;

      _eventDAL.Update(ev);
      _eventDAL.SaveChanges();

      double prizeAmount = (double)(payment * (1 - (decimal)ev.Comission.Value) * ev.PrizePercentage);
      double siteRevenueAmount = (double)(payment * (1 - (decimal)ev.Comission.Value) - (decimal)prizeAmount);
      return new EventPaymentDTO
      {
        PrizeAmount = prizeAmount,
        Winner = user,
        SiteRevenueAmount = siteRevenueAmount,
      };
    }

    public BetDTO CreateBet(string userEmail, int matchId, int eventId, int firstTeamScore, int secondTeamScore)
    {
      var existingBet = _betDAL.Get(new List<Expression<Func<Bet, bool>>>
        { x => x.Event_id == eventId && x.Match_id == matchId && x.User_email == userEmail }).Any();
      if (existingBet)
        throw new BetAlreadyExists(
          $"Bet already exists with event_id: {eventId}, user_email: {userEmail}, match_id: {matchId}");

      var @event = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId })
        .FirstOrDefault() ?? throw new EventNotFoundException($"Event not found with id {eventId}");

      var match = _matchDAL.Get(new List<Expression<Func<Match, bool>>>
          { x => x.Id == matchId && x.Event_id == eventId })
        .FirstOrDefault() ?? throw new MatchNotFoundException($"Match not found with if {matchId} for event {eventId}");

      if (match.Date < DateTime.Now) throw new MatchAlreadyStartedException($"The match {matchId} has already started");

      var user = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail })
        .FirstOrDefault() ?? throw new UserNotFoundException($"User not found with email {userEmail}");

      var matchTie = _matchDAL.Get(new List<Expression<Func<Match, bool>>> { x => x.Id == matchId })
        .Select(x => x.Sport.Tie).FirstOrDefault();

      if (!matchTie && firstTeamScore == secondTeamScore) throw new SportTieException();

      var bet = new Bet
      {
        Event = @event,
        Match = match,
        User = user,
        ScoreFirstTeam = firstTeamScore,
        ScoreSecondTeam = secondTeamScore,
        Points = 0,
      };

      _betDAL.Insert(bet);
      _betDAL.SaveChanges();

      return new BetDTO
      {
        ScoreFirstTeam = bet.ScoreFirstTeam,
        ScoreSecondTeam = bet.ScoreSecondTeam,
        Points = bet.Points,
        Event = new EventDTO
        {
          Finished = bet.Event.Finished,
          Name = bet.Event.Name,
          Comission = bet.Event.Comission,
          MatchesCount = bet.Event.Matches.Count(),
          EndDate = bet.Event.EndDate,
          StartDate = bet.Event.StartDate,
          Id = bet.Event.Id,
          ReferenceEvent = bet.Event.RefEvent,
          Instantiable = bet.Event.Instantiable,
          TeamType = bet.Event.TeamType,
          Sport = new SportDTO
          {
            Name = bet.Event.Sports.FirstOrDefault().Name,
            Id = bet.Event.Sports.FirstOrDefault().Id,
            ReferenceSport = bet.Event.Sports.FirstOrDefault().RefSport,
            ExactPoints = bet.Event.Sports.FirstOrDefault().ExactPoints,
            PartialPoints = bet.Event.Sports.FirstOrDefault().PartialPoints,
            Tie = bet.Event.Sports.FirstOrDefault().Tie,
          }
        },
        Match = new MatchDTO
        {
          Finished = bet.Match.Finished,
          Date = bet.Match.Date,
          FirstTeamScore = bet.Match.FirstTeamScore,
          SecondTeamScore = bet.Match.SecondTeamScore,
          ReferenceMatch = bet.Match.RefMatch,
          Id = bet.Match.Id,
          Sport = new SportDTO
          {
            Name = bet.Match.Sport.Name,
            Id = bet.Match.Sport.Id,
            ReferenceSport = bet.Match.Sport.RefSport,
            ExactPoints = bet.Match.Sport.ExactPoints,
            PartialPoints = bet.Match.Sport.PartialPoints,
            Tie = bet.Match.Sport.Tie,
          },
          FirstTeam = new TeamDTO
          {
            Id = bet.Match.FirstTeam.Id,
            ReferenceTeam = bet.Match.FirstTeam.RefTeam,
            Logo = bet.Match.FirstTeam?.Logo,
            Name = bet.Match.FirstTeam.Name,
            TeamType = bet.Match.FirstTeam.TeamType,
            Sport = new SportDTO
            {
              Name = bet.Match.FirstTeam.Sport.Name,
              Id = bet.Match.FirstTeam.Sport.Id,
              ReferenceSport = bet.Match.FirstTeam.Sport.RefSport,
              ExactPoints = bet.Match.FirstTeam.Sport.ExactPoints,
              PartialPoints = bet.Match.FirstTeam.Sport.PartialPoints,
              Tie = bet.Match.FirstTeam.Sport.Tie,
            }
          },
          SecondTeam = new TeamDTO
          {
            Id = bet.Match.SecondTeam.Id,
            ReferenceTeam = bet.Match.SecondTeam.RefTeam,
            Logo = bet.Match.SecondTeam?.Logo,
            Name = bet.Match.SecondTeam.Name,
            TeamType = bet.Match.SecondTeam.TeamType,
            Sport = new SportDTO
            {
              Name = bet.Match.SecondTeam.Sport.Name,
              Id = bet.Match.SecondTeam.Sport.Id,
              ReferenceSport = bet.Match.SecondTeam.Sport.RefSport,
              ExactPoints = bet.Match.SecondTeam.Sport.ExactPoints,
              PartialPoints = bet.Match.SecondTeam.Sport.PartialPoints,
              Tie = bet.Match.SecondTeam.Sport.Tie,
            }
          }
        },
        User = new UserDTO
        {
          Email = userEmail,
          Name = bet.User.Name,
        }
      };
    }

    public void DeleteBet(string userEmail, int matchId, int eventId)
    {
      var bet = _betDAL.Get(new List<Expression<Func<Bet, bool>>>
                    { x => x.Event_id == eventId && x.Match_id == matchId && x.User_email == userEmail })
                  .FirstOrDefault() ??
                throw new BetNotFoundException(
                  $"Bet not found with event_id: {eventId}, user_email: {userEmail}, match_id: {matchId}");

      _betDAL.Delete(bet);
      _betDAL.SaveChanges();
    }

    public List<BetDTO> GetBets(out int count, string? userEmail, int? matchId, int? eventId, int? page, int? pageSize)
    {
      SetPagination(page, pageSize);

      var conditions = new List<Expression<Func<Bet, bool>>>();

      if (userEmail != null) conditions.Add(x => x.User_email == userEmail);
      if (matchId != null) conditions.Add(x => x.Match_id == matchId);
      if (eventId != null) conditions.Add(x => x.Event_id == eventId);

      IQueryable<BetDTO> betsQuery = _betDAL.Get(conditions)
        .Select(x => new BetDTO
        {
          ScoreFirstTeam = x.ScoreFirstTeam,
          ScoreSecondTeam = x.ScoreSecondTeam,
          Points = x.Points,
          Event = new EventDTO
          {
            Finished = x.Event.Finished,
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
          Match = new MatchDTO
          {
            Finished = x.Match.Finished,
            Date = x.Match.Date,
            FirstTeamScore = x.Match.FirstTeamScore,
            SecondTeamScore = x.Match.SecondTeamScore,
            ReferenceMatch = x.Match.RefMatch,
            Id = x.Match.Id,
            Sport = new SportDTO
            {
              Name = x.Match.Sport.Name,
              Id = x.Match.Sport.Id,
              ReferenceSport = x.Match.Sport.RefSport,
              ExactPoints = x.Match.Sport.ExactPoints,
              PartialPoints = x.Match.Sport.PartialPoints,
              Tie = x.Match.Sport.Tie,
            },
            FirstTeam = new TeamDTO
            {
              Id = x.Match.FirstTeam.Id,
              ReferenceTeam = x.Match.FirstTeam.RefTeam,
              Logo = x.Match.FirstTeam.Logo,
              Name = x.Match.FirstTeam.Name,
              TeamType = x.Match.FirstTeam.TeamType,
              Sport = new SportDTO
              {
                Name = x.Match.FirstTeam.Sport.Name,
                Id = x.Match.FirstTeam.Sport.Id,
                ReferenceSport = x.Match.FirstTeam.Sport.RefSport,
                ExactPoints = x.Match.FirstTeam.Sport.ExactPoints,
                PartialPoints = x.Match.FirstTeam.Sport.PartialPoints,
                Tie = x.Match.FirstTeam.Sport.Tie,
              }
            },
            SecondTeam = new TeamDTO
            {
              Id = x.Match.SecondTeam.Id,
              ReferenceTeam = x.Match.SecondTeam.RefTeam,
              Logo = x.Match.SecondTeam.Logo,
              Name = x.Match.SecondTeam.Name,
              TeamType = x.Match.SecondTeam.TeamType,
              Sport = new SportDTO
              {
                Name = x.Match.SecondTeam.Sport.Name,
                Id = x.Match.SecondTeam.Sport.Id,
                ReferenceSport = x.Match.SecondTeam.Sport.RefSport,
                ExactPoints = x.Match.SecondTeam.Sport.ExactPoints,
                PartialPoints = x.Match.SecondTeam.Sport.PartialPoints,
                Tie = x.Match.SecondTeam.Sport.Tie,
              }
            }
          },
          User = new UserDTO
          {
            Email = userEmail,
            Name = x.User.Name,
          }
        });

      count = betsQuery.Count();

      return betsQuery.ToList();
    }

    public BetDTO ModifyBet(string userEmail, int matchId, int eventId, int? firstTeamScore, int? secondTeamScore)
    {
      var bet = _betDAL.Get(new List<Expression<Func<Bet, bool>>>
                    { x => x.Event_id == eventId && x.Match_id == matchId && x.User_email == userEmail })
                  .FirstOrDefault() ??
                throw new BetNotFoundException(
                  $"Bet not found with event_id: {eventId}, user_email: {userEmail}, match_id: {matchId}");

      bool changes = false;

      if (firstTeamScore != null && firstTeamScore != bet.ScoreFirstTeam)
      {
        bet.ScoreFirstTeam = firstTeamScore.Value;
        changes = changes || true;
      }

      if (secondTeamScore != null && secondTeamScore != bet.ScoreSecondTeam)
      {
        bet.ScoreSecondTeam = secondTeamScore.Value;
        changes = changes || true;
      }

      var matchTie = _matchDAL.Get(new List<Expression<Func<Match, bool>>> { x => x.Id == matchId })
        .Select(x => x.Sport.Tie).FirstOrDefault();

      if (!matchTie && firstTeamScore == secondTeamScore) throw new SportTieException();


      var match = _matchDAL.Get(new List<Expression<Func<Match, bool>>> { x => matchId == x.Id })
        .Select(x => new MatchDTO
        {
          Finished = x.Finished,
          Date = x.Date,
          FirstTeamScore = x.FirstTeamScore,
          SecondTeamScore = x.SecondTeamScore,
          ReferenceMatch = x.RefMatch,
          Id = x.Id,
          Sport = new SportDTO
          {
            Name = x.Sport.Name,
            Id = x.Sport.Id,
            ReferenceSport = x.Sport.RefSport,
            ExactPoints = x.Sport.ExactPoints,
            PartialPoints = x.Sport.PartialPoints,
            Tie = x.Sport.Tie,
          },
          FirstTeam = new TeamDTO
          {
            Id = x.FirstTeam.Id,
            ReferenceTeam = x.FirstTeam.RefTeam,
            Logo = x.FirstTeam.Logo,
            Name = x.FirstTeam.Name,
            TeamType = x.FirstTeam.TeamType,
            Sport = new SportDTO
            {
              Name = x.FirstTeam.Sport.Name,
              Id = x.FirstTeam.Sport.Id,
              ReferenceSport = x.FirstTeam.Sport.RefSport,
              ExactPoints = x.FirstTeam.Sport.ExactPoints,
              PartialPoints = x.FirstTeam.Sport.PartialPoints,
              Tie = x.FirstTeam.Sport.Tie,
            }
          },
          SecondTeam = new TeamDTO
          {
            Id = x.SecondTeam.Id,
            ReferenceTeam = x.SecondTeam.RefTeam,
            Logo = x.SecondTeam.Logo,
            Name = x.SecondTeam.Name,
            TeamType = x.SecondTeam.TeamType,
            Sport = new SportDTO
            {
              Name = x.SecondTeam.Sport.Name,
              Id = x.SecondTeam.Sport.Id,
              ReferenceSport = x.SecondTeam.Sport.RefSport,
              ExactPoints = x.SecondTeam.Sport.ExactPoints,
              PartialPoints = x.SecondTeam.Sport.PartialPoints,
              Tie = x.SecondTeam.Sport.Tie,
            }
          }
        })
        .FirstOrDefault();

      if (match.Date < DateTime.Now) throw new MatchAlreadyStartedException($"The match {matchId} has already started");

      if (changes)
      {
        _betDAL.Update(bet);
        _betDAL.SaveChanges();
      }

      var @event = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => eventId == x.Id })
        .Select(x => new EventDTO
        {
          Finished = x.Finished,
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

      return new BetDTO
      {
        ScoreFirstTeam = bet.ScoreFirstTeam,
        ScoreSecondTeam = bet.ScoreSecondTeam,
        Points = bet.Points,
        Event = @event,
        Match = match,
        User = user
      };
    }

    public void UpdatePoints(string userEmail, int matchId, int eventId)
    {
      var @event = _eventDAL.Get(new List<Expression<Func<Event, bool>>> { x => x.Id == eventId }).Any();
      if (!@event) throw new EventNotFoundException($"Event not found with id {eventId}");

      var user = _userDAL.Get(new List<Expression<Func<User, bool>>> { x => x.Email == userEmail }).Any();
      if (!user) throw new UserNotFoundException($"User not found with email {userEmail}");

      var match = _matchDAL.Get(new List<Expression<Func<Match, bool>>>
          { x => x.Id == matchId && x.Event_id == eventId })
        .Select(x => new
        {
          FirstTeamScore = x.FirstTeamScore,
          SecondTeamScore = x.SecondTeamScore,
          PartialPoints = x.Sport.PartialPoints,
          ExactPoints = x.Sport.ExactPoints,
          Date = x.Date,
        })
        .FirstOrDefault() ?? throw new MatchNotFoundException($"Match not found with if {matchId} for event {eventId}");

      if (match.Date >= DateTime.Now) throw new MatchDoesntStartException($"The match {matchId} doesn't start");

      var bet = _betDAL.Get(new List<Expression<Func<Bet, bool>>>
                    { x => x.Event_id == eventId && x.Match_id == matchId && x.User_email == userEmail })
                  .FirstOrDefault() ??
                throw new BetNotFoundException(
                  $"Bet not found with event_id: {eventId}, user_email: {userEmail}, match_id: {matchId}");

      CalculatePoints(ref bet, match.FirstTeamScore.Value, match.SecondTeamScore.Value, match.PartialPoints.Value,
        match.ExactPoints.Value);

      _betDAL.Update(bet);
      _betDAL.SaveChanges();
    }

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }

    private void CalculatePoints(ref Bet bet, int firstTeamScore, int secondTeamScore, int partialPoints,
      int exactPoints)
    {
      int points = 0;

      if (bet.ScoreFirstTeam == firstTeamScore && secondTeamScore == bet.ScoreSecondTeam)
      {
        points = exactPoints;
      }
      else if (
        (firstTeamScore > secondTeamScore && bet.ScoreFirstTeam > bet.ScoreSecondTeam) ||
        (firstTeamScore < secondTeamScore && bet.ScoreFirstTeam < bet.ScoreSecondTeam) ||
        (firstTeamScore == secondTeamScore && bet.ScoreFirstTeam == bet.ScoreSecondTeam))
      {
        points = partialPoints;
      }

      bet.Points = points;
    }
  }
}
