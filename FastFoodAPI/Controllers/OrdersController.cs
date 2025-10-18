using FastFoodAPI.Data;
using FastFoodAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// NOTA: El namespace de tu controlador es FastFoodAPI.Controllers
namespace FastFoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(Order order)
        {
            // Puedes agregar lógica de validación aquí si es necesario
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }

        // DELETE: api/Orders/{id} (Nueva funcionalidad)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound(); // 404
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}