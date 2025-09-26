using simplified_picpay.Models;

namespace simplified_picpay.Repositories.Abstractions
{
    public interface IAccountRepository
    {
        Task<Account?> LoginAsync(string email, CancellationToken cancellationToken = default);
        Task<Account?> GetAccountByIdAsync(Guid id);
        Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default);
        Account Update(Account account);
        Task DisableAccountAsync(Guid id, CancellationToken cancellationToken = default);
        Task EnableAccountAsync(Guid id, CancellationToken cancellationToken = default);
    }
}