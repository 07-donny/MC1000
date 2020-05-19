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

namespace MC1000.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportXml()
        {
            XDocument xdoc = XDocument.Load("http://supermaco.starwave.nl/api/products");

            var categorys = xdoc.Descendants("Category").Select(a => a.Value).Distinct();
            foreach(var category in categorys)
            {
                var existing = _context.Category.FirstOrDefault(a => a.Name == category);
                if (existing == null)
                {
                    Category c = new Category();
                    c.Name = category;
                    _context.Add(c);
                }
            }

            var subcategorys = xdoc.Descendants("Subcategory").Select(a => a.Value).Distinct();
            foreach (var subcategory in subcategorys)
            {
                var existing = _context.SubCategory.FirstOrDefault(a => a.Name == subcategory);
                if (existing == null)
                {
                    SubCategory c = new SubCategory();
                    c.Name = subcategory;
                    _context.Add(c);
                }
            }

            var subsubcategorys = xdoc.Descendants("Subsubcategory").Select(a => a.Value).Distinct();
            foreach (var subsubcategory in subsubcategorys)
            {
                var existing = _context.SubSubCategory.FirstOrDefault(a => a.Name == subsubcategory);
                if (existing == null)
                {
                    SubSubCategory c = new SubSubCategory();
                    c.Name = subsubcategory;
                    _context.Add(c);
                }
            }

            var products = xdoc.Descendants("Product");

            foreach(var product in products)
            {
                Product p = new Product();

                p.EAN = product.Descendants("EAN").First().Value;
                p.Title = product.Descendants("Title").First().Value;
                p.Brand = product.Descendants("Brand").First().Value;
                p.ShortDescription = product.Descendants("Shortdescription").First().Value;
                p.FullDescription = product.Descendants("Fulldescription").First().Value;
                p.Image = product.Descendants("Image").First().Value;
                p.Weight = product.Descendants("Weight").First().Value;
                p.Price = Decimal.Parse(product.Descendants("Price").First().Value);

                var category = product.Descendants("Category").First().Value;
                var existing1 = _context.Category.FirstOrDefault(a => a.Name == category);
                p.Category = existing1;

                var subcategory = product.Descendants("Subcategory").First().Value;
                var existing2 = _context.SubCategory.FirstOrDefault(a => a.Name == subcategory);
                p.SubCategory = existing2;

                var subsubcategory = product.Descendants("Subsubcategory").First().Value;
                var existing3 = _context.SubSubCategory.FirstOrDefault(a => a.Name == subsubcategory);
                p.SubSubCategory = existing3;

                _context.Add(p);
            }

            _context.SaveChanges();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
