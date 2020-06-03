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
using System.IO;

namespace MC1000.Areas.CMS.Controllers
{
    [Area("CMS")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private object homeBanner;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CMS/Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // GET: CMS/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: CMS/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BannerImage")] Category category, IFormFile Afbeelding)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Afbeelding != null && Afbeelding.Length > 0)
                    {
                        string fileGuid = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(Afbeelding.FileName);
                        string filePath = Url.Content($"wwwroot/banners/categories/{fileGuid}{extension}");

                        category.BannerImage = $"{fileGuid}{extension}";

                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            Afbeelding.CopyTo(stream);
                        }
                    }
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
