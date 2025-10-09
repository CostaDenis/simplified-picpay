
namespace simplified_picpay.Exceptions.TransactionExceptions
{
    public class ZeroTransactionValueNotAllowedException(string error) : DomainException(error);
}