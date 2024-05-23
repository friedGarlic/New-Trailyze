using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.DataAccess.Repositories
{
	public class AccountRepository : Repository<Account_Model>, IAccountRepository
	{
		private ApplicationDBContext _dbContext;

		public AccountRepository(ApplicationDBContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
        }

        public void Update(string imageUrl, string id) //update Image url
        {
            var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == id);
            if (objFromDb != null)
            {
                objFromDb.ImageUrl = imageUrl;
            }
        }


		public void UpdateRegistrationStatus(string id, string registrationStatus)
        {
			var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == id);
			if (objFromDb != null)
			{
                objFromDb.RegistrationPermission = registrationStatus;
			}
		}

		public void UpdateRequirementFile(string fileEnrollment, string fileMedical, string id)
        {
            var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == id);
            if (objFromDb != null)
            {
                objFromDb.Enrollment = fileEnrollment;
                objFromDb.Medical = fileEnrollment;
            }
        }

        public void UpdateCourse(string? course, string id)
        {
            var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == id);
            if (objFromDb != null)
            {
                objFromDb.Course = course;
            }
        }

		public void UpdateAccount(string course, int hrsRemain,int weeklyReport, string id)
		{
			var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == id);
			if (objFromDb != null)
			{
				objFromDb.HoursRemaining = hrsRemain;
                objFromDb.WeeklyReportRemaining = weeklyReport;
                objFromDb.Course = course;
			}
		}

        public void UpdateTime(
            int? hCompleted, int? mCompleted,  int? sCompleted,
            int? hRemaining, int? mRemaining, int? sRemaining,
            TimeSpan totalTime,
            string id)
        {
            var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == id);
            if (objFromDb != null)
            {
                //remaining
                objFromDb.HoursRemaining = hRemaining;
                objFromDb.MinutesRemaining = mRemaining;
                objFromDb.SecondsRemaining = sRemaining;

                //completed
                objFromDb.HoursCompleted = hCompleted;
                objFromDb.MinutesCompleted = mCompleted;
                objFromDb.SecondsCompleted = sCompleted;

                objFromDb.TotalTime = totalTime;
            }
        }

        //GETTERS ------------------------------------
        public double? GetRemainingHours(IdentityUser user)
		{
            var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == user.Id);

			var remain = objFromDb.HoursRemaining;

            return remain;
        }

        public int? GetRemainingReports(IdentityUser user)
        {
            var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == user.Id);

            var remain = objFromDb.WeeklyReportRemaining;

            return remain;
        }

        public string? GetImageUrl(IdentityUser user)
        {
            var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == user.Id);

            var remain = objFromDb.ImageUrl;

            return remain;
        }

        public Account_Model GetFirstAndDefault()
        {
            return _dbContext.Accounts.FirstOrDefault();
        }

        public string? GetCourse(IdentityUser user)
        {
            var objFromDb = _dbContext.Accounts.FirstOrDefault(u => u.Id == user.Id);

            var remain = objFromDb.Course;

            return remain;
        }
    }
}
