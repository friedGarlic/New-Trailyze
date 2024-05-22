using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models.ViewModel;
using ML_ASP.Utility;

namespace ML_ASP.Controllers
{
	public class TimeLogController : Controller
	{
		private readonly IUnitOfWork _unit;
		public SubmissionVM submissionVM { get; set; }

		public TimeLogController(IUnitOfWork unit)
        {
			_unit = unit;
        }

        public IActionResult TimeLog()
		{
			var getTimeLogs = _unit.Log.GetAll();
			submissionVM = new SubmissionVM()
			{
				LogList = getTimeLogs
			};

			return View(submissionVM);
		}

		[Authorize(Roles = SD.Role_Admin)]
		[HttpPost]
		public IActionResult UpdateApprovalStatusBulk(List<int> id, List<string> approvalStatus)
		{
			for (int i = 0; i < id.Count; i++)
			{
				int changedId = id[i];
				string newApprovalStatus = approvalStatus[i];

				_unit.Log.ChangeApprovalStatus(changedId, newApprovalStatus);
			}
			try
			{
				_unit.Save();
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				Console.WriteLine("Exception Message: " + message);
			}

			return RedirectToAction(nameof(TimeLog));
		}

		#region API CALLS
		[Authorize]
		[HttpGet]
		public IActionResult GetAll()
		{
			var modelList = _unit.Log.GetAll();
			return Json(new { data = modelList });
		}

		#endregion
	}
}
