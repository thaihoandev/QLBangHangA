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
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService _notifyService { get; }
        public AdminProductController(ApplicationDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;
        }
        public IActionResult Index(int? page = 0, int catID = 0)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

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
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentCatID = catID;
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", catID);
            ViewBag.ListProductVariant = _context.ProductVariants.ToList();
            return View(models);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            

            return View();
        }
        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile fThumb, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                if (fThumb != null)
                {
                    
                    string extention = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(product.ProductName) + extention;
                    product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                }
                if (images != null)
                {
                    product.Images = new List<ProductImage>();
                    int i = 1;
                    foreach (var item in images)
                    {
                        
                        string extention = Path.GetExtension(item.FileName);
                        string image = Utilities.SEOUrl(product.ProductName)+"-"+ i + extention;
                        ProductImage imagePro = new ProductImage()
                        {
                            ProductId = product.ProductId,
                            Url = await Utilities.UploadFile(item, @"products", image.ToLower())
                        };
                        i++;
                        product.Images.Add(imagePro);
                    }
                }

                if (string.IsNullOrEmpty(product.Thumb)) product.Thumb = "default.jpg";

                product.Alias = Utilities.SEOUrl(product.ProductName);
                product.DateCreated = DateTime.Now;
                product.DateModified = DateTime.Now;


                _context.Add(product);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo mới sản phẩm thành công!");
                return RedirectToAction(nameof(Index));
            }



            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);

            _notifyService.Error("Tạo mới sản phẩm thất bại!");

            return View(product);
        }


        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Product product, IFormFile? fThumb, List<IFormFile> images)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extention = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(product.ProductName) + extention;
                        product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.Thumb)) product.Thumb = "default.jpg";

                    if (images != null)
                    {
                        product.Images = new List<ProductImage>();
                        foreach (var item in images)
                        {
                            int i = 1;
                            string extention = Path.GetExtension(item.FileName);
                            string image = Utilities.SEOUrl(product.ProductName) + "-" + i + extention;
                            ProductImage imagePro = new ProductImage()
                            {
                                ProductId = product.ProductId,
                                Url = await Utilities.UploadFile(item, @"products", image.ToLower())
                            };
                            i++;
                            product.Images.Add(imagePro);
                        }
                    }

                    product.Alias = Utilities.SEOUrl(product.ProductName);

                    product.DateModified = DateTime.Now;

                    _context.Update(product);
                    _notifyService.Success("Cập nhật sản phẩm thành công!");
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            _notifyService.Error("Cập nhật sản phẩm thất bại!");
            return View(product);
        }

        public IActionResult Filter(int CatID = 0)
        {

            var url = $"/admin/adminProduct?catID={CatID}";
            if (CatID == 0)
            {
                url = $"/admin/AdminProduct";

            }
            return Json(new { status = "success", redirectUrl = url });
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            ViewData["BangMau"] = new SelectList(_context.ProductVariantValues.Where(x => x.ProductAttribute.Name.ToLower() == "color"), "Id", "Value");
            ViewData["BangSize"] = new SelectList(_context.ProductVariantValues.Where(x => x.ProductAttribute.Name.ToLower() == "size"), "Id", "Value");

            ViewBag.ItemList = _context.ProductVariants.Where(x=>x.ProductId == id).ToList();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DbEcommerceMarketContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa sản phẩm thành công!");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }


    }

    

}
