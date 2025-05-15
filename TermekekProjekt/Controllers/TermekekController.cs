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

        [HttpGet("/api/customer/{customer_id}")]
        public async Task<IActionResult> GetCustomerPurchases([FromRoute] int customer_id)
        {
            return Ok(_context.Purchases.Where(p => p.CustomerId == customer_id));
        }

        /* nem lehet megcsinálni mert nincs purchase_id meg egy vásárlásban egy termék van
        [HttpGet("/api/purchases/{purchase_id}/products")]
        public async Task<IActionResult> GetPurchasedProducts([FromRoute] int purchase_id)
        {
        }*/

        [HttpGet("/api/products/{product_id}/stock")]
        public async Task<IActionResult> GetProductStock([FromRoute] int product_id)
        {
            return Ok(_context.Products.Where(p => p.Id == product_id).Select(p => p.Stock));
        }

        /*
        így kéne megoldani, ha lenne a felhasználónak id-je :)
        [HttpGet("/api/products/{customer_id}/budget")]
        public async Task<IActionResult> GetCustomerBudget([FromRoute] int customer_id)
        {
            return Ok(_context.Customers.Where(c => c.Id == customer_id).Select(c => c.Budget));
        }*/

        [HttpGet("/api/purchases/date")]
        public async Task<IActionResult> GetPurchasesByDate([FromQuery] DateOnly? start_date = null, [FromQuery] DateOnly? end_date = null)
        {
            if(start_date > end_date)
            {
                return BadRequest("Start date must be less than or equal to end date.");
            }
            if(start_date == null || end_date == null)
            {
                return BadRequest("Start date and end date cannot be null.");
            }
            return Ok(_context.Purchases.Where(p => p.Date >= start_date && p.Date <= end_date));
        }

        [HttpGet("/api/customers/budget")]
        public async Task<IActionResult> GetCustomersByBudget([FromQuery] int min_budget = 0, [FromQuery] int max_budget = int.MaxValue)
        {
            return Ok(_context.Customers.Where(c => c.Budget >= min_budget && c.Budget <= max_budget));
        }

        [HttpGet("/api/products/price")]
        public async Task<IActionResult> GetProductByPrise([FromQuery] int min = 0, [FromQuery] int max = int.MaxValue)
        {
            return Ok(_context.Products.Where(p=>p.Price>=min && p.Price<=max));
        }
    }
}
