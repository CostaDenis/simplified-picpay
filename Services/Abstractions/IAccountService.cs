using simplified_picpay.DTOs;
using simplified_picpay.Models;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Services.Abstractions
{
    public interface IAccountService
    {
        public Task<(bool success, string? error, LoggedAccountViewModel? data)> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, AccountSummaryViewModel? data)> SearchAccountByDisplayNameAsync(SearchDisplayNameDTO searchDisplayNameDTO, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, Account? data)> CreateAsync(Account account, CancellationToken cancellationToken = default);
        public (bool success, string? error, Account? data) Update(Account account);
        public (bool success, string? error, Account? data) AddFounds(Account account, decimal amount);
        public (bool success, string? error, Account? data) RemoveFounds(Account account, decimal amount);
        public Task<(bool success, string? error, string? data)> DisableAccountAsync(Guid id, CancellationToken cancellationToken = default);
        public bool VerifyDocument(Account account);
        public bool VerifyAccountType(Account account);
        public string PasswordHasher(Account account, string password);
        public bool CheckPassword(Account account, string hashedPassword, string providerPassword);
        public Task<bool> EnableAccount(Guid id, CancellationToken cancellationToken = default);
    }
}