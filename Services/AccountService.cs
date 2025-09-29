using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using simplified_picpay.DTOs;
using simplified_picpay.Enums;
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


        public async Task<(bool success, string? error, LoggedAccountViewModel? data)> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.LoginAsync(loginDTO.Email, cancellationToken);

            if (account == null)
                return (false, "Acesso negado!", null);

            if (!CheckPassword(account, account.PasswordHash, loginDTO.Password))
                return (false, "Acesso negado!", null);

            if (!account.IsActive)
            {
                var enabled = await EnableAccount(account.Id, cancellationToken);
                if (!enabled)
                    return (false, "Erro interno ao reativar conta!", null);

            }

            var token = _tokenService.GenerateTokenJwt(account.Id, account.Email, account.AccountType);

            return (true, null, new LoggedAccountViewModel
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
            });

        }

        public async Task<(bool success, string? error, AccountSummaryViewModel? data)> SearchAccountByDisplayNameAsync(SearchDisplayNameDTO searchDisplayNameDTO, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.SearchAccountByDisplayNameAsync(searchDisplayNameDTO.DisplayName, cancellationToken);
            string accountIsActive = "Conta ativa";

            if (account == null)
                return (false, "Conta não encontrada", null);

            if (!account.IsActive)
                accountIsActive = "Conta desativada";

            return (true, null, new AccountSummaryViewModel
            {
                DisplayName = account.DisplayName,
                PublicId = account.PublicId,
                AccountType = account.AccountType,
                IsActive = accountIsActive
            });
        }

        public async Task<(bool success, string? error, Account? data)> CreateAsync(Account account, CancellationToken cancellationToken = default)
        {
            if (!VerifyAccountType(account))
                return (false, "Só está disponível conta de User e Storekeeper!", null);

            if (!VerifyDocument(account))
                return (false, "Verifique o documento!", null);

            var passwordHash = PasswordHasher(account, account.PasswordHash);
            account.PasswordHash = passwordHash;

            try
            {
                return (true, null, await _accountRepository.CreateAsync(account, cancellationToken));
            }
            catch (DbException)
            {
                return (false, "Email ou DisplayName já registrado. Use outro!", null);
            }
            catch
            {
                return (false, "Erro interno ao criar a conta", null);
            }
        }

        public (bool success, string? error, Account? data) Update(Account account)
        {
            var passwordHash = PasswordHasher(account, account.PasswordHash);
            account.PasswordHash = passwordHash;

            try
            {
                return (true, null, _accountRepository.Update(account));
            }
            catch (DbUpdateException)
            {
                return (false, "Email ou DisplayName já registrado. Use outro!", null);
            }
            catch
            {
                return (false, "Erro interno ao criar a conta", null);
            }
        }

        public (bool success, string? error, Account? data) AddFounds(Account account, decimal amount)
        {
            if (amount <= 0 || amount > decimal.MaxValue)
                return (false, "A quantia a ser adicionada deve estar entre 1 e 79228162514264337593543950335!", null);

            var newBalance = account!.CurrentBalance + amount;
            account.CurrentBalance = newBalance;

            try
            {
                return (true, null, _accountRepository.UpdateFounds(account));
            }
            catch
            {
                return (false, "Erro interno ao atualizar saldo!", null);
            }
        }

        public (bool success, string? error, Account? data) RemoveFounds(Account account, decimal amount)
        {
            if (amount >= 0 || amount < decimal.MinValue)
                return (false, "A quantia a ser removida deve estar entre -1 e -79228162514264337593543950335!", null);

            var newBalance = account!.CurrentBalance + amount;

            if (newBalance < 0)
                return (false, "Saldo insuficiente", null);

            account!.CurrentBalance = newBalance;

            try
            {
                return (true, null, _accountRepository.UpdateFounds(account));
            }
            catch
            {
                return (false, "Erro interno ao atualizar saldo!", null);
            }
        }

        public async Task<(bool success, string? error, string? data)> DisableAccountAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _accountRepository.DisableAccountAsync(id, cancellationToken);
                return (true, null, "Conta desativada");
            }
            catch
            {
                return (false, "Erro interno ao desativar a conta", null);
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

        public async Task<bool> EnableAccount(Guid id, CancellationToken cancellationToken = default)
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