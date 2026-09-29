using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportApp.Data;
using TransportApp.Models;

namespace TransportApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new DashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalRoutes = await _context.Routes.CountAsync(),
                ActiveRoutes = await _context.Routes.CountAsync(r => r.IsActive),
                TotalMessages = await _context.ContactMessages.CountAsync(),
                UnreadMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead)
            };
            return View(stats);
        }

        public async Task<IActionResult> Routes()
        {
            var routes = await _context.Routes.OrderByDescending(r => r.CreatedAt).ToListAsync();
            return View(routes);
        }

        public IActionResult CreateRoute() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoute(RouteInfo route)
        {
            if (ModelState.IsValid)
            {
                route.CreatedAt = DateTime.Now;
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Routes));
            }
            return View(route);
        }

        public async Task<IActionResult> EditRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return NotFound();
            return View(route);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoute(int id, RouteInfo route)
        {
            if (id != route.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Routes));
            }
            return View(route);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Routes));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                route.IsActive = !route.IsActive;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Routes));
        }

        public async Task<IActionResult> Messages()
        {
            var messages = await _context.ContactMessages.OrderByDescending(m => m.SentAt).ToListAsync();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Messages));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                _context.ContactMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Messages));
        }
    }

    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalRoutes { get; set; }
        public int ActiveRoutes { get; set; }
        public int TotalMessages { get; set; }
        public int UnreadMessages { get; set; }
    }
}
