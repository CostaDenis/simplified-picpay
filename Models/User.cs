using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = default!;
        public string CPF { get; set; } = string.Empty;
    }
}