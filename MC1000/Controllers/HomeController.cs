using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MC1000.Models;
using System.Xml.Linq;
using MC1000.Data;
using System.ComponentModel;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace MC1000.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {

            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("en-GB");
            //Load products into DB
            XDocument xdocProduct = XDocument.Load("http://supermaco.starwave.nl/api/products");

            var products = xdocProduct.Descendants("Product");

            foreach (var product in products)
            {
                Product p = new Product
                {
                    EAN = product.Descendants("EAN").First().Value,
                    Title = product.Descendants("Title").First().Value,
                    Brand = product.Descendants("Brand").First().Value,
                    ShortDescription = product.Descendants("Shortdescription").First().Value,
                    FullDescription = product.Descendants("Fulldescription").First().Value,
                    Image = product.Descendants("Image").First().Value,
                    Weight = product.Descendants("Weight").First().Value,
                    Price = Decimal.Parse(product.Descendants("Price").First().Value, style, provider)
                };
                _context.Add(p);
            }
            //Products Loaded

            //Load Categories into DB
            XDocument xdocCategory = XDocument.Load("http://supermaco.starwave.nl/api/categories");

            var categories = xdocCategory.Descendants("Category");
            foreach (var category in categories)
            {
                Category c = new Category();
                c.Name = category.Descendants("Name").First().Value;

                //Load Subcategories to DB and link to Category
                var subCategories = category.Descendants("Subcategory");

                c.SubCategories = new List<SubCategory>();
                foreach (var sub in subCategories)
                {
                    SubCategory s = new SubCategory();
                    s.Name = sub.Descendants("Name").First().Value;
                    s.Category = c;
                    c.SubCategories.Add(s);

                    //Load SubSubcategories to DB and link to SubCategory
                    var subSubCategories = category.Descendants("Subsubcategory");

                    s.SubSubCategories = new List<SubSubCategory>();

                    foreach (var subsub in subSubCategories)
                    {
                        SubSubCategory u = new SubSubCategory();
                        u.Name = subsub.Descendants("Name").First().Value;
                        u.SubCategory = s;
                        s.SubSubCategories.Add(u);
                    }

                }
                _context.Add(c);
            }


            //Load promotions to DB
            XDocument xdocPromo = XDocument.Load("http://supermaco.starwave.nl/api/promotions");
            var promotions = xdocPromo.Descendants("Promotion");
            foreach (var promotion in promotions)
            {
                Promotion p = new Promotion();
                p.Title = promotion.Descendants("Title").First().Value;

                //Load discounts to DB and link to Promotion
                var discounts = promotion.Descendants("Discount");
                p.Discounts = new List<Discount>();
                foreach (var discount in discounts)
                {
                    Discount d = new Discount();
                    d.EAN = discount.Descendants("EAN").First().Value;
                    d.DiscountedPrice = Decimal.Parse(discount.Descendants("DiscountPrice").First().Value, style, provider);
                    d.ValidUntil = DateTime.Parse(discount.Descendants("ValidUntil").First().Value);
                    p.Discounts.Add(d);
                }
                _context.Add(p);
            }


            //Load deliveryslots into DB
            XDocument xdocDeliveryslot = XDocument.Load("http://supermaco.starwave.nl/api/deliveryslots");

            var deliveryslots = xdocDeliveryslot.Descendants("Deliveryslot");
            foreach (var deliveryslot in deliveryslots)
            {
                DeliverySlot d = new DeliverySlot();
                {
                    d.DeliveryDate = DateTime.Parse(deliveryslot.Descendants("Date").First().Value);

                    var timeslots = xdocDeliveryslot.Descendants("Timeslot");
                    d.TimeSlots = new List<TimeSlot>();
                    foreach (var timeslot in timeslots)
                    {
                        TimeSlot t = new TimeSlot();
                        t.StartTime = DateTime.Parse(deliveryslot.Descendants("StartTime").First().Value);
                        t.EndTime = DateTime.Parse(deliveryslot.Descendants("EndTime").First().Value);
                        t.Price = Decimal.Parse(deliveryslot.Descendants("Price").First().Value, style, provider);
                        d.TimeSlots.Add(t);
                    }
                }
                _context.Add(d);
            }
            //Deliveryslots Loaded

            _context.SaveChanges();
            return View(await _context.HomeBanner.OrderByDescending(x => x.Id).Take(1).ToListAsync());
    }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View(_context.CategorieBanner.OrderByDescending(x => x.Id).Take(1).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
