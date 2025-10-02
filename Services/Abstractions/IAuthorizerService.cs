using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Services.Abstractions
{
    public interface IAuthorizerService
    {
        public Task<bool> IsAuthorizedAsync(CancellationToken cancellationToken);
    }
}