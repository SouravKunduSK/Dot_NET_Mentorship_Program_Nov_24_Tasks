using ERP_Project.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
            return Ok(order);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
