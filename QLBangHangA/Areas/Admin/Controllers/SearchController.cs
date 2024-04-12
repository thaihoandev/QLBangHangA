using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBangHangA.Data_Access;
using QLBangHangA.Models.Entities;

namespace QLBangHangA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Get: Search/FindProduct
        [HttpPost]
        public IActionResult FindResult(string keyword)
        {
            List<Product> lsAll = new List<Product>();
            List<Product> ls = new List<Product>();

            lsAll = _context.Products.AsNoTracking().Include(x => x.Category).ToList();

            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return PartialView("_ListProductSearchPartialView", lsAll);
            }
            ls = _context.Products.AsNoTracking().
                Include(x => x.Category)
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
                .Take(10)
                .ToList();

            if (ls == null)
            {
                return PartialView("_ListProductSearchPartialView", null);

            }
            else
            {
                return PartialView("_ListProductSearchPartialView", ls);

            }

        }
    }
}
