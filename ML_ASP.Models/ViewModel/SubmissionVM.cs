using ML_ASP.Models.Models;
using ML_ASP.Models.Models.UserDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_ASP.Models.ViewModel
{
	public class SubmissionVM
	{
		public IEnumerable<SubmissionModel> SubmissionList { get; set; }

		public IEnumerable<Account_Model> AccountList { get; set; }

        public IEnumerable<Reminder_Model> ReminderList { get; set; }

		public IEnumerable<LogModel> LogList { get; set; }

		public IEnumerable<Workload_Model> WorkloadList { get; set; }

		public IEnumerable<WorkloadSubmissionList_Model> WorkloadSubmissionList { get; set; }

        public IEnumerable<WorkloadSubmissionList_Model> CurrentUserSubmissionList { get; set; }

		public IEnumerable<Overtime_Model> OvertimeList { get; set; }

        public LogModel LogModel { get; set; }

        public string CurrentUserId{ get; set; }

        public string AccountName { get; set; }

		public string TimeLog { get; set; }

		public string Prediction { get; set; }

		public bool IsMultipleFile { get; set; }

		public List<string> GradeList { get; set; }

		public string SearchQuery { get; set; }
	}
}
