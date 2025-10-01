using simplified_picpay.Enums;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Services
{
    public class TransactionService(ITransactionRepository transactionRepository,
                                        IAccountRepository accountRepository) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IAccountRepository _accountRepository = accountRepository;
        public async Task<(bool success, string? error, Transaction? data)> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            var payer = await _accountRepository.GetAccountByIdAsync(transaction.PayeeId, cancellationToken);

            if (payer!.AccountType == EAccountType.Storekeeper.ToString())
                return (false, "Lojistas não são permitidos fazerem transferências, apenas podem receber!", null);

            try
            {
                await _transactionRepository.CreateTransactionAsync(transaction, cancellationToken);
                return (true, null, transaction);
            }
            catch
            {
                return (false, "Erro interno ao fazer a transação", null);
            }
        }
    }
}