using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.DTOs
{
  public class SiteFinanceDTO
  {
    public int SiteId { get; set; }
    public string SiteName { get; set; }
    public decimal TotalRaised { get; set; }
  }
}
