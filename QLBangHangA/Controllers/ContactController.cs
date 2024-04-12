using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using QLBangHangA.Data_Access;
using QLBangHangA.Models.Entities;

namespace QLBangHangA.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService _notifyService { get; }
        public ContactController(ApplicationDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitMessage(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateCreated = DateTime.Now;
                _context.Add(contact);
                await _context.SaveChangesAsync();
                _notifyService.Success("Gửi liên hệ thành công!");
                return RedirectToAction("Index", "Contact");

            }
            _notifyService.Success("Gửi liên hệ thành công!");

            return View();
        }
    }
}
