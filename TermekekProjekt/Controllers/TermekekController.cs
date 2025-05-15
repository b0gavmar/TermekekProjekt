using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TermekekProjekt.Models;

namespace TermekekProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermekekController : ControllerBase
    {
        private TermekekContext _context = new();

        [HttpGet("/api/purchases/count")]
        public async Task<IActionResult> GetPurchaseCount()
        {
            return Ok(_context.Purchases.Count());
        }

        [HttpGet("/api/purchases/count/{product_id}")]
        public async Task<IActionResult> GetPurchaseCountWithProductId([FromRoute]int product_id)
        {
            return Ok(_context.Purchases.Where(p => p.ProductId == product_id).Count());
        }

        [HttpGet("/api/customers/count")]
        public async Task<IActionResult> GetPurchaseCount([FromQuery] int min_budget)
        {
            return Ok(_context.Customers.Where(c => c.Budget>=min_budget).Count());
        }
    }
}
