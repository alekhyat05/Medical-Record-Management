using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ATClassLibrary;
using System.Text.RegularExpressions;

namespace ATPatients.Models
{
    [ModelMetadataTypeAttribute(typeof(PatientMetaData))]

    public partial class Patient : IValidatableObject
    {
        PatientsContext _context = new PatientsContext();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == null || FirstName.Trim() == "")
            {
                yield return new ValidationResult("First name cannot be empty or blank.");
            }
            else
            {
                FirstName = FirstName.Trim();
                FirstName = ATValidations.ATCapitalize(FirstName);
            }

            if (LastName == null || LastName.Trim() == "")
            {
                yield return new ValidationResult("Last name cannot be empty or blank.");
            }
            else
            {
                LastName = LastName.Trim();
                LastName = ATValidations.ATCapitalize(LastName);
            }


            if (Address == null || Address.Trim() == "")
            {
                yield return new ValidationResult("Address cannot be empty or blank.");
            }
            else
            {
                Address = ATValidations.ATCapitalize(Address);
            }


            if (City == null || City.Trim() == "")
            {
                yield return new ValidationResult("City cannot be empty or blank.");
            }
            else
            {
                City = ATValidations.ATCapitalize(City);
            }


            if (Gender == null || Gender.Trim() == "")
            {
                yield return new ValidationResult("Gender cannot be empty or blank.");
            }
            else
            {
                Gender = ATValidations.ATCapitalize(Gender);
            }

            if (ProvinceCode.Length > 2) 
            {
                yield return new ValidationResult("Province code cannot be more than 2 letters.");
            }
            //if (!string.IsNullOrEmpty(PostalCode)){

            //    var provinceName = _context.Diagnosis.Where(a => a.DiagnosisId == patDiagnosisRecord.DiagnosisId).FirstOrDefault();
            //    PostalCode = PostalCode.ToUpper();
            //    if (string.IsNullOrEmpty(ProvinceCode))
            //    {
            //        yield return new ValidationResult("Province code is needed first", new[] { "Postalcode" });
            //    }
            //    if(corres)
            //}

            if (!string.IsNullOrEmpty(Ohip))
            {
                Ohip = Ohip.Trim().ToUpper();

                Regex ohipRegex = new Regex(@"\d{4}(-\d{3})(-\d{3})-[A-Z][A-Z]", RegexOptions.IgnoreCase);

                if (!ohipRegex.IsMatch(Ohip)){
                    yield return new ValidationResult("OHIP pattern is 1111-111-111-XX", new[] { "Ohip" });
                }

            }

            if (!string.IsNullOrEmpty(HomePhone))
            {
                HomePhone = ATValidations.ATExtractDigits(HomePhone);
                if(HomePhone.Length != 10)
                {
                    yield return new ValidationResult("Please enter 10 digits for Phone number", new[] { "HomePhone" });
                }
                else
                {
                    HomePhone = HomePhone.Insert(3, "-").Insert(7, "-");
                }

            }

            if (DateOfBirth.HasValue)
            {
                if (DateOfBirth.Value.Date > DateTime.Now.Date)
                {
                    yield return new ValidationResult("Date of birth cannot be future date", new[] { "DateOfBirth" });
                }
                if (Deceased)
                {
                    if (!DateOfDeath.HasValue)
                    {
                        yield return new ValidationResult("Please enter the Date of Death", new[] { "DateOfDeath" });
                    }
                }
                else
                {
                    if (DateOfDeath.HasValue)
                    {
                        yield return new ValidationResult("Date of Death should be empty", new[] { "DateOfDeath" });
                    }
                }

                if (DateOfDeath.HasValue)
                {
                    if (DateOfDeath.Value.Date > DateTime.Now.Date || DateOfDeath.Value.Date < DateOfBirth.Value.Date)
                    {
                        yield return new ValidationResult("Date of Death cannot be in the future or before date of birth", new[] { "DateOfDeath" });
                    }
                }
            }

            if (!string.IsNullOrEmpty(Gender))
            {
                Gender = Gender.ToUpper();

                Regex genderRegex = new Regex("[MFX]");
                if (!genderRegex.IsMatch(Gender))
                {
                    yield return new ValidationResult("Gender is required and must be 'M', 'F' or 'X'", new[] { "Gender" });
                }
            }

        }
    }
    public class PatientMetaData
    {
        public int PatientId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Street Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Province Code")]
        public string ProvinceCode { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "OHIP")]
        public string Ohip { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Deceased")]
        public bool Deceased { get; set; }

        [Display(Name = "Date of Death")]
        public DateTime? DateOfDeath { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}
