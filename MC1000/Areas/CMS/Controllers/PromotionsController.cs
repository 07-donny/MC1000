using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MC1000.Data;
using MC1000.Models;

namespace MC1000.Areas.CMS.Controllers
{
    [Area("CMS")]
    public class PromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PromotionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Promotie beheer
        public async Task<IActionResult> Index()
        {
            return View(await _context.Promotion.ToListAsync());
        }

        // GET: CMS/Promotions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // GET: CMS/Promotions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CMS/Promotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(promotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(promotion);
        }

        // GET: CMS/Promotions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion.FindAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // POST: CMS/Promotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(promotion);
        }

        // GET: CMS/Promotions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // POST: CMS/Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotion = await _context.Promotion.FindAsync(id);
            _context.Promotion.Remove(promotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Discount beheer
        public async Task<IActionResult> Discounts(int? id) //index
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            List<Discount> discountList = new List<Discount>();
            foreach (var d in _context.Discount)
            {
                if (d.PromotionId == id)
                {
                    discountList.Add(d);
                }
            }

            return View("Discounts", discountList);
        }

        public IActionResult DiscountCreate()
        {
            return View();
        }
        //Wanneer ik een discount probeer te maken wordt het niet opgeslagen. En het lukt nog niet om de promotie te bereiken en op te slaan.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiscountCreate([Bind("Id,DiscountedPrice,ValidUntil,PromotionId,EAN")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discount);
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var promotion = await _context.Promotion.FindAsync(id);
        //    if (promotion == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(promotion);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Promotion promotion)
        //{
        //    if (id != promotion.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(promotion);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PromotionExists(promotion.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(promotion);
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var promotion = await _context.Promotion
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (promotion == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(promotion);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var promotion = await _context.Promotion.FindAsync(id);
        //    _context.Promotion.Remove(promotion);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool PromotionExists(int id)
        {
            return _context.Promotion.Any(e => e.Id == id);
        }
    }
}
