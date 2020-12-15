using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATPatients.Models;
using Microsoft.AspNetCore.Authorization;

namespace ATPatients.Controllers
{
    [Authorize]
    public class ATCountryController : Controller
    {
        private readonly PatientsContext _context;
        /// <summary>
        /// DB Is intialised in The below Constructor
        /// </summary>
        /// <param name="context"></param>
        public ATCountryController(PatientsContext context)
        {
            _context = context;
        }


        /// <summary>
        /// To show all the records of Country upon clicking Country  in the menu Bar
        /// </summary>
        /// <returns>index view</returns>
        // GET: ATCountry
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Country.ToListAsync());
        }

        // GET: ATCountry/Details/5
        /// <summary>
        /// Upon Click of Details .A details of selected id's(element's) complete record is displayed and Deatils view of Country invoked
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: ATCountry/Create
        /// <summary>
        /// on Click of create link a create view is invoked
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrators,MedicalStaff,Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ATCountry/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// The newly created record is inserted and the updated list is shown and redirected to Index page
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrators,MedicalStaff,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }
       
        // GET: ATCountry/Edit/5
        /// <summary>
        /// Id of sekected selected element is taken and Edit view is displayed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrators,MedicalStaff,Administrator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: ATCountry/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        ///  based on Id and unit value is updated in DB and redirected to index page 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrators,MedicalStaff,Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax")] Country country)
        {
            if (id != country.CountryCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryCode))
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
            return View(country);
        }

        // GET: ATCountry/Delete/5
        /// <summary>
        ///  upon Invoking Delete button based on id the DB  record is fetched and delete view is shown with selected record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Administrators,MedicalStaff,Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: ATCountry/Delete/5
        /// <summary>
        ///  upon click of Delete record is deletd and index view is returned
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Administrators,MedicalStaff,Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(string id)
        {
            return _context.Country.Any(e => e.CountryCode == id);
        }
    }
}
