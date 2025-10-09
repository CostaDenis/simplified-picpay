
namespace simplified_picpay.Exceptions.TransactionExceptions
{
    public class SelfTransectionNotAllowedException(string error) : DomainException(error);
}