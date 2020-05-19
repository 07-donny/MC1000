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
    public class SubSubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubSubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubSubCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubSubCategory.ToListAsync());
        }

        // GET: SubSubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSubCategory = await _context.SubSubCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subSubCategory == null)
            {
                return NotFound();
            }

            return View(subSubCategory);
        }

        // GET: SubSubCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubSubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BannerImage,SubCategoryId")] SubSubCategory subSubCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subSubCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subSubCategory);
        }

        // GET: SubSubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSubCategory = await _context.SubSubCategory.FindAsync(id);
            if (subSubCategory == null)
            {
                return NotFound();
            }
            return View(subSubCategory);
        }

        // POST: SubSubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BannerImage,SubCategoryId")] SubSubCategory subSubCategory)
        {
            if (id != subSubCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subSubCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubSubCategoryExists(subSubCategory.Id))
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
            return View(subSubCategory);
        }

        // GET: SubSubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subSubCategory = await _context.SubSubCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subSubCategory == null)
            {
                return NotFound();
            }

            return View(subSubCategory);
        }

        // POST: SubSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subSubCategory = await _context.SubSubCategory.FindAsync(id);
            _context.SubSubCategory.Remove(subSubCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubSubCategoryExists(int id)
        {
            return _context.SubSubCategory.Any(e => e.Id == id);
        }
    }
}
