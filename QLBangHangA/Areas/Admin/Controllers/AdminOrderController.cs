using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class AdminOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService _notifyService { get; }
        public AdminOrderController(ApplicationDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }
        public IActionResult Index(int? page = 0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var confirmed = _context.TransactStatuses.SingleOrDefault(x => x.Status.ToLower() == "confirmed");

            var lsOrders = _context.Orders
                .AsNoTracking()
                .OrderByDescending(x => x.OrderId).Where(y => y.TransactStatusId == confirmed.TransactStatusId && y.Deleted == false); ;
            PagedList<Order> models = new PagedList<Order>(lsOrders, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;


            return View(models);
        }

        public IActionResult Pending(int? page = 0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;

            var pending = _context.TransactStatuses.SingleOrDefault(x => x.Status.ToLower() == "pending");
            var lsOrders = _context.Orders
                .AsNoTracking()
                .OrderByDescending(x => x.OrderId).Where(y => y.TransactStatusId == pending.TransactStatusId && y.Deleted == false) ;
            PagedList<Order> models = new PagedList<Order>(lsOrders, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;


            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Confirmed(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == orderId);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                var confirmed = _context.TransactStatuses.SingleOrDefault(x => x.Status.ToLower() == "confirmed");
                order.TransactStatusId = confirmed.TransactStatusId;
                _context.Update(order);
                await _context.SaveChangesAsync();
                _notifyService.Success("Confirm Success!");
                return RedirectToAction("Pending");
            }
        }
        // GET: Admin/AdminOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            ViewBag.OrderDetails = _context.OrderDetails.Where(x => x.OrderId == id).ToList();

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Products = _context.Products.ToList();

            return View(order);
        }

        // GET: Admin/AdminOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/AdminOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DbEcommerceMarketContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Deleted = true;
                _context.Orders.Update(order);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa đơn hàng thành công!");
            return RedirectToAction(nameof(Index));
        }

    }
}
