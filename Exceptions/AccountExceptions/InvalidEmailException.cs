
namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class InvalidEmailException(string error) : DomainException(error);
}