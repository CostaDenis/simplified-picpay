using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simplified_picpay.Models;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Services
{
    public class AuthorizerService(HttpClient httpClient) : IAuthorizerService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<bool> IsAuthorizedAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("https://util.devi.tools/api/v2/authorize", cancellationToken);

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadFromJsonAsync<AutorizeResponse>(cancellationToken);

            return content is not null &&
                content.Status.Equals("success", StringComparison.OrdinalIgnoreCase) &&
                content.Data.Authorization;
        }
    }
}