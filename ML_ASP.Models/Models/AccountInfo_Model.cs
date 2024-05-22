using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.Models
{
    public class AccountInfo_Model
    {
        [Key]
        public string id { get; set; }

        public string? UserId { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? CurriculumYear { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Sex { get; set; }

        public string? Age { get; set; }

        public string? CivilStatus { get; set; }

        public int? MobileNumber { get; set; }

        public string? Address { get; set; }

        public string? Scholarship { get; set; }

        //----------------------------------

        public string? GuardianName { get; set; }

        public string? GuardianRelationship { get; set; }

        public string? GuardianAddress { get; set; }

        public int? GuardianMobileNumber { get; set; }

        //-------------------------------------internship
        public string Category { get; set; }
        public string Offices { get; set; }
        public string OffCampus { get; set; }
    }
}
