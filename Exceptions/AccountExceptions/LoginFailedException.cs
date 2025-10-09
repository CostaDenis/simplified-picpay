
namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class LoginFailedException(string error) : DomainException(error);
}