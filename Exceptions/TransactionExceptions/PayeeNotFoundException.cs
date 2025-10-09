
namespace simplified_picpay.Exceptions.TransactionExceptions
{
    public class PayeeNotFoundException(string error) : DomainException(error);
}