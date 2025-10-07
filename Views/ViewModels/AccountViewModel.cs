using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Views.ViewModels
{
    public class AccountViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = default!;
        public string PublicId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
    }
}