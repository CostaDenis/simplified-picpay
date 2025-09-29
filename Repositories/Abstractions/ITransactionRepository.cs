using simplified_picpay.Models;

namespace simplified_picpay.Repositories.Abstractions
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    }
}