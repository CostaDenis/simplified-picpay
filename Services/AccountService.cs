using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using simplified_picpay.DTOs.AccountDTOs;
using simplified_picpay.Enums;
using simplified_picpay.Exceptions;
using simplified_picpay.Exceptions.AccountExceptions;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Services
{
    public class AccountService(IAccountRepository accountRepository,
                                    ITokenService tokenService) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ITokenService _tokenService = tokenService;


        public async Task<LoggedAccountViewModel> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.LoginAsync(loginDTO.Email, cancellationToken);

            if (account == null)
                throw new LoginFailedException("Acesso negado!");

            if (!CheckPassword(account, account.PasswordHash, loginDTO.Password))
                throw new LoginFailedException("Acesso negado!");

            if (!account.IsActive)
            {
                var enabled = await EnableAccountAsync(account.Id, cancellationToken);
                if (!enabled)
                    throw new AccountReactivationException("Erro ao reativar conta!");
            }

            var token = _tokenService.GenerateTokenJwt(account.Id, account.Email, account.AccountType);

            return new LoggedAccountViewModel
            {
                Id = account.Id,
                FullName = account.FullName,
                DisplayName = account.DisplayName,
                PublicId = account.PublicId,
                Email = account.Email,
                CurrentBalance = account.CurrentBalance,
                AccountType = account.AccountType,
                Document = account.Document,
                Token = token
            };

        }

        public async Task<AccountSummaryViewModel> SearchAccountByDisplayNameAsync(SearchDisplayNameDTO searchDisplayNameDTO, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.SearchAccountByDisplayNameAsync(searchDisplayNameDTO.DisplayName, cancellationToken);
            string accountIsActive = "Conta ativa";

            if (account == null)
                throw new AccountNotFoundException("Conta não encontrada!");

            if (!account.IsActive)
                accountIsActive = "Conta desativada";

            return new AccountSummaryViewModel
            {
                DisplayName = account.DisplayName,
                PublicId = account.PublicId,
                AccountType = account.AccountType,
                IsActive = accountIsActive
            };
        }

        public async Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default)
        {
            if (!VerifyAccountType(account))
                throw new InvalidAccountTypeException("Só está disponível conta de User e Storekeeper!");

            if (!VerifyDocument(account))
                throw new InvalidDocumentException("Documento inválido! Em caso de conta de usuário," +
                    "o documento (CPF) deve ter 11 dígitos, e se for lojista, o documento (CNPJ) deve ter 14 dígitos!");

            if (!account.Email.Contains("@"))
                throw new InvalidEmailException("Email inválido!");

            var passwordHash = PasswordHasher(account, account.PasswordHash);
            account.PasswordHash = passwordHash;

            try
            {
                return await _accountRepository.CreateAsync(account, cancellationToken);
            }
            catch (DbException)
            {
                throw new DuplicateAccountException("Email ou DisplayName já registrado. Use outro!");
            }
            catch
            {
                throw new DomainException("Erro interno ao criar a conta");
            }
        }

        public async Task<Account> Update(UpdateAccountDTO updateAccountDTO)
        {
            var account = await _accountRepository.GetAccountByIdAsync(updateAccountDTO.Id);
            var passwordHash = PasswordHasher(account!, updateAccountDTO.Password);

            account!.FullName = updateAccountDTO.FullName;
            account!.DisplayName = updateAccountDTO.DisplayName;
            account!.Email = updateAccountDTO.Email;
            account!.FullName = updateAccountDTO.FullName;
            account!.PasswordHash = passwordHash;

            try
            {
                return _accountRepository.Update(account);
            }
            catch (DbUpdateException)
            {
                throw new DuplicateAccountException("Email ou DisplayName já registrado. Use outro!");
            }
            catch
            {
                throw new DomainException("Erro interno ao atualizar a conta");
            }
        }

        public async Task<Account> AddFounds(Guid id, decimal amount, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id, cancellationToken);

            if (amount <= 0 || amount > decimal.MaxValue)
                throw new InvalidAmountException("A quantia a ser adicionada deve estar entre 1 e 79228162514264337593543950335!");

            var newBalance = account!.CurrentBalance + amount;
            account.CurrentBalance = newBalance;

            try
            {
                return _accountRepository.UpdateFounds(account);
            }
            catch
            {
                throw new DomainException("Erro interno ao atualizar saldo!");
            }
        }

        public async Task<Account> RemoveFounds(Guid id, decimal amount, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAccountByIdAsync(id, cancellationToken);

            if (amount >= 0 || amount < decimal.MinValue)
                throw new InvalidWithdrawalAmountException("A quantia a ser removida deve estar entre -1 e -79228162514264337593543950335!");

            var newBalance = account!.CurrentBalance + amount;

            if (newBalance < 0)
                throw new InsufficientFundsException("Saldo insuficiente para realizar a transação!");

            account!.CurrentBalance = newBalance;

            try
            {
                return _accountRepository.UpdateFounds(account);
            }
            catch
            {
                throw new DomainException("Erro interno ao atualizar saldo!");
            }
        }

        public async Task<string> DisableAccountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _accountRepository.DisableAccountAsync(id, cancellationToken);
                return "Conta desativada";
            }
            catch
            {
                throw new DomainException("Erro interno ao desativar a conta");
            }
        }

        public bool VerifyDocument(Account account)
        {
            var accountType = account.AccountType;
            var document = account.Document;

            if (accountType.ToLower() == EAccountType.Storekeeper.ToString().ToLower() && document.Length != 14 ||
                accountType.ToLower() == EAccountType.User.ToString().ToLower() && document.Length != 11)
                return false;

            return true;
        }

        public bool VerifyAccountType(Account account)
        {
            if (account.AccountType.ToLower() != EAccountType.Storekeeper.ToString().ToLower() &&
                account.AccountType.ToLower() != EAccountType.User.ToString().ToLower())
                return false;

            return true;
        }

        public string PasswordHasher(Account account, string password)
        {
            var hasher = new PasswordHasher<Account>();
            var passwordHash = hasher.HashPassword(account, password);

            return passwordHash;
        }

        public bool CheckPassword(Account account, string hashedPassword, string providerPassword)
        {
            var hasher = new PasswordHasher<Account>();
            var result = hasher.VerifyHashedPassword(account, hashedPassword, providerPassword);

            if (result == PasswordVerificationResult.Failed)
                return false;

            return true;
        }

        public async Task<bool> EnableAccountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _accountRepository.EnableAccountAsync(id, cancellationToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}