using simplified_picpay.DTOs.Transaction;
using simplified_picpay.Models;

namespace simplified_picpay.Services.Abstractions
{
    public interface ITransactionService
    {
        public Task<(bool success, string? error, Transaction? data)> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, List<Transaction>? data)> AllYourTransactionsAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, List<Transaction>? data)> AllYourTransactionsReceivedAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, List<Transaction>? data)> AllYourTransactionsPaiedAsync(Guid id, CancellationToken cancellationToken = default);
    }
}