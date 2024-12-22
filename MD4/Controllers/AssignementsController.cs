using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MD4;
using MD4.Data;

namespace MD4.Controllers
{
    public class AssignementsController : Controller
    {
        private readonly SchoolSystemContext _context;

        public AssignementsController(SchoolSystemContext context)
        {
            _context = context;
        }

        // GET: Assignements
        public async Task<IActionResult> Index()
        {
            var schoolSystemContext = _context.Assignement.Include(a => a.Course);
            return View(await schoolSystemContext.ToListAsync());
        }

        // GET: Assignements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignement = await _context.Assignement
                .Include(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignement == null)
            {
                return NotFound();
            }

            return View(assignement);
        }

        // GET: Assignements/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id");
            return View();
        }

        // POST: Assignements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Deadline,CourseId,Description")] Assignement assignement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", assignement.CourseId);
            return View(assignement);
        }

        // GET: Assignements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignement = await _context.Assignement.FindAsync(id);
            if (assignement == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", assignement.CourseId);
            return View(assignement);
        }

        // POST: Assignements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Deadline,CourseId,Description")] Assignement assignement)
        {
            if (id != assignement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignementExists(assignement.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", assignement.CourseId);
            return View(assignement);
        }

        // GET: Assignements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignement = await _context.Assignement
                .Include(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignement == null)
            {
                return NotFound();
            }

            return View(assignement);
        }

        // POST: Assignements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignement = await _context.Assignement.FindAsync(id);
            if (assignement != null)
            {
                _context.Assignement.Remove(assignement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignementExists(int id)
        {
            return _context.Assignement.Any(e => e.Id == id);
        }
    }
}
