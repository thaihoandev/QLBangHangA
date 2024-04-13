using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
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
    public class AdminBlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }

        public AdminBlogController(ApplicationDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;

        }
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var lsNews = _context.News
                .AsNoTracking()
                .OrderByDescending(x => x.PostId).Where(y=>y.Published == true);
            PagedList<News> models = new PagedList<News>(lsNews, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }
        public IActionResult Pending(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE;
            var lsNews = _context.News
                .AsNoTracking()
                .OrderByDescending(x => x.PostId).Where(y => y.Published == false);
            PagedList<News> models = new PagedList<News>(lsNews, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(models);
        }
        [HttpPost]
        public async Task<IActionResult> Confirmed(int postId)
        {
            var news = _context.News.FirstOrDefault(x=>x.PostId == postId);
            if (news == null)
            {
                return NotFound();
            }
            else
            {
                news.Published = true;
                _context.Update(news);
                await _context.SaveChangesAsync();
                _notifyService.Success("Confirm Success!");
                return RedirectToAction("Pending");
            }          
        }

        // GET: Admin/AdminBlog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: Admin/AdminBlog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Blog'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa bbài thành công!");

            return RedirectToAction(nameof(Index));
        }

    }
}
