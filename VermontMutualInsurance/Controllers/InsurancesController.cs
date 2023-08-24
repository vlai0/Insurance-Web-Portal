using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VermontMutualInsurance.Data;
using VermontMutualInsurance.Models;

namespace VermontMutualInsurance.Controllers
{
    public class InsurancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InsurancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Insurances
        public async Task<IActionResult> Index()
        {
              return _context.Insurance != null ? 
                          View(await _context.Insurance.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Insurance'  is null.");
        }
        
        // GET: Insurances/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.Insurance != null ?
                        View() :
                        Problem("Entity set 'ApplicationDbContext.Insurance'  is null.");
        }

        // POST: Insurances/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return _context.Insurance != null ?
                        View("Index", await _context.Insurance.Where( i => i.Name.Contains(SearchPhrase)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Insurance'  is null.");
        }

        // GET: Insurances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: Insurances/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insurance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insurance);
        }

        // GET: Insurances/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            return View(insurance);
        }

        // POST: Insurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insurance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.Id))
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
            return View(insurance);
        }

        // GET: Insurances/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Insurance == null)
            {
                return NotFound();
            }

            var insurance = await _context.Insurance
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Insurances/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Insurance == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Insurance'  is null.");
            }
            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance != null)
            {
                _context.Insurance.Remove(insurance);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(int id)
        {
          return (_context.Insurance?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
