using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MC1000.Data;
using MC1000.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MC1000.Areas.CMS.Controllers
{
    [Area("CMS")]
    [Authorize(Roles = "Admin, Redactie")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public NewsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CMS/News
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.News.Include(n => n.User);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: CMS/News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: CMS/News/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id");

            return View();
        }

        // POST: CMS/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Text,Date,UserId,UserName,AfbeeldingUrl")] News news, IFormFile Afbeelding)
        {
            if (ModelState.IsValid)
            {
                // Code voor het toevoegen van een afbeelding
                if (Afbeelding != null && Afbeelding.Length > 0)
                {
                    string fileGuid = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(Afbeelding.FileName);
                    string filePath = Url.Content($"wwwroot/uploads/images/nieuws/{fileGuid}{extension}");

                    news.AfbeeldingUrl = $"{fileGuid}{extension}";

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        Afbeelding.CopyTo(stream);
                    }
                }
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", news.UserId);
            ViewData["UserName"] = new SelectList(_context.Set<User>(), "UserName", "UserName");

            return View(news);
        }

        // GET: CMS/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", news.UserId);
            ViewData["UserName"] = new SelectList(_context.Set<User>(), "UserName", "UserName");

            return View(news);
        }

        // POST: CMS/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text,Date,UserId,UserName,AfbeeldingUrl")] News news, IFormFile Afbeelding)
        {
            if (id != news.Id)
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
                        string filePath = Url.Content($"wwwroot/uploads/images/nieuws/{fileGuid}{extension}");

                        news.AfbeeldingUrl = $"{fileGuid}{extension}";

                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            Afbeelding.CopyTo(stream);
                        }
                    }
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
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
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", news.UserId);
            ViewData["UserName"] = new SelectList(_context.Set<User>(), "UserName", "UserName");

            return View(news);
        }

        // GET: CMS/News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: CMS/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}