
namespace simplified_picpay.Exceptions.TransactionExceptions
{
    public class StorekeeperTransectionNotAllowedException(string error) : DomainException(error);
}