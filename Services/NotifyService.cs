using simplified_picpay.DTOs.NotifyDTOs;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Services
{
    public class NotifyService(HttpClient httpClient) : INotifyService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<bool> SendNotificationAsync(string email, string message, CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                email,
                message
            };

            var response = await _httpClient.PostAsJsonAsync("https://util.devi.tools/api/v1/notify", payload, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadFromJsonAsync<NotifyResponseDTO>(cancellationToken);

            return content?.Status?.ToLower() == "success";
        }
    }
}