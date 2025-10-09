
namespace simplified_picpay.Exceptions.InfrastructureExceptions
{
    public class JwtKeyNotFoundException(string error) : DomainException(error);
}