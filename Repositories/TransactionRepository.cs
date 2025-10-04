using Microsoft.EntityFrameworkCore;
using simplified_picpay.Data;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;

namespace simplified_picpay.Repositories
{
    public class TransactionRepository(AppDbContext context) : ITransactionRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<List<Transaction>?> AllYourTransactionsAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.Transactions.Where(x => x.PayeeId == id || x.PayerId == id).AsNoTracking().ToListAsync(cancellationToken);

        public async Task<List<Transaction>?> AllYourTransactionsReceivedAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.Transactions.Where(x => x.PayeeId == id).AsNoTracking().ToListAsync(cancellationToken);

        public async Task<List<Transaction>?> AllYourTransactionsPaiedAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.Transactions.Where(x => x.PayerId == id).AsNoTracking().ToListAsync(cancellationToken);
    }
}