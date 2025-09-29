using System.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using simplified_picpay.DTOs;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Controllers
{
    [ApiController]
    [Authorize(Roles = "user, storekeeper")]
    [Route("accounts")]
    public class AccountController(IAccountRepository repository,
    IAccountService AccountService, ITokenService tokenService) : ControllerBase
    {
        private readonly IAccountRepository _repository = repository;
        private readonly IAccountService _accountService = AccountService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO,
                                                    CancellationToken cancellationToken)
        {
            var account = await _repository.LoginAsync(loginDTO.Email, cancellationToken);

            if (account == null)
                return Unauthorized(new ResultViewModel<string>("Acesso negado!"));

            if (!_accountService.CheckPassword(account, account.PasswordHash, loginDTO.Password))
                return Unauthorized(new ResultViewModel<string>("Acesso negado!"));

            if (account.IsActive == false)
            {
                if (!await _accountService.EnableAccount(account.Id, cancellationToken))
                    return StatusCode(500, new ResultViewModel<string>("Erro interno ao reativar sua conta!"));

                return Ok(new ResultViewModel<string>(data: "Conta reativada!"));
            }

            var token = _tokenService.GenerateTokenJwt(account.Id, account.Email, account.AccountType);

            return Ok(new ResultViewModel<LoggedAccountViewModel>(new LoggedAccountViewModel
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
            }));

        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> SearchAccountByDisplayNameAsync([FromBody] SearchDisplayNameDTO searchDisplayNameDTO,
                                                                            CancellationToken cancellationToken)
        {
            var account = await _repository.SearchAccountByDisplayNameAsync(searchDisplayNameDTO.DisplayName, cancellationToken);
            string accountIsActive = "Conta ativa";

            if (account == null)
                return NotFound(new ResultViewModel<string>("Conta não encontrada!"));

            if (!account.IsActive)
                accountIsActive = "Conta desativada";

            return Ok(new ResultViewModel<AccountSummaryViewModel>(new AccountSummaryViewModel
            {
                DisplayName = account.DisplayName,
                PublicId = account.PublicId,
                AccountType = account.AccountType,
                IsActive = accountIsActive
            }));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAccountDTO createAccountDTO,
                                                        CancellationToken cancellationToken)
        {

            var account = new Account
            {
                FullName = createAccountDTO.FullName,
                DisplayName = createAccountDTO.DisplayName,
                Email = createAccountDTO.Email,
                PasswordHash = createAccountDTO.Password,
                CurrentBalance = 0.0M,
                AccountType = createAccountDTO.AccountType.ToLower(),
                Document = createAccountDTO.Document
            };

            if (!_accountService.VerifyAccountType(account))
                return BadRequest(new ResultViewModel<string>("Só está disponível conta de User e Storekeeper!"));

            if (!_accountService.VerifyDocument(account))
                return BadRequest(new ResultViewModel<string>("Verifique o documento!"));

            var passwordHash = _accountService.PasswordHasher(account, account.PasswordHash);
            account.PasswordHash = passwordHash;

            try
            {
                return Ok(new ResultViewModel<Account>(await _repository.CreateAsync(account, cancellationToken)));
            }
            catch (DbException)
            {
                return StatusCode(500, new ResultViewModel<string>("Esse email já esta registrado!"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao criar a conta"));
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateAccountDTO updateAccountDTO)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var account = new Account
            {
                Id = id,
                Email = updateAccountDTO.Email,
                PasswordHash = updateAccountDTO.Password
            };

            var passwordHash = _accountService.PasswordHasher(account, account.PasswordHash);
            account.PasswordHash = passwordHash;

            try
            {
                return Ok(_repository.Update(account));
            }
            catch (DbUpdateException)
            {
                return Conflict(new ResultViewModel<string>("Email já resgitrado na aplicação!"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao atualizar a conta"));
            }
        }

        [HttpPut]
        [Route("add-founds")]
        public async Task<IActionResult> AddFounds([FromBody] UpdateFoundsDTO updateFoundsDTO,
                                                        CancellationToken cancellationToken)
        {
            if (updateFoundsDTO.Amount <= 0 || updateFoundsDTO.Amount > decimal.MaxValue)
                return BadRequest(new ResultViewModel<string>("A quantia a ser adicionada deve estar entre 1 e 79228162514264337593543950335!"));

            var id = _tokenService.GetAccounId(this.HttpContext);
            var account = await _repository.GetAccountByIdAsync(id, cancellationToken);
            var amount = updateFoundsDTO.Amount;
            var newBalance = account!.CurrentBalance + amount;

            account!.CurrentBalance = newBalance;

            try
            {
                _repository.UpdateFounds(account);
                return Ok(new ResultViewModel<Account>(account));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao atualizar saldo!"));
            }
        }

        [HttpPut]
        [Route("remove-founds")]
        public async Task<IActionResult> RemoveFounds([FromBody] UpdateFoundsDTO updateFoundsDTO,
                                                        CancellationToken cancellationToken)
        {
            if (updateFoundsDTO.Amount >= 0 || updateFoundsDTO.Amount < decimal.MinValue)
                return BadRequest(new ResultViewModel<string>("A quantia a ser removida deve estar entre -1 e -79228162514264337593543950335!"));

            var id = _tokenService.GetAccounId(this.HttpContext);
            var account = await _repository.GetAccountByIdAsync(id, cancellationToken);
            var amount = updateFoundsDTO.Amount;
            var newBalance = account!.CurrentBalance + amount;

            if (newBalance < 0)
                return BadRequest(new ResultViewModel<string>("Saldo insuficiente"));

            account!.CurrentBalance = newBalance;

            try
            {
                _repository.UpdateFounds(account);
                return Ok(new ResultViewModel<Account>(account));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao atualizar saldo!"));
            }
        }

        [HttpPut]
        [Route("disable-account")]
        public async Task<IActionResult> DisableAccount(CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);

            try
            {
                await _repository.DisableAccountAsync(id, cancellationToken);
                return Ok(new ResultViewModel<string>(data: "Conta desativada"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao desativar a conta"));
            }
        }
    }
}