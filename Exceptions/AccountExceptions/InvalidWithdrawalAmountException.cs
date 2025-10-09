
namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class InvalidWithdrawalAmountException(string error) : DomainException(error);
}