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



    }
}
