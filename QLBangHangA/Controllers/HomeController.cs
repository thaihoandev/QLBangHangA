using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBangHangA.Data_Access;
using QLBangHangA.Models;
using QLBangHangA.Models.Entities;
using System.Diagnostics;

namespace QLBangHangA.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ApplicationDbContext context)
        {
            _logger = logger;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if(_context.Categories != null)
            {
                ViewBag.Category = _context.Categories.ToList().Take(5);
                ViewBag.CategoryTop = _context.Categories.ToList().First();
                ViewBag.CategoryElement = _context.Categories.ToList().Skip(1).Take(4);
            }
            if (_context.Products != null)
            {
                ViewBag.Product = _context.Products.ToList().Take(12);
                ViewBag.Products = _context.Products.ToList();

                ViewBag.ProductBestSellers = _context.Products.Where(p => p.BestSellers == true).ToList().Take(3);
                ViewBag.ProductHomeFlag = _context.Products.Where(p => p.HomeFlag == true).ToList().Take(3);
                ViewBag.ProductActive = _context.Products.Where(p => p.Active == true).ToList().Take(3);
                ViewBag.ProductIns = _context.Products.Where(p => p.Active == true).ToList().Take(6);

            }
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
