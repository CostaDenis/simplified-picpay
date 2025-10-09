
namespace simplified_picpay.Exceptions.TransactionExceptions
{
    public class PayeeAccountInactiveException(string error) : DomainException(error);
}