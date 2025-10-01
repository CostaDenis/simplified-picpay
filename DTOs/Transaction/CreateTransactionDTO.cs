using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.DTOs.Transaction
{
    public class CreateTransactionDTO
    {
        public Guid PayerId { get; set; }
        public string PayerPublicId { get; set; } = string.Empty;
        // public Guid PayeeId { get; set; }
        public string PayeePublicId { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }
}