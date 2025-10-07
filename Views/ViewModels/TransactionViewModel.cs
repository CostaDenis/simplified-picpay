using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Views.ViewModels
{
    public class TransactionViewModel
    {
        public Guid Id { get; set; }
        public string PayerPublicId { get; set; } = string.Empty;
        public string PayeePublicId { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }
}