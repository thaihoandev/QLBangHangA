using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBangHangA.Data_Access;
using QLBangHangA.Extentions;
using QLBangHangA.Models;
using QLBangHangA.Models.Entities;

namespace QLBangHangA.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(ApplicationDbContext context,UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _userManager = userManager;
        }

        

        public IActionResult AddToCart(ProductVariant productVariant, int productId, int quantity=0, int colorId =0, int sizeId=0)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var product = GetProductFromDatabase(productId);
            
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();

            var sizeProValue = _context.ProductVariantValues.FirstOrDefault(item => item.Id == sizeId).Id;
            var colorProValue = _context.ProductVariantValues.FirstOrDefault(item => item.Id == colorId).Id;


            // 3. Find existing cart item for this product (if any)
            var existingCartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId && item.ColorId == colorId && item.SizeId == sizeId);
            // 4. Handle different scenarios:
            if (existingCartItem == null  )
            {
                // Product not in cart yet: Create a new CartItem
                int sumItem = cart.Items.Count();
                var cartItem = new CartItem
                {
                    Id = sumItem++,
                    ProductId = productId,
                    Name = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity,
                    SizeId = sizeProValue,
                    ColorId = colorProValue

                };
                cart.AddItem(cartItem);
                
            } 
            else
            {
                // Product already in cart: Update quantity
                existingCartItem.Quantity += quantity;

                // Check for potential overflow (quantity might become negative)
                if (existingCartItem.Quantity < 1)
                {
                    existingCartItem.Quantity = 1; // Set minimum quantity to 1
                }
            }
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var productsWithVariants = _context.Products.Include(p => p.ProductVariants).ToList();
            ViewBag.productList = productsWithVariants;
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            ViewBag.Color = _context.ProductVariantValues.Where(x => x.ProductAttribute.Name.ToLower() == "color");
            ViewBag.Size = _context.ProductVariantValues.Where(x => x.ProductAttribute.Name.ToLower() == "size");

            return View(cart);
        }
        // Các actions khác...
        private Product GetProductFromDatabase(int productId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
            var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            // Kiểm tra null
            if (product == null)
            {
                return null;
            }

            // Trả về sản phẩm
            return product;
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart =
           HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);// Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if(cart != null)
            {
                // Access and process list items from cart.Items
                List<CartItem> cartItems = cart.Items;

                // Example: Iterate through cart items and display or process data
                foreach (var item in cartItems)
                {
                    string productName = item.Name;
                    int quantity = item.Quantity;
                    decimal price = item.Price;

                    // ... (Process or display item data)
                }

                ViewBag.CartOrder = cartItems; 
            }
            return View(new Order());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("Index");
            }
            var user = await _userManager.GetUserAsync(User);
            var pending = _context.TransactStatuses.SingleOrDefault(x => x.Status.ToLower() == "pending");

            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TransactStatusId = pending.TransactStatusId;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price,
                
            }).ToList();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return View("OrderCompleted", order.OrderId); // Trang xác nhận hoàn thành đơn hàng
        }
    }
}




