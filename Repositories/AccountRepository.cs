using Microsoft.EntityFrameworkCore;
using simplified_picpay.Data;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;

namespace simplified_picpay.Repositories
{
    public class AccountRepository(AppDbContext context) : IAccountRepository
    {
        private readonly AppDbContext _context = context;


        public async Task<Account?> LoginAsync(string email, CancellationToken cancellationToken = default)
            => await _context.Accounts.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        public async Task<Account?> GetAccountByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default)
        {
            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return account;
        }

        public Account Update(Account account)
        {
            _context.Accounts.Update(account);
            _context.SaveChanges();

            return account;
        }

        public Account UpdateFounds(Account account)
        {
            _context.Accounts.Update(account);
            _context.SaveChanges();

            return account;
        }

        public async Task DisableAccountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var account = await GetAccountByIdAsync(id);
            account!.IsActive = false;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task EnableAccountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var account = await GetAccountByIdAsync(id);
            account!.IsActive = true;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}