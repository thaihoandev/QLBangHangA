using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBangHangA.Data_Access;
using QLBangHangA.Models.Entities;
using QLBangHangA.Models;

namespace QLBangHangA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminProductVariantValue : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }

        public AdminProductVariantValue(ApplicationDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;

        }

        // GET: Admin/AdminProductVariantValue/Create
        public IActionResult Create(int attributeId)
        {
            var attribute = _context.ProductAttribute.SingleOrDefault(x=>x.Id == attributeId);
            ViewBag.Attribute = attribute;
            return View();
        }

        // POST: Admin/AdminProductVariantValue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVariantValue productVariantValue, int attributeId)
        {
            if (ModelState.IsValid)
            {
                var attribute = _context.ProductAttribute
                    .Include(attr => attr.ProductVariantValues) // Đảm bảo rằng List<ProductVariantValue> được load
                    .SingleOrDefault(x => x.Id == attributeId);

                if (attribute != null)
                {
                    // Thêm productVariantValue vào List<ProductVariantValue> của attribute
                    attribute.ProductVariantValues.Add(productVariantValue);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();

                    _notifyService.Success("Tạo mới thành công!");
                    return RedirectToAction("details", "adminAttribute", new { Id = attributeId });
                }
                else
                {
                    _notifyService.Error("Không tìm thấy thuộc tính sản phẩm.");
                    return RedirectToAction("Index", "adminAttribute");
                }
            }

            _notifyService.Error("Dữ liệu không hợp lệ!");
            return View(productVariantValue);

        }

        // GET: Admin/AdminProductVariantValue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductVariantValues == null)
            {
                return NotFound();
            }

            var productVariantValue = await _context.ProductVariantValues.FindAsync(id);
            if (productVariantValue == null)
            {
                return NotFound();
            }
            return View(productVariantValue);
        }


        // GET: Admin/AdminProductVariantValue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductVariantValues == null)
            {
                return NotFound();
            }

            var productVariantValue = await _context.ProductVariantValues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productVariantValue == null)
            {
                return NotFound();
            }
            return View(productVariantValue);
        }

        // POST: Admin/AdminProductVariantValue/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVariantValue productVariantValue)
        {
            if (id != productVariantValue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productVariantValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductVariantValueExists(productVariantValue.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notifyService.Success("Cập nhật thành công!");

                return RedirectToAction(nameof(Index));
            }
            _notifyService.Success("Cập nhật thất bại!");

            return View(productVariantValue);
        }

        private bool ProductVariantValueExists(int id)
        {
            return (_context.ProductVariantValues?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
