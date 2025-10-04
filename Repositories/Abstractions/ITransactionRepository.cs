using simplified_picpay.Models;

namespace simplified_picpay.Repositories.Abstractions
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> AllYourTransactionsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> AllYourTransactionsReceivedAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> AllYourTransactionsPaiedAsync(Guid id, CancellationToken cancellationToken = default);
    }
}