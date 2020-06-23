using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MC1000.Data;
using MC1000.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MC1000.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subsub = _context.SubSubCategory.FirstOrDefault(s => s.Id == id);

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.SubSub == subsub.Name);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult ProductList(string searchString)
        {
            var products = from a in _context.Product
                           select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(v => v.Title.Contains(searchString));
            }

            return View(products);
        }

        public IActionResult AddToCart(int id)
        {
            List<CartItem> cart = new List<CartItem>();
            var cartStr = HttpContext.Session.GetString("cart");
            if (cartStr != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cartStr);
            }

            CartItem i = new CartItem { ProductId = id, Amount = 1 };
            cart.Add(i);

            cartStr = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", cartStr);

            return RedirectToAction("ShowCart");
        }

        public IActionResult ShowCart()
        {
            List<CartItem> cart = new List<CartItem>();
            var cartStr = HttpContext.Session.GetString("cart");
            if (cartStr != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cartStr);
            }
            List<CartItemViewModel> civm = (from product in _context.Product.AsEnumerable()
                                            join cartItem in cart
                                            on product.Id equals cartItem.ProductId
                                            select new CartItemViewModel
                                            {
                                                Amount = cartItem.Amount,
                                                Title = product.Title,
                                                Price = product.Price,
                                                ProductId = cartItem.ProductId,
                                                Image = product.Image
                                            }).ToList();

            return View(civm);
        }

        public async Task<IActionResult> PlaceOrder(int id)
        {
            List<CartItem> cart = new List<CartItem>();
            var cartStr = HttpContext.Session.GetString("cart");
            if (cartStr != null)
            {
                //cart is een list met een amount en productId
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cartStr);
            }

            Order o = new Order();
            o.DatePlaced = DateTime.Now;
            o.Status = "Verwerken";
            o.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _userManager.FindByIdAsync(o.UserId).Result;
            o.User = currentUser;
            o.TimeSlotId = id;

            List<OrderLine> orderLineList = new List<OrderLine>();

            decimal totalPrice = new decimal();

            foreach (var item in cart)
            {
                OrderLine ol = new OrderLine();
                ol.ProductId = item.ProductId;
                ol.Amount = item.Amount;
                ol.Product = _context.Product.Where(p => p.Id == item.ProductId).FirstOrDefault();
                totalPrice += ol.Product.Price * item.Amount;
                orderLineList.Add(ol);
            }

            o.OrderLines = orderLineList;
            o.TotalPrice = totalPrice;

            ViewData["DeliverySlots"] = LoadDSlots();

            _context.Add(o);
            _context.SaveChanges();

            //return RedirectToAction("Details", "Orders", o.Id);   //this should work, but it doesn't. I cry.
            return Redirect("/Orders/Details/" + o.Id); //Dan maar zo
        }

        public IActionResult IncreaseAmount(int id)
        {
            List<CartItem> cart = new List<CartItem>();
            var cartStr = HttpContext.Session.GetString("cart");
            if (cartStr != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cartStr);
            }
            var product = cart.FirstOrDefault(p => p.ProductId == id);
            product.Amount++;

            cartStr = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", cartStr);

            return RedirectToAction("ShowCart");
        }

        public IActionResult DecreaseAmount(int id)
        {
            List<CartItem> cart = new List<CartItem>();
            var cartStr = HttpContext.Session.GetString("cart");
            if (cartStr != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cartStr);
            }
            var product = cart.FirstOrDefault(p => p.ProductId == id);
            if (product.Amount <= 1)
            {
                product.Amount = 1;
            }
            else
            {
                product.Amount--;
            }

            cartStr = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", cartStr);

            return RedirectToAction("ShowCart");
        }

        public IActionResult DeleteFromCart(int id)
        {
            List<CartItem> cart = new List<CartItem>();
            var cartStr = HttpContext.Session.GetString("cart");
            if (cartStr != null)
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(cartStr);
            }
            var product = cart.FirstOrDefault(p => p.ProductId == id);

            cart.Remove(product);

            cartStr = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", cartStr);

            return RedirectToAction("ShowCart");
        }

        private List<DeliverySlot> LoadDSlots()
        {
            var deliveries = _context.DeliverySlot;
            List<DeliverySlot> DList = new List<DeliverySlot>();
            foreach (var item in deliveries)
            {
                DeliverySlot d = new DeliverySlot();
                d.DeliveryDate = item.DeliveryDate;
                List<TimeSlot> timelist = new List<TimeSlot>();

                foreach (var time in _context.TimeSlot)
                {
                    if (time.DeliverySlotId == item.Id)
                    {
                        timelist.Add(time);
                    }
                }
                d.TimeSlots = timelist;

                DList.Add(d);
            }
            return DList;
        }
    }
}