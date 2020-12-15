using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATPatients.Models;

namespace ATPatients.Controllers
{
    public class ATDispensingUnitController : Controller
    {
        private readonly PatientsContext _context;
        /// <summary>
        /// DB Is intialised in The below Constructor
        /// </summary>
        /// <param name="context"></param>
        public ATDispensingUnitController(PatientsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// To show all the records of DispensingUnit upon clicking DispensingUnit  in the menu Bar
        /// </summary>
        /// <returns></returns>
        // GET: ATDispensingUnit
        public async Task<IActionResult> Index()
        {
            return View(await _context.DispensingUnit.ToListAsync());
        }

        // GET: ATDispensingUnit/Details/5
        /// <summary>
        /// Upon Click of Details .A details of selected id's(element's) complete record is displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit
                .FirstOrDefaultAsync(m => m.DispensingCode == id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }

            return View(dispensingUnit);
        }

        // GET: ATDispensingUnit/Create
        /// <summary>
        ///  on Click of create link a create view is invoked
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// The newly created record is inserted and the updated list is shown and redirected to Index page
        /// </summary>
        /// <param name="dispensingUnit"></param>
        /// <returns></returns>

        // POST: ATDispensingUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DispensingCode")] DispensingUnit dispensingUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispensingUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispensingUnit);
        }

        // GET: ATDispensingUnit/Edit/5
        /// <summary>
        ///  Id of sekected selected element is taken and Edit view is displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit.FindAsync(id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }
            return View(dispensingUnit);
        }

        // POST: ATDispensingUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        ///   based on Id and unit value is updated in DB and redirected to index page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dispensingUnit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DispensingCode")] DispensingUnit dispensingUnit)
        {
            if (id != dispensingUnit.DispensingCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispensingUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispensingUnitExists(dispensingUnit.DispensingCode))
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
            return View(dispensingUnit);
        }

        // GET: ATDispensingUnit/Delete/5
        /// <summary>
        /// upon Invoking Delete button based on id the DB  record is fetched and delete view is shown with selected record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dispensingUnit = await _context.DispensingUnit
                .FirstOrDefaultAsync(m => m.DispensingCode == id);
            if (dispensingUnit == null)
            {
                return NotFound();
            }

            return View(dispensingUnit);
        }

        /// <summary>
        /// upon click of Delete record is deletd and index view is returned
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: ATDispensingUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dispensingUnit = await _context.DispensingUnit.FindAsync(id);
            _context.DispensingUnit.Remove(dispensingUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DispensingUnitExists(string id)
        {
            return _context.DispensingUnit.Any(e => e.DispensingCode == id);
        }
    }
}
