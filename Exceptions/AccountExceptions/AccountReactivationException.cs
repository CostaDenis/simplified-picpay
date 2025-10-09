
namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class AccountReactivationException(string error) : DomainException(error);
}