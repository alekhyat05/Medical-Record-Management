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
    public class ATPatientController : Controller
    {
        private readonly PatientsContext _context;

        public ATPatientController(PatientsContext context)
        {
            _context = context;
        }

        // GET: ATPatient
        public async Task<IActionResult> Index()
        {
            var patientsContext =await _context.Patient.Include(p => p.ProvinceCodeNavigation).OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToListAsync();           
            return View(patientsContext);
        }

        // GET: ATPatient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: ATPatient/Create
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode");
            return View();
        }

        // POST: ATPatient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,FirstName,LastName,Address,City,ProvinceCode,PostalCode,Ohip,DateOfBirth,Deceased,DateOfDeath,HomePhone,Gender")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Patient has been added successfully";
                    return RedirectToAction(nameof(Index));
                }catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
                
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", patient.ProvinceCode);
            return View(patient);
        }

        // GET: ATPatient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
           ViewData["ProvinceCode"] = new SelectList(_context.Province.OrderBy(m=>m.Name), "ProvinceCode", "Name", patient.ProvinceCode);
         return View(patient);
        }

        // POST: ATPatient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,FirstName,LastName,Address,City,ProvinceCode,PostalCode,Ohip,DateOfBirth,Deceased,DateOfDeath,HomePhone,Gender")] Patient patient)
        {
            if (id != patient.PatientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Patient Information is Added";
                    return RedirectToAction(nameof(Index));
                }
                // catch (DbUpdateConcurrencyException)
                 catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException.Message);
                }

            
               
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", patient.ProvinceCode);
            return View(patient);
        }

        // GET: ATPatient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patient
                .Include(p => p.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: ATPatient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var patient = await _context.Patient.FindAsync(id);
            try
            {
                _context.Patient.Remove(patient);                
                await _context.SaveChangesAsync();
                TempData["message"] = "Successfully Deleted";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["message"] = "Error in deleting record";
                ModelState.AddModelError("", ex.InnerException.Message);
            }
            return View(patient);
        }

        private bool PatientExists(int id)
        {
            return _context.Patient.Any(e => e.PatientId == id);
        }
    }
}
