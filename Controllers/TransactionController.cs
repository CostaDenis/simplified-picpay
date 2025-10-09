using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplified_picpay.DTOs.TransactionDTOs;
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

            return Created(
                            $"/transactions/{result.Id}",
                            new ResultViewModel<TransactionViewModel>(
                            new TransactionViewModel
                            {
                                Id = result.Id,
                                PayerPublicId = result.PayerPublicId,
                                PayeePublicId = result.PayeePublicId,
                                Value = result.Value
                            }));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var accountId = _tokenService.GetAccounId(this.HttpContext);
            var result = await _transactionService.GetTransactionByIdAsync(id, accountId, cancellationToken);

            if (result == null)
                return NotFound(new ResultViewModel<string>(data: "Nenhuma transação encontrada!"));

            return Ok(new ResultViewModel<TransactionViewModel>(new TransactionViewModel
            {
                Id = result.Id,
                PayerPublicId = result.PayerPublicId,
                PayeePublicId = result.PayeePublicId,
                Value = result.Value
            }));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllYourTransactionsAsync([FromBody] PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _transactionService.GetAllYourTransactionsAsync(id, paginationTransactionDTO, cancellationToken);
            var transactionsViewModel = new List<TransactionViewModel>();

            if (result == null)
                return NotFound(new ResultViewModel<string>(data: "Nenhuma transação encontrada!"));

            foreach (var transaction in result)
            {
                transactionsViewModel.Add(new TransactionViewModel
                {
                    Id = transaction.Id,
                    PayerPublicId = transaction.PayerPublicId,
                    PayeePublicId = transaction.PayeePublicId,
                    Value = transaction.Value
                });
            }

            return Ok(new ResultViewModel<List<TransactionViewModel>>(transactionsViewModel));
        }

        [HttpGet]
        [Route("received")]
        public async Task<IActionResult> GetAllReceivedTransactionsAsync([FromBody] PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _transactionService.GetAllReceivedTransactionsAsync(id, paginationTransactionDTO, cancellationToken);
            var transactionsViewModel = new List<TransactionViewModel>();

            if (result == null)
                return NotFound(new ResultViewModel<string>(data: "Nenhuma transação encontrada!"));

            foreach (var transaction in result)
            {
                transactionsViewModel.Add(new TransactionViewModel
                {
                    Id = transaction.Id,
                    PayerPublicId = transaction.PayerPublicId,
                    PayeePublicId = transaction.PayeePublicId,
                    Value = transaction.Value
                });
            }

            return Ok(new ResultViewModel<List<TransactionViewModel>>(transactionsViewModel));
        }

        [HttpGet]
        [Route("paid")]
        public async Task<IActionResult> GetAllPaidTransactionsAsync([FromBody] PaginationTransactionDTO paginationTransactionDTO, CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var result = await _transactionService.GetAllPaidTransactionsAsync(id, paginationTransactionDTO, cancellationToken);
            var transactionsViewModel = new List<TransactionViewModel>();

            if (result == null)
                return Ok(new ResultViewModel<string>(data: "Nenhuma transação encontrada!"));

            foreach (var transaction in result)
            {
                transactionsViewModel.Add(new TransactionViewModel
                {
                    Id = transaction.Id,
                    PayerPublicId = transaction.PayerPublicId,
                    PayeePublicId = transaction.PayeePublicId,
                    Value = transaction.Value
                });
            }

            return Ok(new ResultViewModel<List<TransactionViewModel>>(transactionsViewModel));
        }
    }
}