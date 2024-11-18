using ERP_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ERPDbContext _context;
        public OrdersController(ERPDbContext context)
        {
            _context = context;
        }
        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.tblOrders
                .Include(o => o.Product)
                .Select(o => new
                {
                    o.Id,
                    o.CustomerName,
                    o.Quantity,
                    OrderDate = o.OrderDate.Value.ToLocalTime(),
                    ProductName = o.Product.Name,
                    o.Product.UnitPrice

                }).ToListAsync();
            return Ok(orders);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.tblOrders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            order.OrderDate = order.OrderDate.Value.ToLocalTime();
            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<IActionResult> CreateNewOrder(Order order)
        {
            var product = await _context.tblProducts.FindAsync(order.ProductId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            if(product.Stock<order.Quantity)
            {
                return BadRequest("Insufficient stock.");
            }
            var newOrder = new Order()
            {
                ProductId = order.ProductId,
                CustomerName = order.CustomerName,
                Quantity = order.Quantity,
                OrderDate = DateTime.UtcNow
            };

            product.Stock -= order.Quantity;
            await _context.tblOrders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrderById",new { id = newOrder.Id }, newOrder);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            var existingOrder = await _context.tblOrders
                .Include(o=>o.Product)
                .FirstOrDefaultAsync(o=>o.Id == id);
            if (existingOrder==null)
            {
                return NotFound("Order not found.");
            }
            if(order.Quantity<1)
            {
                return BadRequest("Quantity must be 1 or more!");
            }

            var quantityDifference = order.Quantity - existingOrder.Quantity;
            if(existingOrder.Product.Stock < quantityDifference)
            {
                return BadRequest("Insufficient stock for updatig!");
            }

            existingOrder.Product.Stock -= quantityDifference;
            existingOrder.Quantity = order.Quantity;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrderById", new {id = existingOrder.Id},existingOrder);
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.tblOrders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) 
            {
               return NotFound("Order not found!");
            }
            order.Product.Stock += order.Quantity;
            _context.tblOrders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok(order);
        }
    }
}
