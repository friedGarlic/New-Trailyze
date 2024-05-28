using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ML_ASP.Models.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ML_ASP.Models
{
    public class Account_Model : IdentityUser
    {
        [Range(18, 50)]
        public int Age { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        //for viewing
        public int? WeeklyReportRemaining { get; set; } = 16;

        //remaining
        public int? HoursRemaining { get; set; } = 600;
        public int? MinutesRemaining { get; set; } = 0;
        public int? SecondsRemaining { get; set; } = 0;

        //completed
        public int? HoursCompleted { get; set; } = 0;
        public int? MinutesCompleted { get; set; } = 0;
        public int? SecondsCompleted { get; set; } = 0;

        //total
        public TimeSpan TotalTime { get; set; }

        //profile
        public string? ImageUrl { get; set; } = "/ProfileImages\\defProf.jpg";
        public string? Course { get; set; }

        //CERTIFICATE REQUIREMENTS
        public string? Medical { get; set; }
        public string? Enrollment { get; set; }

        public List<RequirementFile_Model>? Requirements { get; set; }

        public string RegistrationPermission { get; set; } = "Pending";
	}
}
