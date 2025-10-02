using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Models
{
    public class AutorizeResponse
    {
        public string Status { get; set; } = string.Empty;
        public AuthorizeData Data { get; set; } = new();
    }
}