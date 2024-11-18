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
        public async Task<IActionResult> CreateNewOrder([FromBody]OrderDTO order)
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
        public async Task<IActionResult> UpdateOrder(int id,[FromBody] OrderDTO order)
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


        //Get api/ product/ summary
        [HttpGet]
        [Route("Products/Summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _context.tblOrders
                .GroupBy(o => o.Product)
                .Select(g=> new
                {
                    ProductName = g.Key.Name,
                    TotalQuantityOrdered = g.Sum(o=>o.Quantity),
                    TotalRevenue = g.Sum(o=>o.Quantity*g.Key.UnitPrice)
                }).ToListAsync();
            return Ok(summary);
        }

        //Get api/ TopCustomers
        [HttpGet]
        [Route("Customers")]
        public async Task<IActionResult> GetTopCustomers()
        {
            var customers = await _context.tblOrders
                .GroupBy(o => o.CustomerName)
                .OrderByDescending(g => g.Sum(o => o.Quantity))
                .Take(3)
                .Select(g => new
                {
                    CustomerName = g.Key,
                    TotalQuantity = g.Sum(o => o.Quantity)
                }).ToListAsync();
            if(customers == null)
            {
                return NotFound("Customer not found!");
            }

            return Ok(customers);
        }

        [HttpPost]
        [Route("BulkOrderCreation")]
        public async Task<IActionResult> CreateBulkOrder([FromBody] List<OrderDTO> orders)
        {
            if (orders == null || !orders.Any())
            {
                return BadRequest("Order list cannot be empty.");
            }
                
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var order in orders)
                {
                    //Check if product exists
                    var product = await _context.tblProducts.FirstOrDefaultAsync(p=>p.Id == order.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {order.ProductId} does not exist.");
                    }
                    //Check stock
                    if(product.Stock<order.Quantity)
                    {
                        throw new Exception($"Insufficient stock for Product ID {order.ProductId}. Available stock: {product.Stock}");
                    }

                    //Create new order
                    var newOrder = new Order()
                    {
                        ProductId = order.ProductId,
                        CustomerName = order.CustomerName,
                        Quantity = order.Quantity,
                        OrderDate = DateTime.UtcNow
                    };

                    await _context.tblOrders.AddAsync(newOrder);
                    product.Stock -= order.Quantity;
                    _context.tblProducts.Update(product);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok("Bulk orders created successfully.");
            }
            catch (Exception ex) 
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.Message);
            }
        }
    }
}
