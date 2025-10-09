
namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class AccountNotFoundException(string error) : DomainException(error);
}