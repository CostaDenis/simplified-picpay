using simplified_picpay.Models;

namespace simplified_picpay.Services.Abstractions
{
    public interface ITransactionService
    {
        public Task<(bool success, string? error, Transaction? data)> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
    }
}