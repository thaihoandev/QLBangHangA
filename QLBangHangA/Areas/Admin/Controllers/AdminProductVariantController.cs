using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLBangHangA.Data_Access;
using QLBangHangA.Models;
using QLBangHangA.Models.Entities;
using System;

namespace QLBangHangA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminProductVariantController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }

        public AdminProductVariantController(ApplicationDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;

        }
        // GET: Admin/AdminProductVariant/Create
        public IActionResult Create(int productId)
        {
            var product = _context.Products.SingleOrDefault(x => x.ProductId == productId);
            ViewBag.Product = product;
            ViewData["BangMau"] = new SelectList(_context.ProductVariantValues.Where(x => x.ProductAttribute.Name.ToLower() == "color"), "Id", "Value");
            ViewData["BangSize"] = new SelectList(_context.ProductVariantValues.Where(x => x.ProductAttribute.Name.ToLower() == "size"), "Id", "Value");
            return View();
        }

        // POST: Admin/AdminProductVariant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVariant productVariant, int productId)
        {
            if (ModelState.IsValid)
            {
                var product = _context.Products
                    .Include(attr => attr.ProductVariants) // Đảm bảo rằng List<ProductVariant> được load
                    .SingleOrDefault(x => x.ProductId == productId);

                if (product != null)
                {
                    // Thêm productVariant vào List<Products> của attribute
                    product.ProductVariants.Add(productVariant);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    _notifyService.Success("Tạo mới thành công!");
                    return RedirectToAction("details", "adminProduct", new { Id = productId });
                }
                else
                {
                    _notifyService.Error("Không tìm thấy sản phẩm.");
                    return RedirectToAction("Index", "adminProduct");
                }
            }

            _notifyService.Error("Dữ liệu không hợp lệ!");
            return View(productVariant);
        }
    }
}
