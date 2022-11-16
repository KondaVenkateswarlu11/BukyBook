using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitofWork _unitofWork;
		[BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }
		public int OrderTotal { get; set; }
		public CartController(IUnitofWork unitofWork)
		{
			_unitofWork = unitofWork;
		}

		public IActionResult Index()
		{
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM = new ShoppingCartVM()
			{
				ListCart = _unitofWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
				includeProperties: "Product"),

				OrderHeader = new()
			};
			foreach(var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
					cart.Product.Price50, cart.Product.Price100);

				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count); 
			}
            return View(ShoppingCartVM);
		}


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
			{
                ListCart = _unitofWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),

                OrderHeader = new()
			};
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofWork.ApplicationUser.GetFirstorDefault(
                u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddresss;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitofWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product");

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
          
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price,
                    cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            _unitofWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitofWork.Save();

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitofWork.OrderDetail.Add(orderDetail);
                _unitofWork.Save();
            }

            _unitofWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitofWork.Save();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Plus(int cartId)
		{
			var cart = _unitofWork.ShoppingCart.GetFirstorDefault(u => u.Id == cartId);
			_unitofWork.ShoppingCart.IncrementCount(cart, 1);
			_unitofWork.Save();
			return RedirectToAction(nameof(Index));
		}
        public IActionResult Minus(int cartId)
        {
            var cart = _unitofWork.ShoppingCart.GetFirstorDefault(u => u.Id == cartId);
			if (cart.Count <= 1)
			{
				_unitofWork.ShoppingCart.Remove(cart);
			}
			else
			{
				_unitofWork.ShoppingCart.DecrementCount(cart, 1);
			}
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitofWork.ShoppingCart.GetFirstorDefault(u => u.Id == cartId);
            _unitofWork.ShoppingCart.Remove(cart);
            _unitofWork.Save();
            return RedirectToAction(nameof(Index));
        }





        private double GetPriceBasedOnQuantity(double quantity,double price,double price50,double price100)
		{
			if(quantity <= 50)
			{
				return price;
			}
			else
			{
				if(quantity <= 100)
				{
					return price50;
				}
				return price100;
				
			}
		}
	}
}
