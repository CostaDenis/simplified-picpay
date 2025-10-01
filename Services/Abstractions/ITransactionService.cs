using simplified_picpay.DTOs.Transaction;
using simplified_picpay.Models;

namespace simplified_picpay.Services.Abstractions
{
    public interface ITransactionService
    {
        public Task<(bool success, string? error, Transaction? data)> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO, CancellationToken cancellationToken);
    }
}