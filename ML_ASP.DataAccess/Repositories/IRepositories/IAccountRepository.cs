using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ML_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories.IRepositories
{
	public interface IAccountRepository : IRepository<Account_Model>
	{
        public void UpdateAccount(string course, int hrsRemain, int weeklyReport, string id);

        public void UpdateRegistrationStatus(string id, string registrationStatus);

		public void UpdateTime(
            int? hCompleted, int? mCompleted, int? sCompleted,
            int? hRemaining, int? mRemaining, int? sRemaining,
            TimeSpan totalTime,
            string id);

        public void Update(string imageUrl, string id);
        public void UpdateCourse(string? course, string id);
        public void UpdateRequirementFile(string fileEnrollment, string fileMedical,  string id);

        //GETTERS
        public Account_Model GetFirstAndDefault();
		public double? GetRemainingHours(IdentityUser user);
		public int? GetRemainingReports(IdentityUser user);
        public string? GetImageUrl(IdentityUser user);
        public string? GetCourse(IdentityUser user);
    }
}
