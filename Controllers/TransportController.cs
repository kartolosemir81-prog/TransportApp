using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportApp.Data;
using TransportApp.Models;

namespace TransportApp.Controllers
{
    public class TransportController : Controller
    {
        private readonly AppDbContext _context;

        public TransportController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? vehicleType = null, string? search = null)
        {
            var routes = _context.Routes.Where(r => r.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(vehicleType))
                routes = routes.Where(r => r.VehicleType == vehicleType);

            if (!string.IsNullOrEmpty(search))
                routes = routes.Where(r => r.RouteName.Contains(search) || r.StartPoint.Contains(search) || r.EndPoint.Contains(search) || r.Stops.Contains(search));

            ViewBag.VehicleTypes = await _context.Routes.Select(r => r.VehicleType).Distinct().ToListAsync();
            ViewBag.SelectedType = vehicleType;
            ViewBag.Search = search;

            return View(routes.OrderBy(r => r.VehicleType).ThenBy(r => r.DepartureTime).ToList());
        }

        public async Task<IActionResult> Details(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return NotFound();
            return View(route);
        }

        [Route("Transport/ByRoute/{routeName}")]
        public async Task<IActionResult> ByRoute(string routeName)
        {
            var routes = await _context.Routes.Where(r => r.RouteName.Contains(routeName) && r.IsActive).OrderBy(r => r.DepartureTime).ToListAsync();
            ViewBag.RouteName = routeName;
            return View(routes);
        }
    }
}
