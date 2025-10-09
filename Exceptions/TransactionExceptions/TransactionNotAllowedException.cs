
namespace simplified_picpay.Exceptions.TransactionExceptions
{
    public class TransactionNotAllowedException(string error) : DomainException(error);
}