using simplified_picpay.Models;

namespace simplified_picpay.Repositories.Abstractions
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default);
        Task<Transaction?> GetTransactionAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> GetAllYourTransactionsAsync
            (Guid id, int skip, int take, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> GetAllReceivedTransactionsAsync
            (Guid id, int skip, int take, CancellationToken cancellationToken = default);
        Task<List<Transaction>?> GetAllPaidTransactionsAsync
            (Guid id, int skip, int take, CancellationToken cancellationToken = default);
    }
}