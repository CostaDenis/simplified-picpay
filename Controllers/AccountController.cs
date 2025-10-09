using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplified_picpay.DTOs.AccountDTOs;
using simplified_picpay.Models;
using simplified_picpay.Services.Abstractions;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Controllers
{
    [ApiController]
    [Authorize(Roles = "user, storekeeper")]
    [Route("accounts")]
    public class AccountController(IAccountService AccountService, ITokenService tokenService) : ControllerBase
    {
        private readonly IAccountService _accountService = AccountService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDTO,
                                                    CancellationToken cancellationToken)
        {
            var result = await _accountService.LoginAsync(loginDTO, cancellationToken);
            return Ok(new ResultViewModel<LoggedAccountViewModel>(result));
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> SearchAccountByDisplayNameAsync([FromBody] SearchDisplayNameDTO searchDisplayNameDTO,
                                                                            CancellationToken cancellationToken)
        {
            var result = await _accountService.SearchAccountByDisplayNameAsync(searchDisplayNameDTO, cancellationToken);
            return Ok(new ResultViewModel<AccountSummaryViewModel>(result));
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

            var result = await _accountService.CreateAsync(account, cancellationToken);

            return Ok(new ResultViewModel<AccountViewModel>(new AccountViewModel
            {
                Id = result.Id,
                FullName = result.FullName,
                DisplayName = result.DisplayName,
                PublicId = result.PublicId,
                Email = result.Email,
                AccountType = result.AccountType,
                Document = result.Document
            }));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAccountDTO updateAccountDTO)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            updateAccountDTO.Id = id;

            var result = await _accountService.Update(updateAccountDTO);

            return Ok(new ResultViewModel<AccountViewModel>(new AccountViewModel
            {
                Id = result.Id,
                FullName = result.FullName,
                DisplayName = result.DisplayName,
                PublicId = result.PublicId,
                Email = result.Email,
                AccountType = result.AccountType,
                Document = result.Document
            }));
        }

        [HttpPut]
        [Route("add-founds")]
        public async Task<IActionResult> AddFounds([FromBody] UpdateFoundsDTO updateFoundsDTO,
                                                        CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _accountService.AddFounds(id, updateFoundsDTO.Amount, cancellationToken);

            return Ok(new ResultViewModel<NewAccountBalanceViewModel>(new NewAccountBalanceViewModel
            {
                Id = result.Id,
                PublicId = result.PublicId,
                Balance = result.CurrentBalance
            }));
        }

        [HttpPut]
        [Route("remove-founds")]
        public async Task<IActionResult> RemoveFounds([FromBody] UpdateFoundsDTO updateFoundsDTO,
                                                        CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _accountService.RemoveFounds(id, updateFoundsDTO.Amount, cancellationToken);

            return Ok(new ResultViewModel<NewAccountBalanceViewModel>(new NewAccountBalanceViewModel
            {
                Id = result.Id,
                PublicId = result.PublicId,
                Balance = result.CurrentBalance
            }));
        }

        [HttpPut]
        [Route("disable-account")]
        public async Task<IActionResult> DisableAccountAsync(CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _accountService.DisableAccountAsync(id, cancellationToken);

            return Ok(new ResultViewModel<string>(data: result));
        }
    }
}