using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuPencaUy.DTOs;

namespace TuPencaUy.Core.DTOs
{
  public class EventPaymentDTO
  {
    public UserDTO Winner { get; set; }
    public double PrizeAmount { get; set; }
    public string? SitePaypalEmail { get; set; }
    public double SiteRevenueAmount {  get; set; }
  }
}
