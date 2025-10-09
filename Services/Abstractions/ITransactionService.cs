using simplified_picpay.DTOs.TransactionDTOs;
using simplified_picpay.Models;

namespace simplified_picpay.Services.Abstractions
{
    public interface ITransactionService
    {
        public Task<Transaction> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO,
            CancellationToken cancellationToken = default);
        public Task<Transaction?> GetTransactionByIdAsync(Guid transactionId, Guid accountId, CancellationToken cancellationToken = default);
        public Task<List<Transaction>?> GetAllYourTransactionsAsync(Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default);
        public Task<List<Transaction>?> GetAllReceivedTransactionsAsync(Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default);
        public Task<List<Transaction>?> GetAllPaidTransactionsAsync(Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default);
    }
}