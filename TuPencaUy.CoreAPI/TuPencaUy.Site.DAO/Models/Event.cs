﻿using TuPencaUy.Core.DAO;

namespace TuPencaUy.Site.DAO.Models
{
  public class Event : BaseEvent
  {
    public int Price { get; set; }
    public decimal PrizePercentage { get; set; }
    public virtual ICollection<Sport>? Sports { get; set; }
    public virtual ICollection<Match>? Matches { get; set; }
    public virtual ICollection<User>? Users { get; set; }
    public virtual ICollection<Bet>? Bets { get; set; }
    public virtual ICollection<Payment>? Payments { get; set; }
    public virtual int RefEvent { get; set; }
  }
}
