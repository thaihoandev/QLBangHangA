using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using QLBangHangA.Data_Access;
using QLBangHangA.Extentions;
using QLBangHangA.Models;
using QLBangHangA.Models.Entities;

namespace QLBangHangA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public INotyfService _notifyService { get; }

        public AdminAccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;

            var adminRole = await _roleManager.FindByNameAsync(SD.Role_Admin);
            var usersInAdminRole = await _userManager.GetUsersInRoleAsync(adminRole.Name);
            var users = usersInAdminRole.AsQueryable();

            PagedList<ApplicationUser> models = new PagedList<ApplicationUser>(users.Where(x => x.Active == true), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }
        public async Task<IActionResult> Customer(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;

            var adminRole = await _roleManager.FindByNameAsync(SD.Role_Customer);
            var usersInAdminRole = await _userManager.GetUsersInRoleAsync(adminRole.Name);
            var users = usersInAdminRole.AsQueryable();

            PagedList<ApplicationUser> models = new PagedList<ApplicationUser>(users.Where(x=>x.Active == true), pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }

        // GET: Admin/AdminAccount/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.ApplicationUsers == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/AdminAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ApplicationUsers == null)
            {
                return Problem("Entity set 'DbEcommerceMarketContext.ApplicationUsers'  is null.");
            }
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user != null)
            {
                user.Active = false;
                _context.ApplicationUsers.Update(user);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa tài khoản thành công!");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return (_context.ApplicationUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
