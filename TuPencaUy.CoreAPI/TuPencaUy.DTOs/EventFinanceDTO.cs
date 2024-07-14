using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuPencaUy.Core.DTOs
{
  public class EventFinanceDTO
  {
    public int EventId {  get; set; }
    public string EventName { get; set; }
    public decimal TotalRaised { get; set; }
  }
}
