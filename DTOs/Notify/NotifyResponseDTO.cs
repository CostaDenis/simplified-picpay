using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.DTOs.Notify
{
    public class NotifyResponseDTO
    {
        public string Status { get; set; } = string.Empty;
        public NotifyDataDTO Data { get; set; } = new();
    }
}