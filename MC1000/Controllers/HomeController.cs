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

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CategoriesAsync()
        {
            return View(await _context.Category.ToListAsync());
        }

        public IActionResult Aanbiedingen()
        {
            ViewData["Promotions"] = LoadPromotion();
            ViewData["Discounts"] = LoadDiscounts();

            return View();
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

        private List<Discount> LoadDiscounts()
        {
            var discounts = _context.Discount;
            List<Discount> DList = new List<Discount>();
            foreach (var item in discounts)
            {
                Discount n = new Discount();
                n.EAN = item.EAN;
                n.DiscountedPrice = item.DiscountedPrice;
                n.ValidUntil = item.ValidUntil;
                DList.Add(n);
            }
            return DList;
        }
    }
}