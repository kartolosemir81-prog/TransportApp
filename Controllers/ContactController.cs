using Microsoft.AspNetCore.Mvc;
using TransportApp.Data;
using TransportApp.Models;

namespace TransportApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(ContactMessage message)
        {
            if (ModelState.IsValid)
            {
                message.SentAt = DateTime.Now;
                _context.Add(message);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Mesajın başarıyla gönderildi!";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
