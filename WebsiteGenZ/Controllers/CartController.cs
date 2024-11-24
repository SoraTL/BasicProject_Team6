using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteGenZ.Models.ViewModels;
using WebsiteGenZ.Models;
using WebsiteGenZ.Repository;
using Microsoft.AspNetCore.Identity;

namespace WebsiteGenZ.Controllers
{
    public class CartController : Controller
    {   
        private DataContext _dataContext;

        private UserManager<AppUser> _userManager;

        public CartController(DataContext dataContext, UserManager<AppUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 10;

            // Lấy userId của người dùng hiện tại
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized("User not logged in.");
            }

            // Tìm giỏ hàng theo userId
            var cart = await _dataContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                ViewBag.Message = "Your cart is empty!";
                return View(new CartViewModel
                {
                    UserId = userId,
                    CartItems = new List<CartItemModel>(),
                    Products = Enumerable.Empty<ProductModel>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    PageSize = pageSize
                });
            }

            // Lấy các mục giỏ hàng liên quan
            var cartItems = await _dataContext.CartItems
                .Where(ci => ci.CartId == cart.Id)
                .Include(ci => ci.Product)
                .ToListAsync();

            // Tính toán phân trang
            var totalItems = cartItems.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedCartItems = cartItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            // Tạo ViewModel
            var viewModel = new CartViewModel
            {
                UserId = userId,
                CartItems = pagedCartItems.ToList(),
                Products = pagedCartItems.Select(ci => ci.Product),
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized("You must be logged in to add items to the cart.");
            }

            var cart = await _dataContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new CartModel
                {
                    UserId = userId
                };
                _dataContext.Carts.Add(cart);
                await _dataContext.SaveChangesAsync();
            }

            var existingCartItem = await _dataContext.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var newCartItem = new CartItemModel
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                _dataContext.CartItems.Add(newCartItem);
            }

            await _dataContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
