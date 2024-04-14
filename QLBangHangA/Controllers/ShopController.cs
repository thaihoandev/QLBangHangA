using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using QLBangHangA.Data_Access;
using QLBangHangA.Extentions;
using QLBangHangA.Models;
using QLBangHangA.Models.Entities;
using QLBangHangA.Models.ViewModels;
using System.Drawing;

namespace QLBangHangA.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        public ShopController( IProductRepository productRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }
        public IActionResult Index(int? page, int catID=0)
        {
                var pageNumber = page == null || page <= 0 ? 1 : page.Value;
                var pageSize = Utilities.PAGE_SIZE_SHOP;

                List<Product> lsProducts = new List<Product>();
                if (catID != 0)
                {
                    lsProducts = _context.Products
                    .AsNoTracking()
                    .Where(x => x.CatId == catID)
                    .Include(x => x.Category)
                    .OrderByDescending(x => x.ProductId).ToList();
                }
                else
                {
                    lsProducts = _context.Products
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .OrderByDescending(x => x.ProductId).ToList();
                }
                PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

                IEnumerable<Category> category = _context.Categories.ToList();
                var viewModel = new SideBarShopVM
                {
                    Products = models,
                    Categories = category
                };
                ViewBag.CurrentPage = pageNumber;
                ViewBag.CurrentCatID = catID;
                ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", catID);
            

            return View(viewModel);
        }

        public IActionResult Detail(int id)
        {

            var colorList = _context.ProductVariantValues.Where(x => x.ProductAttribute.Name.ToLower() == "color").OrderByDescending(x => x.Value).ToList();
            var sizeList = _context.ProductVariantValues.Where(x => x.ProductAttribute.Name.ToLower() == "size").OrderByDescending(x => x.Value).ToList();
            List<ProductVariant>productVariant = _context.ProductVariants.Where(x => x.ProductId == id).ToList();

            List<int> colorIds = productVariant
                .Select(x => x.ColorId)
                .Where(id => id.HasValue) // Lọc ra các giá trị không null
                .Select(id => id.Value)   // Chuyển đổi từ Nullable<int> thành int
                .ToList();
            List<int> sizeIds = productVariant
                .Select(x => x.SizeId)
                .Where(id => id.HasValue) // Lọc ra các giá trị không null
                .Select(id => id.Value)   // Chuyển đổi từ Nullable<int> thành int
                .ToList();

            List<ProductVariantValue> filteredColorList = colorList.Where(color => colorIds.Contains(color.Id)).ToList();
            List<ProductVariantValue> filteredSizeList = sizeList.Where(size => sizeIds.Contains(size.Id)).ToList();

            ViewData["BangMau"] = new SelectList(filteredColorList, "Id", "Value");
            ViewData["BangSize"] = new SelectList(filteredSizeList, "Id", "Value");

            var productDetails = _context.Products.FirstOrDefault(x => x.ProductId == id);

            ViewBag.Category = _context.Categories.FirstOrDefault(x => x.CatId == productDetails.CatId).CatName;
            ViewBag.SameProduct = _context.Products.Where(x => x.CatId == productDetails.CatId).ToList().Take(4);

            ViewBag.Images = _context.ProductImages.Where(x=>x.ProductId == id).ToList();
            ViewBag.ListProductVariant = _context.ProductVariants.ToList();
            return View(productDetails);
        }

        public IActionResult Search(string? query, int? page) {
            var products = _context.Products.AsQueryable();
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utilities.PAGE_SIZE_SHOP;

            if (query != null)
            {
                products = products.Where(p => p.ProductName.Contains(query));
            }

            PagedList<Product> models = new PagedList<Product>(products, pageNumber, pageSize);
            IEnumerable<Category> category = _context.Categories.ToList();
            var viewModel = new SideBarShopVM
            {
                Products = models,
                Categories = category
            };
            return View(viewModel);
        }
    }
}
