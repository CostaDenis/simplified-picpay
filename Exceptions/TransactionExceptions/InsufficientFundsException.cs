
namespace simplified_picpay.Exceptions.TransactionExceptions
{
    public class InsufficientFundsException(string error) : DomainException(error);
}