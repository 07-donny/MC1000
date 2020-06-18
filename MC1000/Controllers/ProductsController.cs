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

namespace MC1000.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
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
            // ignore dit
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
    }
}