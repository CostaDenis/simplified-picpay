using Microsoft.EntityFrameworkCore.Storage;
using simplified_picpay.Data;
using simplified_picpay.DTOs.TransactionDTOs;
using simplified_picpay.Enums;
using simplified_picpay.Exceptions;
using simplified_picpay.Exceptions.TransactionExceptions;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Services
{
    public class TransactionService(ITransactionRepository transactionRepository,
                                        IAuthorizerService authorizerService,
                                        INotifyService notifyService,
                                        IAccountRepository accountRepository,
                                        AppDbContext appDbContext) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IAuthorizerService _authorizerService = authorizerService;
        private readonly INotifyService _notifyService = notifyService;
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<Transaction> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO, CancellationToken cancellationToken = default)
        {
            if (!await _authorizerService.IsAuthorizedAsync(cancellationToken))
                throw new TransactionNotAllowedException("Transação não autorizada pelo serviço externo!");

            var payer = await _accountRepository.GetAccountByIdAsync(createTransactionDTO.PayerId, cancellationToken);
            var payerPublicId = await _accountRepository.GetPublicIdAsync(payer!.Id);
            var payee = await _accountRepository.GetAccountByPublicIdAsync(createTransactionDTO.PayeePublicId, cancellationToken);

            if (payer == payee)
                throw new SelfTransectionNotAllowedException("A conta a se tranferir não pode ser a mesma que envia a tranferência!");

            if (payee == null)
                throw new PayeeNotFoundException("Conta recebedora não encontrada!");

            if (payer!.AccountType == EAccountType.Storekeeper.ToString().ToLower())
                throw new StorekeeperTransectionNotAllowedException("Lojistas não são permitidos a fazer transferências, apenas podem receber!");

            if (payee!.IsActive == false)
                throw new PayeeAccountInactiveException("A conta destinada está desativada!");


            await using var transactionDatabase = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var value = createTransactionDTO.Value;

                if (value <= 0)
                    throw new ZeroTransactionValueNotAllowedException("O valor da transação deve ser maior que zero!");

                if (payer.CurrentBalance < value)
                    throw new InsufficientFundsException("Saldo insuficiente!");

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

                return transaction;
            }
            catch
            {
                if (transactionDatabase.GetDbTransaction().Connection != null)
                    await transactionDatabase.RollbackAsync(cancellationToken);

                throw new DomainException("Erro interno ao realizar transação!");
            }
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid transactionId, Guid accountId, CancellationToken cancellationToken = default)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransactionAsync(transactionId, cancellationToken);

                if (transaction != null &&
                    transaction.PayerId != accountId && transaction.PayeeId != accountId)
                    throw new ForbiddenTransactionAccessException("Não é possível consultar transações de terceiros!");

                return transaction;
            }
            catch
            {
                throw new DomainException("Erro interno ao consultar a transação!");
            }
        }

        public async Task<List<Transaction>?> GetAllYourTransactionsAsync(Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionRepository.GetAllYourTransactionsAsync
                    (id, paginationTransactionDTO.Skip, paginationTransactionDTO.Take, cancellationToken);

                return transactions;
            }
            catch
            {
                throw new DomainException("Erro interno ao consultar as transações de sua conta!");
            }
        }

        public async Task<List<Transaction>?> GetAllReceivedTransactionsAsync(Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionRepository.GetAllReceivedTransactionsAsync
                    (id, paginationTransactionDTO.Skip, paginationTransactionDTO.Take, cancellationToken);

                return transactions;
            }
            catch
            {
                throw new DomainException("Erro interno ao consultar as transações de sua conta!");
            }
        }

        public async Task<List<Transaction>?> GetAllPaidTransactionsAsync(Guid id, PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionRepository.GetAllPaidTransactionsAsync
                    (id, paginationTransactionDTO.Skip, paginationTransactionDTO.Take, cancellationToken);

                return transactions;
            }
            catch
            {
                throw new DomainException("Erro interno ao consultar as transações de sua conta!");
            }
        }

    }
}