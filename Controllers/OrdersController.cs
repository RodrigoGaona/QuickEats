using FastFoodAPI.Data;
using FastFoodAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    // Endpoint para guardar una nueva orden
    [HttpPost]
    public async Task<IActionResult> AddOrder([FromBody] Order order)
    {
        order.CreationDate = DateTime.Now;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
    }

    // Endpoint para obtener todas las órdenes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders.ToListAsync();
    }
}