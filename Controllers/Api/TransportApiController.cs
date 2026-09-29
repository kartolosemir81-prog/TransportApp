using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportApp.Data;
using TransportApp.Models;

namespace TransportApp.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TransportApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransportApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteInfo>>> GetAll([FromQuery] string? vehicleType = null, [FromQuery] string? search = null)
        {
            var routes = _context.Routes.Where(r => r.IsActive).AsQueryable();
            if (!string.IsNullOrEmpty(vehicleType)) routes = routes.Where(r => r.VehicleType == vehicleType);
            if (!string.IsNullOrEmpty(search)) routes = routes.Where(r => r.RouteName.Contains(search) || r.StartPoint.Contains(search) || r.EndPoint.Contains(search) || r.Stops.Contains(search));
            return Ok(await routes.OrderBy(r => r.VehicleType).ThenBy(r => r.DepartureTime).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RouteInfo>> GetById(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return NotFound(new { message = "Servis bulunamadı" });
            return Ok(route);
        }

        [HttpGet("route/{routeName}")]
        public async Task<ActionResult<IEnumerable<RouteInfo>>> GetByRoute(string routeName)
        {
            var routes = await _context.Routes.Where(r => r.RouteName.Contains(routeName) && r.IsActive).OrderBy(r => r.DepartureTime).ToListAsync();
            return Ok(routes);
        }

        [HttpGet("vehicle-types")]
        public async Task<ActionResult<IEnumerable<string>>> GetVehicleTypes()
        {
            var types = await _context.Routes.Select(r => r.VehicleType).Distinct().ToListAsync();
            return Ok(types);
        }

        [HttpPost]
        public async Task<ActionResult<RouteInfo>> Create(RouteInfo route)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            route.CreatedAt = DateTime.Now;
            _context.Add(route);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = route.Id }, route);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RouteInfo route)
        {
            if (id != route.Id) return BadRequest(new { message = "ID eşleşmiyor" });
            _context.Entry(route).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return NotFound(new { message = "Servis bulunamadı" });
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return NotFound(new { message = "Servis bulunamadı" });
            route.IsActive = !route.IsActive;
            await _context.SaveChangesAsync();
            return Ok(new { id = route.Id, isActive = route.IsActive });
        }
    }
}
