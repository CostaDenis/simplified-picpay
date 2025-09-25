using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PayerId { get; set; }
        public Account Payer { get; set; } = default!;
        public Guid PayeeId { get; set; }
        public Account Payee { get; set; } = default!;
        public decimal Value { get; set; }
    }
}