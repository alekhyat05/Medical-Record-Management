using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATPatients.Models;
using Microsoft.AspNetCore.Http;

namespace ATPatients.Controllers
{
    public class ATMedicationsController : Controller
    {
        private readonly PatientsContext _context;

        
        public ATMedicationsController(PatientsContext context)
        {
            _context = context;
        }

        // GET: ATMedications
        public async Task<IActionResult> Index(int? medicationTypeId,string? medicationName)
        {
            if (medicationTypeId == null && medicationName==null)
            {
                medicationTypeId = Convert.ToInt32(Request.Cookies["medicationTypeId"]);
                medicationName=Request.Cookies["medicationName"];
                if (medicationTypeId == 0 && medicationName == null)
                {
                    TempData["medicationData"] = "Please select an Id or name !";
                    return RedirectToAction("index", "ATMedicationType");
                }
            }
            else
            {
                if (medicationTypeId != null)
                {
                    Response.Cookies.Append("medicationTypeId", medicationTypeId.ToString(), new CookieOptions { Expires = DateTime.Today.AddDays(2) });
                }
                if(medicationName!=null)
                {
                    Response.Cookies.Append("medicationName", medicationName, new CookieOptions { Expires = DateTime.Today.AddDays(2) });
                }
                
            }
            
            if(medicationTypeId != 0)
            {
                var patientsContext = _context.Medication.Include(m => m.ConcentrationCodeNavigation).Include(m => m.DispensingCodeNavigation).Include(m => m.MedicationType).Where(m => m.MedicationTypeId == medicationTypeId).OrderBy(m => m.Name).ThenBy(m => m.Concentration);
                return View(await patientsContext.ToListAsync());
            }
            else
            {
                var patientsContext = _context.Medication.Include(m => m.ConcentrationCodeNavigation).Include(m => m.DispensingCodeNavigation).Include(m => m.MedicationType).Where(m => m.MedicationType.Name == medicationName).OrderBy(m => m.Name).ThenBy(m => m.Concentration);
                
                return View(await patientsContext.ToListAsync());
            }
            
          
        }

        // GET: ATMedications/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // GET: ATMedications/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name");
                ViewData["display"] = 0;
            }
            else
            {
                ViewData["MedicationTypeId"] = id;
                ViewData["display"] = 1;
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit.OrderBy(code=>code.ConcentrationCode), "ConcentrationCode", "ConcentrationCode");
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit.OrderBy(d=>d.DispensingCode), "DispensingCode", "DispensingCode");
            
            return View();
        }

        // POST: ATMedications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {

            if (ModelState.IsValid)
            {
                var duplication = _context.Medication.Include(m => m.DispensingCodeNavigation).Include(m => m.MedicationType).
                         Where(m => m.Name == medication.Name && m.Concentration == medication.Concentration && m.ConcentrationCode == medication.ConcentrationCode);
                if (!duplication.Any())
                {
                    _context.Add(medication);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["medicationData"] = "Please Enter the Unique Value";
                }
               
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: ATMedications/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                TempData["medicationData"] = "Please Enter Id";

                return RedirectToAction("Index");
                //return RedirectToAction("Index","ATMedicationType"); //when you want to go to another Controller's Index give it in second param
            }

            var medication = await _context.Medication.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // POST: ATMedications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            if (id != medication.Din)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.Din))
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
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnit, "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnit, "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationType, "MedicationTypeId", "Name", medication.MedicationTypeId);
            return View(medication);
        }

        // GET: ATMedications/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // POST: ATMedications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var medication = await _context.Medication.FindAsync(id);
            _context.Medication.Remove(medication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationExists(string id)
        {
            return _context.Medication.Any(e => e.Din == id);
        }
    }
}
