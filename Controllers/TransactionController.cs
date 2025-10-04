using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplified_picpay.DTOs.Transaction;
using simplified_picpay.Models;
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
            createTransactionDTO.PayerId = payerId;
            var result = await _transactionService.CreateTransactionAsync(createTransactionDTO, cancellationToken);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            return Ok(new ResultViewModel<Transaction>(result.data!));
        }

        [HttpGet]
        public async Task<IActionResult> AllYourTransactionsAsync(CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _transactionService.AllYourTransactionsAsync(id, cancellationToken);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            if (result.data!.Count == 0)
                return Ok(new ResultViewModel<string>(data: "Nenhuma transação encontrada!"));

            return Ok(new ResultViewModel<List<Transaction>>(result.data!));
        }

        [HttpGet]
        [Route("received")]
        public async Task<IActionResult> AllYourTransactionsReceivedAsync(CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _transactionService.AllYourTransactionsReceivedAsync(id, cancellationToken);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            if (result.data!.Count == 0)
                return Ok(new ResultViewModel<string>(data: "Nenhuma transação encontrada!"));

            return Ok(new ResultViewModel<List<Transaction>>(result.data!));
        }

        [HttpGet]
        [Route("paied")]
        public async Task<IActionResult> AllYourTransactionsPaiedAsync(CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _transactionService.AllYourTransactionsPaiedAsync(id, cancellationToken);

            if (!result.success)
                return BadRequest(new ResultViewModel<string>(result.error!));

            if (result.data!.Count == 0)
                return Ok(new ResultViewModel<string>(data: "Nenhuma transação encontrada!"));

            return Ok(new ResultViewModel<List<Transaction>>(result.data!));
        }
    }
}