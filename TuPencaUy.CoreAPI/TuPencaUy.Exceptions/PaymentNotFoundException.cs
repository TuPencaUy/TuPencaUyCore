namespace TuPencaUy.Core.Exceptions
{
  public class PaymentNotFoundException : NotFoundException
  {
    public PaymentNotFoundException(string message) : base(message) { }
    public PaymentNotFoundException() : base(message: "Payment not foundd") { }
  }
}
