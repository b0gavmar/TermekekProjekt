using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        [HttpGet("/api/products/count_by_category")]
        public async Task<IActionResult> GetProductsCountByCategory()
        {
            return Ok(_context.Products.GroupBy(p => p.Categoryid).Select(g => new { category_id = g.Key, count = g.ToList() }));
        }

        [HttpGet("/api/products/average_price_by_category")]
        public async Task<IActionResult> GetProductsAvgPriceByCategory()
        {
            return Ok(_context.Products.GroupBy(p => p.Categoryid).Select(g => new { category_id = g.Key, avg_price = g.Average(p => p.Price) }));
        }

        [HttpGet("/api/purchases/total_spent_by_customer")]
        public async Task<IActionResult> GetSpentByCustomer()
        {
            return Ok(_context.Purchases
                                .Join(_context.Products, p => p.ProductId, pr => pr.Id, (p, pr) => new { customer_id = p.CustomerId, Price = pr.Price * p.Quantity })
                                .GroupBy(p => p.customer_id)
                                .Select(g=> new { kulcs = g.Key, ertek = g.Sum(p=>p.Price)}));
        }

        [HttpGet("/api/purchases/most_purchesed_product")]
        public async Task<IActionResult> GetMostPurchased()
        {
            var grouped = await _context.Purchases
                .GroupBy(p => p.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(p => p.Quantity)
                })
                .ToListAsync();

            var maxQuantity = grouped.Max(g => g.TotalQuantity);

            var mostPurchased = grouped
                .Where(g => g.TotalQuantity == maxQuantity)
                .ToList();

            return Ok(mostPurchased);
        }
    }
}
