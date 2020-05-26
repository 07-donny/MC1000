﻿using System;
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

namespace MC1000.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
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
                    Price = Decimal.Parse(product.Descendants("Price").First().Value)
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
