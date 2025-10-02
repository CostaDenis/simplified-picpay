

namespace simplified_picpay.Services.Abstractions
{
    public interface IAuthorizerService
    {
        public Task<bool> IsAuthorizedAsync(CancellationToken cancellationToken);
    }
}