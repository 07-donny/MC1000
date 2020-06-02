using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MC1000.Data;
using MC1000.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MC1000.Areas.CMS
{
    [Area("CMS")]
    public class HomeBannersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeBannersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CMS/HomeBanners
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeBanner.ToListAsync());
        }

        // GET: CMS/HomeBanners/Details/5
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

        // GET: CMS/HomeBanners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CMS/HomeBanners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titel,Afbeelding,AfbeeldingUrl")] HomeBanner homeBanner, IFormFile Afbeelding)
        {
            if (ModelState.IsValid)
            {
                // Code voor het toevoegen van een afbeelding
                if (Afbeelding != null && Afbeelding.Length > 0)
                {
                    string fileGuid = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(Afbeelding.FileName);
                    string filePath = Url.Content($"wwwroot/banners/{fileGuid}{extension}");

                    homeBanner.AfbeeldingUrl = $"{fileGuid}{extension}";

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        Afbeelding.CopyTo(stream);
                    }
                }
                _context.Add(homeBanner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeBanner);
        }

        // GET: CMS/HomeBanners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeBanner = await _context.HomeBanner.FindAsync(id);
            if (homeBanner == null)
            {
                return NotFound();
            }
            return View(homeBanner);
        }

        // POST: CMS/HomeBanners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titel,Afbeelding,AfbeeldingUrl")] HomeBanner homeBanner, IFormFile Afbeelding)
        {
            if (id != homeBanner.Id)
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

                        homeBanner.AfbeeldingUrl = $"{fileGuid}{extension}";

                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            Afbeelding.CopyTo(stream);
                        }
                    }
                    _context.Update(homeBanner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeBannerExists(homeBanner.Id))
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
            return View(homeBanner);
        }

        // GET: CMS/HomeBanners/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: CMS/HomeBanners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeBanner = await _context.HomeBanner.FindAsync(id);
            _context.HomeBanner.Remove(homeBanner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeBannerExists(int id)
        {
            return _context.HomeBanner.Any(e => e.Id == id);
        }
    }
}
