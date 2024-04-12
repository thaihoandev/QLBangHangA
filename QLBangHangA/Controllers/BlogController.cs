using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using QLBangHangA.Data_Access;
using QLBangHangA.Extentions;
using QLBangHangA.Models;
using QLBangHangA.Models.Entities;

namespace QLBangHangA.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }
        public BlogController(ApplicationDbContext context, INotyfService notyfService)
        {
            _notifyService = notyfService;
            _context = context;
        }
        public IActionResult Index(int ? page)
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


        // GET: Blog/Create
        public IActionResult Create()
        {
            return View();
        }
        // Xử lý thêm bài viết mới
        [HttpPost]
        public async Task<IActionResult> Create(News news, IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    string extention = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(news.Title) + extention;
                    news.Thumb = await Utilities.UploadFile(fThumb, @"blogs", image.ToLower());
                }
                if (string.IsNullOrEmpty(news.Thumb)) news.Thumb = "default.jpg";

                news.Alias = Utilities.SEOUrl(news.Title);
                news.CreateDate = DateTime.Now;
                news.Published = false;
                _context.Add(news);
                await _context.SaveChangesAsync();
                _notifyService.Success("Gửi bài viết thành công!");
                return RedirectToAction(nameof(Index));
            }
            _notifyService.Success("Gửi bài viết thất bại!");

            return View(news);
        }

        [Route("/blog/{Alias}--{id}.html", Name = "BlogDetails")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.AsNoTracking()
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (news == null)
            {
                return NotFound();
            }

            ViewBag.News5 = _context.News.Where(x=>x.Published == true).ToList().Take(5);
            ViewBag.News4 = _context.News.Where(x => x.Published == true).ToList().Take(4);

            return View(news);
        }
    }
}
