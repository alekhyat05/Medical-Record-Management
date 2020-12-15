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
    public class ATDiagnosisCategoriesController : Controller
    {
        private readonly PatientsContext _context;
        /// <summary>
        /// DB Is intialised in The below Constructor
        /// </summary>
        /// <param name="context"></param>
        public ATDiagnosisCategoriesController(PatientsContext context)
        {
            _context = context;
        }

        // GET: ATDiagnosisCategories
        /// <summary>
        /// To show all the records of Diagnosis Categories upon clicking Diagnosis Categories  in the menu Bar
        /// </summary>
        /// <returns>index view</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.DiagnosisCategory.ToListAsync());
        }

        // GET: ATDiagnosisCategories/Details/5
        /// <summary>
        /// Upon Click of Details .A details of selected id's(element's) complete record is displayed and Deatils view of Diagnosis Categories invoked
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }

            return View(diagnosisCategory);
        }

        // GET: ATDiagnosisCategories/Create
        /// <summary>
        /// on Click of create link a create view is invoked
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// The newly created record is inserted and the updated list is shown and redirected to Index page
        /// </summary>
        /// <param name="diagnosisCategory"></param>
        /// <returns></returns>
        // POST: ATDiagnosisCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DiagnosisCategory diagnosisCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosisCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diagnosisCategory);
        }

        // GET: ATDiagnosisCategories/Edit/5
        /// <summary>
        /// Id of sekected selected element is taken and Edit view is displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory.FindAsync(id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }
            return View(diagnosisCategory);
        }

        /// <summary>
        ///  based on Id and unit value is updated in DB and redirected to index page 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="diagnosisCategory"></param>
        /// <returns></returns>
        // POST: ATDiagnosisCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] DiagnosisCategory diagnosisCategory)
        {
            if (id != diagnosisCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosisCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisCategoryExists(diagnosisCategory.Id))
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
            return View(diagnosisCategory);
        }

        // GET: ATDiagnosisCategories/Delete/5
        /// <summary>
        /// upon Invoking Delete button based on id the DB  record is fetched and delete view is shown with selected record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }

            return View(diagnosisCategory);
        }
        /// <summary>
        /// upon click of Delete record is deletd and index view is returned
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: ATDiagnosisCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnosisCategory = await _context.DiagnosisCategory.FindAsync(id);
            _context.DiagnosisCategory.Remove(diagnosisCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisCategoryExists(int id)
        {
            return _context.DiagnosisCategory.Any(e => e.Id == id);
        }
    }
}
