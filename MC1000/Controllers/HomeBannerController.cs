using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MC1000.Data;
using MC1000.Models;

namespace MC1000.Controllers
{
    public class HomeBannerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeBannerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HomeBanner
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeBanner.ToListAsync());
        }

        // GET: HomeBanner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeBanner = await _context.HomeBanner
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeBanner == null)
            {
                return NotFound();
            }

            return View(homeBanner);
        }
    }
}
