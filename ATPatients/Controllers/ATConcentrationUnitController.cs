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
    public class ATConcentrationUnitController : Controller
    {
        private readonly PatientsContext _context;


        /// <summary>
        /// DB Is intialised in The below Constructor
        /// </summary>
        /// <param name="context"></param>
        public ATConcentrationUnitController(PatientsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// To show all the records of Concentration Unit upon clicking Concentration Unit in the menu Bar
        /// </summary>
        /// <returns>index view</returns>
        // GET: ATConcentrationUnit
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConcentrationUnit.ToListAsync());
        }

        // GET: ATConcentrationUnit/Details/5
        /// <summary>
        /// Upon Click of Details .A details of selected id's(element's) complete record is displayed and Deatils view invoked
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // GET: ATConcentrationUnit/Create
        /// <summary>
        /// on Click of create link a create view is invoked
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: ATConcentrationUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The newly created record is inserted and the updated list is shown and redirected to Index page
        /// </summary>
        /// <param name="concentrationUnit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concentrationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concentrationUnit);
        }

        // GET: ATConcentrationUnit/Edit/5
        /// <summary>
        /// Id of sekected selected element is taken and Edit view is displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }
            return View(concentrationUnit);
        }

        // POST: ATConcentrationUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// based on Id and unit value is updated in DB and redirected to index page 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="concentrationUnit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (id != concentrationUnit.ConcentrationCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concentrationUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcentrationUnitExists(concentrationUnit.ConcentrationCode))
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
            return View(concentrationUnit);
        }
        /// <summary>
        /// upon Invoking Delete button based on id the DB  record is fetched and delete view is shown with selected record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ATConcentrationUnit/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnit
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        /// <summary>
        /// upon click of Delete record is deletd and index view is returned
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: ATConcentrationUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var concentrationUnit = await _context.ConcentrationUnit.FindAsync(id);
            _context.ConcentrationUnit.Remove(concentrationUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Filters and finds Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ConcentrationUnitExists(string id)
        {
            return _context.ConcentrationUnit.Any(e => e.ConcentrationCode == id);
        }
    }
}
