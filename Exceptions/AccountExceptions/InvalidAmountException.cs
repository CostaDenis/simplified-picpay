
namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class InvalidAmountException(string error) : DomainException(error);
}