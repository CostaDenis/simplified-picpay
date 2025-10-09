using simplified_picpay.DTOs.AccountDTOs;
using simplified_picpay.Models;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Services.Abstractions
{
    public interface IAccountService
    {
        public Task<LoggedAccountViewModel> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken = default);
        public Task<AccountSummaryViewModel> SearchAccountByDisplayNameAsync(SearchDisplayNameDTO searchDisplayNameDTO, CancellationToken cancellationToken = default);
        public Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default);
        public Task<Account> Update(UpdateAccountDTO updateAccountDTO);
        public Task<Account> AddFounds(Guid id, decimal amount, CancellationToken cancellationToken = default);
        public Task<Account> RemoveFounds(Guid id, decimal amount, CancellationToken cancellationToken = default);
        public Task<string> DisableAccountAsync(Guid id, CancellationToken cancellationToken = default);
        public bool VerifyDocument(Account account);
        public bool VerifyAccountType(Account account);
        public string PasswordHasher(Account account, string password);
        public bool CheckPassword(Account account, string hashedPassword, string providerPassword);
        public Task<bool> EnableAccountAsync(Guid id, CancellationToken cancellationToken = default);
    }
}