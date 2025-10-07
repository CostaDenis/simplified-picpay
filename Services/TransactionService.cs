using Microsoft.EntityFrameworkCore.Storage;
using simplified_picpay.Data;
using simplified_picpay.DTOs.Transaction;
using simplified_picpay.Enums;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Services
{
    public class TransactionService(ITransactionRepository transactionRepository,
                                        IAccountService accountService,
                                        IAuthorizerService authorizerService,
                                        INotifyService notifyService,
                                        IAccountRepository accountRepository,
                                        AppDbContext appDbContext) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IAccountService _accountService = accountService;
        private readonly IAuthorizerService _authorizerService = authorizerService;
        private readonly INotifyService _notifyService = notifyService;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<(bool success, string? error, Transaction? data)> CreateTransactionAsync
            (CreateTransactionDTO createTransactionDTO, CancellationToken cancellationToken = default)
        {
            if (!await _authorizerService.IsAuthorizedAsync(cancellationToken))
                return (false, "Transação não autorizada pelo serviço externo!", null);

            var payer = await _accountRepository.GetAccountByIdAsync(createTransactionDTO.PayerId, cancellationToken);
            var payerPublicId = await _accountRepository.GetPublicIdAsync(payer!.Id);
            var payee = await _accountRepository.GetAccountByPublicIdAsync(createTransactionDTO.PayeePublicId, cancellationToken);

            if (payer == payee)
                return (false, "A conta a se tranferir não pode ser a mesma que envia a tranferência!", null);

            if (payee == null)
                return (false, "Conta recebinte não encontrada!", null);

            if (payer!.AccountType == EAccountType.Storekeeper.ToString().ToLower())
                return (false, "Lojistas não são permitidos a fazer transferências, apenas podem receber!", null);

            await using var transactionDatabase = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var value = createTransactionDTO.Value;

                if (value <= 0)
                    return (false, "Valor inválido para transferência.", null);

                if (payer.CurrentBalance < value)
                    return (false, "Saldo insuficiente.", null);

                payer.CurrentBalance -= value;
                payee.CurrentBalance += value;

                _appDbContext.Accounts.Update(payee);
                _appDbContext.Accounts.Update(payer);

                var transaction = new Transaction
                {
                    PayerId = createTransactionDTO.PayerId,
                    PayerPublicId = payerPublicId,
                    PayeeId = payee.Id,
                    PayeePublicId = createTransactionDTO.PayeePublicId,
                    Value = createTransactionDTO.Value
                };

                await _transactionRepository.CreateTransactionAsync(transaction, cancellationToken);
                await _appDbContext.SaveChangesAsync(cancellationToken);
                await transactionDatabase.CommitAsync(cancellationToken);

                _ = _notifyService.SendNotificationAsync(
                    email: payee.Email,
                    message: $"Você recebeu {value} de {payer.DisplayName}!",
                    cancellationToken
                    );

                return (true, null, transaction);
            }
            catch
            {
                if (transactionDatabase.GetDbTransaction().Connection != null)
                    await transactionDatabase.RollbackAsync(cancellationToken);

                return (false, "Erro interno ao fazer a transação", null);
            }
        }

        public async Task<(bool success, string? error, Transaction? data)> GetTransactionByIdAsync
            (Guid transactionId, Guid accountId, CancellationToken cancellationToken = default)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionAsync(transactionId, cancellationToken);

                if (transaction == null)
                    return (false, "Transação não encontrada!", null);

                if (transaction.PayerId != accountId && transaction.PayeeId != accountId)
                    return (false, "Não é possível consultar transações de terceiros!", null);

                return (true, null, transaction);
            }
            catch
            {
                return (false, "Erro interno ao consultar a transação!", null);
            }
        }

        public async Task<(bool success, string? error, List<Transaction>? data)> GetAllYourTransactionsAsync
            (Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionRepository.GetAllYourTransactionsAsync
                    (id, paginationTransactionDTO.Skip, paginationTransactionDTO.Take, cancellationToken);


                if (transactions == null)
                    return (true, null, data: transactions);

                return (true, null, transactions);
            }
            catch
            {
                return (false, "Erro interno ao consultar as transações de sua conta!", null);
            }
        }

        public async Task<(bool success, string? error, List<Transaction>? data)> GetAllReceivedTransactionsAsync
            (Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionRepository.GetAllReceivedTransactionsAsync
                    (id, paginationTransactionDTO.Skip, paginationTransactionDTO.Take, cancellationToken);


                if (transactions == null)
                    return (true, null, data: transactions);

                return (true, null, transactions);
            }
            catch
            {
                return (false, "Erro interno ao consultar as transações recebidas de sua conta!", null);
            }
        }

        public async Task<(bool success, string? error, List<Transaction>? data)> GetAllPaidTransactionsAsync
            (Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionRepository.GetAllPaidTransactionsAsync
                    (id, paginationTransactionDTO.Skip, paginationTransactionDTO.Take, cancellationToken);

                if (transactions == null)
                    return (true, null, data: transactions);

                return (true, null, transactions);
            }
            catch
            {
                return (false, "Erro interno ao consultar as transações feitas de sua conta!", null);
            }
        }

    }
}