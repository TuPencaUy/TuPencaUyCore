﻿using TuPencaUy.Core.DAO;

namespace TuPencaUy.Site.DAO.Models
{
  public class Match : BaseMatch
  {
    public virtual required Sport Sport { get; set; }
    public virtual required Team FirstTeam { get; set; }
    public virtual required Team SecondTeam { get; set; }
  }
}
