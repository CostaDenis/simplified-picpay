using System.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class AccountController(IAccountRepository accountRepository,
    IAccountService AccountService, ITokenService tokenService) : ControllerBase
    {
        private readonly IAccountRepository _AccountRepository = accountRepository;
        private readonly IAccountService _accountService = AccountService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDTO,
                                                    CancellationToken cancellationToken)
        {
            var result = await _accountService.LoginAsync(loginDTO, cancellationToken);

            if (!result.success)
                return Unauthorized(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<LoggedAccountViewModel>(result.data!));

        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> SearchAccountByDisplayNameAsync([FromBody] SearchDisplayNameDTO searchDisplayNameDTO,
                                                                            CancellationToken cancellationToken)
        {
            var result = await _accountService.SearchAccountByDisplayNameAsync(searchDisplayNameDTO, cancellationToken);

            if (!result.success)
                return NotFound(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<AccountSummaryViewModel>(result.data!));

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

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<Account>(result.data!));
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
            var result = _accountService.Update(account);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<Account>(result.data!));
        }

        [HttpPut]
        [Route("add-founds")]
        public async Task<IActionResult> AddFounds([FromBody] UpdateFoundsDTO updateFoundsDTO,
                                                        CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var account = await _AccountRepository.GetAccountByIdAsync(id, cancellationToken);
            var result = _accountService.AddFounds(account!, updateFoundsDTO.Amount);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<Account>(result.data!));
        }

        [HttpPut]
        [Route("remove-founds")]
        public async Task<IActionResult> RemoveFounds([FromBody] UpdateFoundsDTO updateFoundsDTO,
                                                        CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var account = await _AccountRepository.GetAccountByIdAsync(id, cancellationToken);
            var result = _accountService.AddFounds(account!, updateFoundsDTO.Amount);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<Account>(result.data!));
        }

        [HttpPut]
        [Route("disable-account")]
        public async Task<IActionResult> DisableAccountAsync(CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _accountService.DisableAccountAsync(id, cancellationToken);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<Account>(result.data!));
        }
    }
}