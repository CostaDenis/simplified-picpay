
namespace simplified_picpay.Exceptions.TransactionExceptions
{
    public class ForbiddenTransactionAccessException(string error) : DomainException(error);
}