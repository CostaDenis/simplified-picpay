using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplified_picpay.DTOs.Transaction;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Controllers
{

    [ApiController]
    [Authorize(Roles = "user, storekeeper")]
    [Route("accounts")]
    public class TransactionController(ITransactionRepository transactionRepository,
                                        ITokenService tokenService) : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] CreateTransactionDTO createTransactionDTO)
        {
            var payerid = _tokenService.GetAccounId(this.HttpContext);
            var transaction = new Transaction
            {
                PayerId = payerid,
                PayeeId = createTransactionDTO.PayeeId,
                Value = createTransactionDTO.Value
            };

            return Ok();
        }
    }
}