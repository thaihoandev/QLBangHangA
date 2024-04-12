using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBangHangA.Data_Access;
using QLBangHangA.Extentions;
using QLBangHangA.Models;
using QLBangHangA.Models.Entities;

namespace QLBangHangA.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminAttributeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notifyService { get; }

        public AdminAttributeController(ApplicationDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService = notyfService;

        }
        public IActionResult Index()
        {
            var attribute = _context.ProductAttribute.ToList();

            return View(attribute);
        }


        // GET: Admin/AdminAttribute/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminAttribute/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductAttribute productAttribute)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productAttribute);
                await _context.SaveChangesAsync();
                _notifyService.Success("Tạo mới thông số thành công!");

                return RedirectToAction(nameof(Index));
            }
            _notifyService.Success("Tạo mới thông số thất bại!");

            return View(productAttribute);
        }

        // GET: Admin/AdminAttribute/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductAttribute == null)
            {
                return NotFound();
            }

            var attribute = await _context.ProductAttribute.FindAsync(id);
            if (attribute == null)
            {
                return NotFound();
            }
            return View(attribute);
        }


        // GET: Admin/AdminAttribute/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductAttribute == null)
            {
                return NotFound();
            }

            var attribute = await _context.ProductAttribute
                .FirstOrDefaultAsync(m => m.Id == id);

            var variantList = _context.ProductVariantValues.Where(x=>x.ProductAttributeId == id).ToList();
            if (attribute == null)
            {
                return NotFound();
            }
            ViewBag.ProductVariantValue = variantList;
            return View(attribute);
        }

        // POST: Admin/AdminAttribute/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductAttribute productAttribute)
        {
            if (id != productAttribute.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productAttribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttributeExists(productAttribute.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notifyService.Success("Cập nhật thông số thành công!");

                return RedirectToAction(nameof(Index));
            }
            _notifyService.Success("Cập nhật thông số thất bại!");

            return View(productAttribute);
        }

        private bool AttributeExists(int id)
        {
            return (_context.ProductAttribute?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
