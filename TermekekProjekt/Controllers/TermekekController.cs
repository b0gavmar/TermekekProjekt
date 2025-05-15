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
        public async Task<IActionResult> GetCustomerCountByBudget([FromQuery] int min_budget)
        {
            return Ok(_context.Customers.Where(c => c.Budget>=min_budget).Count());
        }

        [HttpGet("/api/products/category/{category_id}")]
        public async Task<IActionResult> GetProductsInACategory([FromRoute] int category_id)
        {
            return Ok(_context.Products.Where(p => p.Categoryid == category_id));
        }

        [HttpGet("/api/purchases")]
        public async Task<IActionResult> GetPurchasesOrdered([FromQuery] string order_by = "date", [FromQuery] bool desc = false)
        {
            if (desc)
            {
                return Ok(_context.Purchases.OrderByDescending(p => p.Date));
            }
            return Ok(_context.Purchases.OrderBy(p => p.Date));
        }

        [HttpGet("/api/customers")]
        public async Task<IActionResult> GetCustomersByBudget([FromQuery] int min_budget)
        {
            return Ok(_context.Customers.Where(c => c.Budget >= min_budget));
        }
    }
}
