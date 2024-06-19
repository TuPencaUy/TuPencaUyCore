using System.Linq.Expressions;
using TuPencaUy.Core.DataAccessLogic;
using TuPencaUy.Core.DTOs;
using TuPencaUy.DTOs;
using TuPencaUy.Site.DAO.Models;

namespace TuPencaUy.Core.DataServices.Services.Tenant
{
  public class SiteBetService : IBetService
  {
    private readonly IGenericRepository<Bet> _betDAL;

    private int _page = 1;
    private int _pageSize = 10;

    public SiteBetService(IGenericRepository<Bet> betDAL) => _betDAL = betDAL;
    public BetDTO CreateBet(string userEmail, int matchId, int eventId, int firstTeamScore, int secondTeamScore)
    {
      throw new NotImplementedException();
    }

    public BetDTO DeleteBet(string userEmail, int matchId, int eventId)
    {
      throw new NotImplementedException();
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
              Logo = x.Match.FirstTeam?.Logo,
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
              Logo = x.Match.SecondTeam?.Logo,
              Name = x.Match.SecondTeam.Name,
              TeamType = x.Match.SecondTeam.TeamType,
              Sport = new SportDTO
              {
                Name = x.Match.SecondTeam.Sport.Name,
                Id = x.Match.SecondTeam.Sport.Id,
                ReferenceSport = x.Match.SecondTeam.Sport.RefSport,
                ExactPoints = x.Match.FirsSecondTeamtTeam.Sport.ExactPoints,
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

    public BetDTO ModifyBet(string? userEmail, int? matchId, int? eventId, int? firstTeamScore, int? secondTeamScore)
    {
      throw new NotImplementedException();
    }

    private void SetPagination(int? page, int? pageSize)
    {
      _page = page != null && page.Value > 0 ? page.Value : _page;
      _pageSize = pageSize != null && pageSize.Value > 0 ? pageSize.Value : _pageSize;
    }
  }
}
