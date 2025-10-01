using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplified_picpay.DTOs.Transaction;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Controllers
{

    [ApiController]
    [Authorize(Roles = "user, storekeeper")]
    [Route("transactions")]
    public class TransactionController(ITransactionService transactionService,
                                        ITokenService tokenService) : ControllerBase
    {
        private readonly ITransactionService _transactionService = transactionService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] CreateTransactionDTO createTransactionDTO,
                                                                    CancellationToken cancellationToken)
        {
            var payerId = _tokenService.GetAccounId(this.HttpContext);
            var transaction = new Transaction
            {
                PayerId = payerId,
                PayeeId = createTransactionDTO.PayeeId,
                Value = createTransactionDTO.Value
            };
            var result = await _transactionService.CreateTransactionAsync(transaction, cancellationToken);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<Transaction>(result.data!));
        }
    }
}