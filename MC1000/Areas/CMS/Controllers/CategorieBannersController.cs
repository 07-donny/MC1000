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
    public class CategorieBannersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategorieBannersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CMS/CategorieBanners
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategorieBanner.ToListAsync());
        }

        // GET: CMS/CategorieBanners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorieBanner = await _context.CategorieBanner
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorieBanner == null)
            {
                return NotFound();
            }

            return View(categorieBanner);
        }

        // GET: CMS/CategorieBanners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CMS/CategorieBanners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titel,Afbeelding,AfbeeldingUrl")] CategorieBanner categorieBanner, IFormFile Afbeelding)
        {
            if (ModelState.IsValid)
            {
                // Code voor het toevoegen van een afbeelding
                if (Afbeelding != null && Afbeelding.Length > 0)
                {
                    string fileGuid = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(Afbeelding.FileName);
                    string filePath = Url.Content($"wwwroot/banners/{fileGuid}{extension}");

                    categorieBanner.AfbeeldingUrl = $"{fileGuid}{extension}";

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        Afbeelding.CopyTo(stream);
                    }
                }
                _context.Add(categorieBanner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categorieBanner);
        }

        // GET: CMS/CategorieBanners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorieBanner = await _context.CategorieBanner.FindAsync(id);
            if (categorieBanner == null)
            {
                return NotFound();
            }
            return View(categorieBanner);
        }

        // POST: CMS/CategorieBanners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titel,Afbeelding,AfbeeldingUrl")] CategorieBanner categorieBanner, IFormFile Afbeelding)
        {
            if (id != categorieBanner.Id)
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
                        string filePath = Url.Content($"wwwroot/banners/{fileGuid}{extension}");

                        categorieBanner.AfbeeldingUrl = $"{fileGuid}{extension}";

                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            Afbeelding.CopyTo(stream);
                        }
                    }
                    _context.Update(categorieBanner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorieBannerExists(categorieBanner.Id))
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
            return View(categorieBanner);
        }

        // GET: CMS/CategorieBanners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorieBanner = await _context.CategorieBanner
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorieBanner == null)
            {
                return NotFound();
            }

            return View(categorieBanner);
        }

        // POST: CMS/CategorieBanners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categorieBanner = await _context.CategorieBanner.FindAsync(id);
            _context.CategorieBanner.Remove(categorieBanner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategorieBannerExists(int id)
        {
            return _context.CategorieBanner.Any(e => e.Id == id);
        }
    }
}
