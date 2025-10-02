

namespace simplified_picpay.Services.Abstractions
{
    public interface ITokenService
    {
        Guid GetAccounId(HttpContext http);
        string GenerateTokenJwt(Guid id, string email, string accountType);
    }
}