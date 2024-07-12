namespace TuPencaUy.Core.Exceptions
{
  public class PaymentAlreadyExists : BadRequestException
  {
    public PaymentAlreadyExists(string message) : base(message) { }
    public PaymentAlreadyExists() : base(message: "Payment already exists") { }
  }
}
