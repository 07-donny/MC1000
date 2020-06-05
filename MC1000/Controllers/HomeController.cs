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
using System.Dynamic;

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
            ViewData["Banners"] = LoadBanner();
            ViewData["News"] = LoadNews();
            ViewData["Promotions"] = LoadPromotion();

            //var producten = from a in _context.Product
            //                select a;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    producten = producten.Where(v => v.Title.Contains(searchString));
            //}

            return View();
        }

        public IActionResult Privacy()
        {
            var style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
            var provider = new CultureInfo("en-GB");
            //Load products into DB
            XDocument xdocProduct = XDocument.Load("http://supermaco.starwave.nl/api/products");

            var products = xdocProduct.Descendants("Product");
            foreach (var product in products)
            {
                var productEAN = product.Descendants("EAN").First().Value;
                if (!_context.Product.Any(u => u.EAN == productEAN))
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
            }
            //Products Loaded

            //Load Categories into DB
            XDocument xdocCategory = XDocument.Load("http://supermaco.starwave.nl/api/categories");

            var categories = xdocCategory.Descendants("Category");
            foreach (var category in categories)
            {
                var categoryName = category.Descendants("Name").First().Value;

                if (!_context.Category.Any(u => u.Name == categoryName))
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
            }
            //Load promotions to DB
            XDocument xdocPromo = XDocument.Load("http://supermaco.starwave.nl/api/promotions");
            var promotions = xdocPromo.Descendants("Promotion");
            foreach (var promotion in promotions)
            {
                Promotion p = new Promotion();
                p.Title = promotion.Descendants("Title").First().Value;
                if (!_context.Promotion.Any(u => u.Title == p.Title))
                {
                    //Load discounts to DB and link to Promotion
                    var discounts = promotion.Descendants("Discount");
                    p.Discounts = new List<Discount>();
                    foreach (var discount in discounts)
                    {
                        var discountedPrice = Decimal.Parse(discount.Descendants("DiscountPrice").First().Value, style, provider);
                        var discountValid = DateTime.Parse(discount.Descendants("ValidUntil").First().Value);
                        var EAN = discount.Descendants("EAN").First().Value;

                        if (!_context.Discount.Any(u => u.DiscountedPrice == discountedPrice &&
                        u.ValidUntil == discountValid && u.EAN == EAN))
                        {
                            Discount d = new Discount();
                            d.EAN = discount.Descendants("EAN").First().Value;
                            d.DiscountedPrice = Decimal.Parse(discount.Descendants("DiscountPrice").First().Value, style, provider);
                            d.ValidUntil = DateTime.Parse(discount.Descendants("ValidUntil").First().Value);
                            p.Discounts.Add(d);
                        }
                    }
                    _context.Add(p);
                }
            }


            //Load deliveryslots into DB
            XDocument xdocDeliveryslot = XDocument.Load("http://supermaco.starwave.nl/api/deliveryslots");

            var deliveryslots = xdocDeliveryslot.Descendants("Deliveryslot");
            foreach (var deliveryslot in deliveryslots)
            {
                var date = DateTime.Parse(deliveryslot.Descendants("Date").First().Value);

                if (!_context.DeliverySlot.Any(u => u.DeliveryDate == date))
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
            }
            //Deliveryslots Loaded

            _context.SaveChanges();
            return View();
        }

        public async Task<IActionResult> CategoriesAsync()
        {
            return View(await _context.Category.ToListAsync());
        }

        public async Task<IActionResult> SubCategories(int id)
        {
            return View(await _context.SubCategory.Where(sc => sc.CategoryId == id).ToListAsync());
        }

        public async Task<IActionResult> SubSubCategories(int id)
        {
            return View(await _context.SubSubCategory.Where(ssc => ssc.SubCategoryId == id).ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private List<News> LoadNews()
        {
            var newss = _context.News;
            List<News> NewsList = new List<News>();
            foreach (var item in newss)
            {
                News n = new News();
                n.Title = item.Title;
                n.Text = item.Text;
                NewsList.Add(n);
            }
            return NewsList;

        }

        private List<HomeBanner> LoadBanner()
        {
            var homebanner = _context.HomeBanner;
            List<HomeBanner> BannerList = new List<HomeBanner>();
            foreach (var item in homebanner)
            {
                HomeBanner h = new HomeBanner();
                h.Titel = item.Titel;
                h.AfbeeldingUrl = item.AfbeeldingUrl;
                BannerList.Add(h);
            }
            return BannerList;
        }
        private List<Promotion> LoadPromotion()
        {
            var promotion = _context.Promotion;
            List<Promotion> PromotionList = new List<Promotion>();
            foreach (var item in promotion)
            {
                Promotion p = new Promotion();
                p.Title = item.Title;
                PromotionList.Add(p);
            }
            return PromotionList;
        }

    }
}
