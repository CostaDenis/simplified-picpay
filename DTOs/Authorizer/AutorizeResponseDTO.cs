using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Dtos.Authorizer
{
    public class AutorizeResponseDTO
    {
        public string Status { get; set; } = string.Empty;
        public AuthorizeDataDTO Data { get; set; } = new();
    }
}