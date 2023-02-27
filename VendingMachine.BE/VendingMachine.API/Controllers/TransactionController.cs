using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VendingMachine.API.Models;
using VendingMachine.BLL;
using VendingMachine.BLL.Interfaces;
using VendingMachine.BLL.Models;
using VendingMachine.DAL;

namespace VendingMachine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("deposit")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> Deposit([FromQuery] int deposit)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _transactionService.Deposit(deposit, userId);

            return Ok();
        }

        [HttpPost("buy")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> Buy([FromBody] BuyRequest buyRequest)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var buyResponse = await _transactionService.Buy(buyRequest.ProductId, buyRequest.AmountOfProducts, userId);

            return Ok(buyResponse);
        }

        [HttpGet("reset")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> Reset()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _transactionService.Reset(userId);

            return Ok();
        }
    }
}
