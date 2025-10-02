

namespace simplified_picpay.Services.Abstractions
{
    public interface INotifyService
    {
        Task<bool> SendNotificationAsync(string email, string message, CancellationToken cancellationToken = default);
    }
}