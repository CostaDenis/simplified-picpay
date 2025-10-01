using simplified_picpay.Models;

namespace simplified_picpay.Repositories.Abstractions
{
    public interface IAccountRepository
    {
        Task<Account?> LoginAsync(string email, CancellationToken cancellationToken = default);
        Task<Account?> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Account?> SearchAccountByDisplayNameAsync(string displayName, CancellationToken cancellationToken = default);
        Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default);
        Account Update(Account account);
        Account UpdateFounds(Account account);
        Task<Account> DisableAccountAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Account> EnableAccountAsync(Guid id, CancellationToken cancellationToken = default);
    }
}