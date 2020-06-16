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
    public class DeliverySlotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliverySlotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DeliverySlots
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeliverySlot.ToListAsync());
        }

        // GET: DeliverySlots/TimeSlot/5
        public async Task<IActionResult> TimeSlot(int? id)
        {
            return View(await _context.TimeSlot.Where(t => t.DeliverySlotId == id).ToListAsync());
        }

        private bool DeliverySlotExists(int id)
        {
            return _context.DeliverySlot.Any(e => e.Id == id);
        }
    }
}
