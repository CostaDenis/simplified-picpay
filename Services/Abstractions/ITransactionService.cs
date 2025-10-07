using simplified_picpay.DTOs.Transaction;
using simplified_picpay.Models;

namespace simplified_picpay.Services.Abstractions
{
    public interface ITransactionService
    {
        public Task<(bool success, string? error, Transaction? data)> CreateTransactionAsync
            (CreateTransactionDTO createTransactionDTO, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, Transaction? data)> GetTransactionByIdAsync
            (Guid transactionId, Guid accountId, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, List<Transaction>? data)> GetAllYourTransactionsAsync
            (Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, List<Transaction>? data)> GetAllReceivedTransactionsAsync
            (Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default);
        public Task<(bool success, string? error, List<Transaction>? data)> GetAllPaidTransactionsAsync
            (Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default);
    }
}