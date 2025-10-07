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

        public async Task<Transaction?> GetTransactionAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Transaction>?> GetAllYourTransactionsAsync(Guid id, int skip, int take, CancellationToken cancellationToken = default)
            => await _context.Transactions.Where(x => x.PayeeId == id || x.PayerId == id).Skip(skip).Take(take).AsNoTracking().ToListAsync(cancellationToken);

        public async Task<List<Transaction>?> GetAllReceivedTransactionsAsync(Guid id, int skip, int take, CancellationToken cancellationToken = default)
            => await _context.Transactions.Where(x => x.PayeeId == id).AsNoTracking().Skip(skip).Take(take).ToListAsync(cancellationToken);

        public async Task<List<Transaction>?> GetAllPaidTransactionsAsync(Guid id, int skip, int take, CancellationToken cancellationToken = default)
            => await _context.Transactions.Where(x => x.PayerId == id).AsNoTracking().Skip(skip).Take(take).ToListAsync(cancellationToken);

    }
}