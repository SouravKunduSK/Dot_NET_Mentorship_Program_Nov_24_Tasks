using ERP_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ERPDbContext _context;
        public ProductsController(ERPDbContext context)
        {
            _context = context;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        [Route("LowStock")]
        public async Task<IActionResult> GetLowStockProducts([FromQuery] decimal threshold)
        {
            var products = await _context.tblProducts
                .Where(p=>p.Stock<threshold)
                .Select(p=> new
                {
                   ProductName = p.Name,
                   UnitPrices = p.UnitPrice,
                    Stock = p.Stock
                }).ToListAsync();
            if(products==null)
            {
                return NotFound("No Product Found!");
            }

            return Ok(products);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
