using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportApp.Data;

namespace TransportApp.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StatsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StatsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetStats()
        {
            var stats = new
            {
                TotalRoutes = await _context.Routes.CountAsync(),
                ActiveRoutes = await _context.Routes.CountAsync(r => r.IsActive),
                VehicleTypes = await _context.Routes.GroupBy(r => r.VehicleType).Select(g => new { Type = g.Key, Count = g.Count() }).ToListAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                TotalMessages = await _context.ContactMessages.CountAsync(),
                UnreadMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead)
            };
            return Ok(stats);
        }
    }
}
