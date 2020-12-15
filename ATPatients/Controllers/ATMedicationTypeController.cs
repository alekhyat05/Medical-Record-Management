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
    public class ATMedicationTypeController : Controller
    {
        private readonly PatientsContext _context;

        /// <summary>
        /// DB Is intialised in The below Constructor
        /// </summary>
        /// <param name="context"></param>
        public ATMedicationTypeController(PatientsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// To show all the records of MedicationType upon clicking MedicationType  in the menu Bar
        /// </summary>
        /// <returns></returns>
        // GET: ATMedicationType
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicationType.OrderBy(m=>m.Name).ToListAsync());
        }

        /// <summary>
        /// Upon Click of Details .A details of selected id's(element's) complete record is displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ATMedicationType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        /// <summary>
        ///  on Click of create link a create view is invoked
        /// </summary>
        /// <returns></returns>
        // GET: ATMedicationType/Create
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// The newly created record is inserted and the updated list is shown and redirected to Index page
        /// </summary>
        /// <param name="medicationType"></param>
        /// <returns></returns>
        // POST: ATMedicationType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicationType);
        }

        // GET: ATMedicationType/Edit/5
        /// <summary>
        ///  Id of sekected selected element is taken and Edit view is displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType.FindAsync(id);
            if (medicationType == null)
            {
                return NotFound();
            }
            return View(medicationType);
        }

        /// <summary>
        ///  based on Id and unit value is updated in DB and redirected to index page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="medicationType"></param>
        /// <returns></returns>

        // POST: ATMedicationType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (id != medicationType.MedicationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationTypeExists(medicationType.MedicationTypeId))
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
            return View(medicationType);
        }
        /// <summary>
        ///  upon Invoking Delete button based on id the DB  record is fetched and delete view is shown with selected record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: ATMedicationType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationType
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }
        /// <summary>
        /// upon click of Delete record is deletd and index view is returned
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: ATMedicationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicationType = await _context.MedicationType.FindAsync(id);
            _context.MedicationType.Remove(medicationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationTypeExists(int id)
        {
            return _context.MedicationType.Any(e => e.MedicationTypeId == id);
        }
    }
}
