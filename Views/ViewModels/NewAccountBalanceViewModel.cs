using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Views.ViewModels
{
    public class NewAccountBalanceViewModel
    {
        public Guid Id { get; set; }
        public string PublicId { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}