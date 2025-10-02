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
                                        IAccountRepository accountRepository) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IAccountService _accountService = accountService;
        private readonly IAuthorizerService _authorizerService = authorizerService;
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<(bool success, string? error, Transaction? data)> CreateTransactionAsync(CreateTransactionDTO createTransactionDTO, CancellationToken cancellationToken)
        {
            if (!await _authorizerService.IsAuthorizedAsync(cancellationToken))
                return (false, "Transação não autorizada pelo serviço externo!", null);

            var payer = await _accountRepository.GetAccountByIdAsync(createTransactionDTO.PayerId, cancellationToken);
            var payee = await _accountRepository.GetAccountByPublicIdAsync(createTransactionDTO.PayeePublicId, cancellationToken);

            if (payer == payee)
                return (false, "A conta a se tranferir não pode ser a mesma que envia a tranferência!", null);

            if (payee == null)
                return (false, "Conta recebinte não encontrada!", null);

            if (payer!.AccountType == EAccountType.Storekeeper.ToString().ToLower())
                return (false, "Lojistas não são permitidos a fazer transferências, apenas podem receber!", null);

            if (!_accountService.AddFounds(payee, createTransactionDTO.Value).success)
                return (false, "Erro ao adicionar fundos!", null);

            if (!_accountService.RemoveFounds(payer, -createTransactionDTO.Value).success)
                return (false, "Erro ao remover fundos!", null);

            var transaction = new Transaction
            {
                PayerId = createTransactionDTO.PayerId,
                PayerPublicId = createTransactionDTO.PayerPublicId,
                PayeeId = payee.Id,
                PayeePublicId = createTransactionDTO.PayeePublicId,
                Value = createTransactionDTO.Value
            };

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