using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebsiteGenZ.Models;
using WebsiteGenZ.Models.ViewModels;
using WebsiteGenZ.Repository;

namespace WebsiteGenZ.Controllers
{
    public class CheckoutController : Controller
    {

        private readonly DataContext _dataContext;
        private UserManager<AppUser> _userManager;

        public CheckoutController(DataContext dataContext, UserManager<AppUser> userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        // GET: CheckoutController
        public ActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var addresses = _dataContext.Addresses.Where(addr => addr.UserId.Equals(userId)).ToList();
            var shippingMethods = _dataContext.ShippingMethods.ToList();

            var Cart = _dataContext.Carts.FirstOrDefault(cart => cart.UserId.Equals(userId));

            var cartItems = Cart.CartItems = _dataContext.CartItems.Where(item => Cart.Id.Equals(item.CartId)).ToList(); 

            var model = new CheckoutViewModel
            {
                Addresses = addresses,
                ShippingMethods = shippingMethods,
                CartItems = cartItems
            };

            return View(model);
        }



        // GET: CheckoutController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckoutController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewAddress(AddressModel address)
        {
            try
            {
                _dataContext.Add(address);
                _dataContext.SaveChanges();
                return RedirectToAction("Index", "Address");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckoutController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckoutController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckoutController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckoutController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
