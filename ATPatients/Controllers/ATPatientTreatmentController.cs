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
    public class ATPatientTreatmentController : Controller
    {
        private readonly PatientsContext _context;

        public ATPatientTreatmentController(PatientsContext context)
        {
            _context = context;
        }

        // GET: ATPatientTreatment
        public async Task<IActionResult> Index(string PatientDiagnosisId)
        {
           

            if (!string.IsNullOrEmpty(PatientDiagnosisId))
            {
                Response.Cookies.Append("PatientDiagnosisId", PatientDiagnosisId);
            }
            else if (Request.Query["PatientDiagnosisId"].Any())
            {
                Response.Cookies.Append("PatientDiagnosisId", Request.Query["PatientDiagnosisId"].ToString());
                PatientDiagnosisId = Request.Query["PatientDiagnosisId"].ToString();
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            else
            {
                TempData["message"] = "Please Select any Diagnosis";
                return RedirectToAction("Index", "ATPatientDiagnosis");
            }


            var patDiagnosisRecord = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            //
            var diagnosisRecord = _context.Diagnosis.Where(a =>a.DiagnosisId  == patDiagnosisRecord.DiagnosisId).FirstOrDefault();
            
            var patientRecord = _context.Patient.Where(a => a.PatientId == patDiagnosisRecord.PatientId).FirstOrDefault();

            ViewData["name"] = diagnosisRecord.Name;
            ViewData["patientName"] = patientRecord.LastName + ", " + patientRecord.FirstName;
            //

            var patientsContext = _context.PatientTreatment.Include(p => p.PatientDiagnosis).Include(p => p.Treatment).Where(p => p.PatientDiagnosisId.ToString() == PatientDiagnosisId).OrderBy(m => m.Treatment); 
            return View(await patientsContext.ToListAsync());
        }

        // GET: ATPatientTreatment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }
            var PatientDiagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }

            var patDiagnosisRecord = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            //
            var diagnosisRecord = _context.Diagnosis.Where(a => a.DiagnosisId == patDiagnosisRecord.DiagnosisId).FirstOrDefault();

            var patientRecord = _context.Patient.Where(a => a.PatientId == patDiagnosisRecord.PatientId).FirstOrDefault();

            ViewData["name"] = diagnosisRecord.Name;
            ViewData["patientName"] = patientRecord.LastName + ", " + patientRecord.FirstName;


            return View(patientTreatment);
        }

        // GET: ATPatientTreatment/Create
        public IActionResult Create()
        {

            var PatientDiagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            // ViewData["DateValue"] = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");

            ViewData["DateValue"] = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");

            var patDiagnosisRecord = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            //
            var diagnosisRecord = _context.Diagnosis.Where(a => a.DiagnosisId == patDiagnosisRecord.DiagnosisId).FirstOrDefault();

            var patientRecord = _context.Patient.Where(a => a.PatientId == patDiagnosisRecord.PatientId).FirstOrDefault();

            ViewData["name"] = diagnosisRecord.Name;
            ViewData["patientName"] = patientRecord.LastName + ", " + patientRecord.FirstName;

            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
          //  ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "Name");

            int PDiagnosisId = 0;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }

         

            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(m => m.DiagnosisId == PDiagnosisId), "TreatmentId", "Name");
            return View();
        }

        // POST: ATPatientTreatment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            string PDiagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }

            patientTreatment.PatientDiagnosisId = Int32.Parse(PDiagnosisId);

          
          //  patientTreatment.DatePrescribed = myDateTime;
            // patientTreatment.DatePrescribed = dateValue.ToString("MM/dd/yyyy hh:mm:ss tt");
            //dateValue.;
            patientTreatment.DatePrescribed = DateTime.Now;
            ViewBag.DateTime = patientTreatment.DatePrescribed.ToString("dd MMMM yyyy HH:mm");
            if (ModelState.IsValid)
            {
               // _context.Add(PatientDiagnosisId);
                _context.Add(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "Name", patientTreatment.TreatmentId);
            return View(patientTreatment);
        }

        // GET: ATPatientTreatment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int PDiagnosisId = 0;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PDiagnosisId = Convert.ToInt32(Request.Cookies["PatientDiagnosisId"]);
            }      

            var patientTreatment = await _context.PatientTreatment.FindAsync(id);

            patientTreatment.PatientDiagnosisId = PDiagnosisId;


            if (patientTreatment == null)
            {
                return NotFound();
            }
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatment.Where(m => m.DiagnosisId == PDiagnosisId), "TreatmentId", "Name");
            return View(patientTreatment);
        }

        // POST: ATPatientTreatment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            if (id != patientTreatment.PatientTreatmentId)
            {
                return NotFound();
            }

            string PDiagnosisId = string.Empty;
            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }

            patientTreatment.PatientDiagnosisId = Int32.Parse(PDiagnosisId);

           int cvasdtyasug= patientTreatment.PatientDiagnosisId;



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTreatmentExists(patientTreatment.PatientTreatmentId))
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
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
           ViewData["TreatmentId"] = new SelectList(_context.Treatment, "TreatmentId", "Name", patientTreatment.TreatmentId).Where(m => id == patientTreatment.PatientDiagnosisId  );
            return View(patientTreatment);
        }

        // GET: ATPatientTreatment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatment
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // POST: ATPatientTreatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientTreatment = await _context.PatientTreatment.FindAsync(id);
            _context.PatientTreatment.Remove(patientTreatment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientTreatmentExists(int id)
        {
            return _context.PatientTreatment.Any(e => e.PatientTreatmentId == id);
        }
    }
}
