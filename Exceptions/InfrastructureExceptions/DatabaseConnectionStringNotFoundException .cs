
namespace simplified_picpay.Exceptions.InfrastructureExceptions
{
    public class DatabaseConnectionStringNotFoundException(string error) : DomainException(error);
}