using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models;
using ML_ASP.Models.Models;
using ML_ASP.Models.ViewModel;
using ML_ASP.Utility;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Security.Claims;
using OpenAI_API;
using ML_ASP.DTO;
using System.Web.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.ML;
using ML_net.ModelSession_3;

namespace ML_ASP.Controllers
{

	public class AdminController : Controller
	{
		private readonly IUnitOfWork _unit;
		private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;
		private readonly UserManager<Account_Model> _userManager;
		private readonly IEmailSender _emailSender;


		public SubmissionVM submissionVM { get; set; }

		public AdminController(IUnitOfWork unit,
			Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment,
			UserManager<Account_Model> userManager,
			IEmailSender emailSender)
		{
			_environment = environment;
			_unit = unit;
			_userManager = userManager;
			_emailSender = emailSender;
		}

		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Admin()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			IEnumerable<Account_Model> accountList = _unit.Account.GetAll();
			IEnumerable<SubmissionModel> modelList = _unit.Submission.GetAll();

			var submissionCount = modelList.Count();
			int accountCount = accountList.Select(u => u.Id).Distinct().Count();

			ViewBag.AccountCount = accountCount;
			ViewBag.SubmissionCount = submissionCount;

			//find last 5 grade from current user selected
			var sublist = _unit.Submission
				  .GetAll(u => u.SubmissionUserId == claim.Value)
				  .OrderByDescending(u => u.Id)
				  .Take(5)
				  .Select(u => u.Grade)
				  .ToList();


			submissionVM = new SubmissionVM()
			{
				ReminderList = _unit.Reminder.GetAll(u => u.UserId == claim.Value),
				GradeList = sublist,
				WorkloadList = _unit.Workload.GetAll(),
			};

			return View(submissionVM);
		}

		public IActionResult RequirementFile()
		{
			return View();
		}

		public IActionResult Overtime()
		{
			return View();
		}


		//---------------------ANALYTIC FEATURES METHODS AND GETTERS------------------------------------
		[Authorize(Roles = SD.Role_Admin)]
		public IActionResult Analytics()
		{
			var getAccounts = _unit.Account.GetAll();
			submissionVM = new SubmissionVM()
			{
				AccountList = getAccounts
			};
			return View(submissionVM);
		}

		public ActionResult ProfileFilter(string searchName)
		{
			var getAccounts = _unit.Account.GetAll();

			submissionVM = new SubmissionVM()
			{
				AccountList = getAccounts,
				SearchQuery = searchName
			};

			return View(nameof(Analytics), submissionVM);
		}
		// --------------------METHODS ------------------


		[Authorize(Roles = SD.Role_Admin)]
		[HttpPost]
		public IActionResult UpdateApprovalStatusBulk(List<int> id, List<string> approvalStatus, List<string> userId, List<string> originalApprovalStatus)
		{
			string newApprovalStatus = "";

			for (int i = 0; i < id.Count; i++)
			{
				if (originalApprovalStatus[i] != approvalStatus[i])
				{
					int changedId = id[i];
					newApprovalStatus = approvalStatus[i];

					_unit.Submission.ChangeApprovalStatus(changedId, newApprovalStatus);
					string fileName = _unit.Submission.GetFirstOrDefault(u => u.Id == id[i]).FileName;

					try
					{
						Notification_Model notif = new Notification_Model();
						notif.Title = "Pending Status";
						notif.Description = "Your Pending status file: " + fileName + " is changed to:" + newApprovalStatus;
						notif.NotifUserId = userId[i];

						_unit.Notification.Add(notif);

						// save changes in batch/ like dishes in restaurant
						_unit.Save();
					}
					catch (Exception ex)
					{
						string message = ex.Message;
						Console.WriteLine("Exception Message: " + message);
					}
				}
			}

			// save any remaining changes after the loop completes
			try
			{
				_unit.Save();
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				Console.WriteLine("Exception Message: " + message);
			}

			return RedirectToAction(nameof(Admin));
		}

		public IActionResult UpdateOvertimeStatus(List<int> id, List<string> approvalStatus, List<string> userId, List<string> originalApprovalStatus)
		{
			string newApprovalStatus = "";

			for (int i = 0; i < id.Count; i++)
			{
				if (originalApprovalStatus[i] != approvalStatus[i])
				{
					int changedId = id[i];
					newApprovalStatus = approvalStatus[i];

					_unit.Overtime.UpdateApprovalStatus(changedId, newApprovalStatus);

					try
					{
						Notification_Model notif = new Notification_Model();
						notif.Title = "Pending Status";
						notif.Description = "Your Pending Overtime Request is changed to:" + newApprovalStatus;
						notif.NotifUserId = userId[i];

						_unit.Notification.Add(notif);

						// save changes in batch/ like dishes in restaurant
						_unit.Save();
					}
					catch (Exception ex)
					{
						string message = ex.Message;
						Console.WriteLine("Exception Message: " + message);
					}
				}
			}

			// save any remaining changes after the loop completes
			try
			{
				_unit.Save();
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				Console.WriteLine("Exception Message: " + message);
			}

			return View(nameof(Overtime));
		}

		[Authorize(Roles = SD.Role_Admin)]
		[HttpPost]
		public IActionResult RequirementFileUpdate(List<string> id, List<string> registrationPermission, List<string> originalApprovalStatus,
			List<int> fileId, List<string> fileApprovalStatus, List<string> fileUserId, List<string> originalFileApprovalStatus)
		{
			string newApprovalStatus = "";

			for (int i = 0; i < id.Count; i++)
			{
				if (originalApprovalStatus[i] != registrationPermission[i])
				{
					newApprovalStatus = registrationPermission[i];

					try
					{
						var getAcc = _unit.Account.GetFirstOrDefault(u => u.Id == id[i]);

						// Synchronously call the method to change the role
						ChangeRegistrationRole(getAcc).Wait(); // Blocking call, not recommended like i did, this
															   // synchronous blocking calls, as they can lead to deadlocks if not used carefully, especially in ASP.NET Core applications

						_emailSender.SendEmailAsync(getAcc.Email, "Registration Accepted", $"Congratulations your registration is completed, you can now use the dashboard in full.");

						_unit.Account.UpdateRegistrationStatus(id[i], newApprovalStatus);

						//----for notification

						Notification_Model notif = new Notification_Model();
						notif.Title = "Pending Status";
						notif.Description = "Your Pending status for registration is changed to:" + newApprovalStatus;
						notif.NotifUserId = id[i];

						_unit.Notification.Add(notif);

						// save changes in batch/ like dishes in restaurant
						_unit.Save();
					}
					catch (Exception ex)
					{
						string message = ex.Message;
						Console.WriteLine("Exception Message: " + message);
					}
				}
			}


			for (int i = 0; i < fileId.Count; i++)
			{
				if (originalFileApprovalStatus[i] != fileApprovalStatus[i])
				{
					int changedId = fileId[i]; //the id row owner that needs change on column Approval Status
					newApprovalStatus = fileApprovalStatus[i];

					try
					{
						_unit.RequirementFile.UpdateStatus(changedId, newApprovalStatus);

						//----for notification
						string fileName = _unit.RequirementFile.GetFirstOrDefault(u => u.Id == fileId[i]).FileName;

						Notification_Model notif = new Notification_Model();
						notif.Title = "Pending Status";
						notif.Description = "Your Pending status file requirement: " + fileName + " is changed to:" + newApprovalStatus;
						notif.NotifUserId = fileUserId[i];

						_unit.Notification.Add(notif);

						// save changes in batch/ like dishes in restaurant
						_unit.Save();
					}
					catch (Exception ex)
					{
						string message = ex.Message;
						Console.WriteLine("Exception Message: " + message);
					}
				}
			}

			// save any remaining changes after the loop completes
			try
			{
				_unit.Save();
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				Console.WriteLine("Exception Message: " + message);
			}

			return View(nameof(RequirementFile));
		}

		//pdf viewers
		[Authorize(Roles = SD.Role_Admin)]
		public ActionResult ViewPdf(string fileName)
		{
			string path = Path.Combine(_environment.ContentRootPath + "\\Uploads", fileName);

			if (System.IO.File.Exists(path))
			{
				return File(System.IO.File.ReadAllBytes(path), "application/pdf");
			}
			else
			{
				TempData["failed"] = "File Not Found";
				return NotFound();
			}
		}

		[Authorize(Roles = SD.Role_Admin)]
		public ActionResult ViewPdf2(string id)
		{
			string path = Path.Combine(_environment.ContentRootPath + "\\Uploads", id);

			if (System.IO.File.Exists(path))
			{
				return File(System.IO.File.ReadAllBytes(path), "application/pdf");
			}
			else
			{
				TempData["failed"] = "File Not Found";
				return NotFound();
			}
		}

		[Authorize(Roles = SD.Role_Admin)]
		public ActionResult RequirementViewPdf2(string id)
		{
			string path = Path.Combine(_environment.WebRootPath + "\\RequirementFiles", id);

			if (System.IO.File.Exists(path))
			{
				return File(System.IO.File.ReadAllBytes(path), "application/pdf");
			}
			else
			{
				TempData["failed"] = "File Not Found";
				return NotFound();
			}
		}

		public ActionResult ViewOvertimePdf(string id)
		{
			string path = Path.Combine(_environment.WebRootPath + "\\Overtime", id + ".pdf");

			if (System.IO.File.Exists(path))
			{
				return File(System.IO.File.ReadAllBytes(path), "application/pdf");
			}
			else
			{
				TempData["failed"] = "File Not Found";
				return NotFound();
			}
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditProfile(Guid id, int numberOfHours, int weeklyReport, string _course)
		{
			_unit.Account.UpdateAccount(_course, numberOfHours, weeklyReport, id.ToString(), numberOfHours);
			_unit.Save();

			return RedirectToAction(nameof(Admin));
		}

		[Authorize]
		[HttpPost]
		public IActionResult AddTodoList(string nameOfReminder, string iconType, string iconClass)
		{
			//find the unique current user
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			Reminder_Model reminder_Model = new Reminder_Model();

			//for admin dashboard todolist, populate so it doesnt sve in db as null
			if (iconType == null)
			{
				iconType = "0";
			}

			reminder_Model.Name = nameOfReminder;
			reminder_Model.UserId = claim.Value;
			reminder_Model.IconClass = iconClass;
			reminder_Model.IconType = iconType;

			//temporary validation
			if (nameOfReminder == null)
			{
				return RedirectToAction(nameof(Admin));
			}
			else
			{
				_unit.Reminder.Add(reminder_Model);
				_unit.Save();
			}


			TempData["success"] = "Added Reminder Succesfully!";

			submissionVM = new SubmissionVM()
			{
				ReminderList = _unit.Reminder.GetAll(u => u.UserId == claim.Value)
			};

			return RedirectToAction(nameof(Admin));
		}

		[Authorize]
		[HttpPost]
		public IActionResult AddWorkload(string nameOfReminder, DateTime dateTime, string typeOfCourse, string description)
		{
			Random rnd = new Random();
			Workload_Model model = new Workload_Model();

			model.Name = nameOfReminder;
			model.Description = description;
			model.DueDate = dateTime;
			model.Course = typeOfCourse;

			model.ModelId = rnd.Next(100, 1000);

			_unit.Workload.Add(model);

			foreach (var i in _unit.Account.GetAll())
			{
				WorkloadSubmissionList_Model wrkldModel = new();
				wrkldModel.SubmissionUserID = i.Id;
				wrkldModel.WorkloadId = model.ModelId;
				wrkldModel.IsSubmitted = false;

				_unit.WorkloadSubmissionList.Add(wrkldModel);
			}
			_unit.Save();

			TempData["success"] = "Added Workload Succesfully!";

			return RedirectToAction(nameof(Admin));
		}

		[Authorize]
		public async Task<IActionResult> ChangeRegistrationRole(Account_Model account)
		{
			var user = await _userManager.FindByIdAsync(account.Id);
			if (user == null)
			{
				return NotFound();
			}

			// Remove from all roles
			var roles = await _userManager.GetRolesAsync(user);
			var result = await _userManager.RemoveFromRolesAsync(user, roles);

			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Failed to remove user roles");
				return View(nameof(Index));
			}

			// Add to the new role
			result = await _userManager.AddToRoleAsync(user, SD.Role_User);
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Failed to add user to the new role");
				return View(nameof(Index));
			}

			return RedirectToAction(nameof(Index));
		}
		//------------------------------ENDPOINT REGIONS ------------------------------//
		#region API CALLS
		[Authorize]
		[HttpGet]
		public IActionResult GetAll()
		{
			var modelList = _unit.Submission.GetAll();
			return Json(new { data = modelList });
		}

		public IActionResult GetAllReqFile()
		{
			var accountList = _unit.Account.GetAll().ToList();
			var getAllReqFile = _unit.RequirementFile.GetAll().ToList();

			foreach (var account in accountList)
			{
				// Filter the requirement files for the current account
				var accountRequirementFiles = getAllReqFile
					.Where(r => r.UserId == account.Id)
					.ToList();

				// Populate the Requirements property of the current account with the filtered requirement files
				account.Requirements = accountRequirementFiles;
			}
			return Json(new { data = accountList });
		}

		public IActionResult GetAllOvertime()
		{
			var getAllOvertime = _unit.Overtime.GetAll();

			return Json(new { data = getAllOvertime });
		}

		[HttpGet]
		public async Task<IActionResult> SendToApi()
		{
			string OutputResult = "";
			try
			{
				string apiKey = "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";
				var openai = new OpenAIAPI(apiKey);
				var request = openai.Chat.CreateConversation();
				request.AppendUserInput("The grade of student is 82/100,79/100,82/100,81.67/100. What is your analysis and recommendation on this.");
				var response = await request.GetResponseFromChatbotAsync();

				foreach (var message in response)
				{
					OutputResult += message.ToString();
				}

				return Json(new { data = OutputResult });
			}
			catch (Exception ex)
			{
				// Log any exceptions
				Console.WriteLine(ex.Message);
				return StatusCode(500, ex.Message); // Internal Server Error
			}
		}

		[HttpGet]
		public ActionResult RetrieveGradeList(string id)
		{
			var sublist = _unit.Submission
				  .GetAll(u => u.SubmissionUserId == id)
				  .OrderByDescending(u => u.Id)
				  .Take(5)
				  .Select(u => u.Grade)
				  .ToList();

			return Json(new { data = sublist });
		}
		#endregion

		[HttpPost]
		public ViewResult GenerateQRCode()  ////////////// currently not used whatsoever
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			QRModel model = new QRModel();
			if (model.QrCode == null)
			{
				var id = _unit.QR.GetFirstOrDefault().Id;
				var killFile = _unit.QR.GetFirstOrDefault(u => u.Id == id);
				_unit.QR.Remove(killFile);
			}

			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			int length = 8;
			var randomString = new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());

			model.QrCode = randomString;

			//simply do 64bit image ang 64bit string using qrcoder and bitmap
			using (MemoryStream ms = new MemoryStream())
			{
				QRCodeGenerator qrGenerator = new QRCodeGenerator();
				QRCodeData qrData = qrGenerator.CreateQrCode(model.QrCode, QRCodeGenerator.ECCLevel.Q);
				QRCode qrCode = new QRCode(qrData);
				using (Bitmap bitmap = qrCode.GetGraphic(20))
				{
					bitmap.Save(ms, ImageFormat.Png);
					model.QrCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
					ViewBag.QRCodeImage = model.QrCode;
				}
			}

			_unit.QR.Add(model);
			_unit.Save();

			submissionVM = new SubmissionVM()
			{
				ReminderList = _unit.Reminder.GetAll(u => u.UserId == claim.Value)
			};
			return View(nameof(Admin), submissionVM);
		}

		public SubmissionVM GetSubmissionVM()
		{
				var claimsIdentity = (ClaimsIdentity) User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			IEnumerable<Account_Model> accountList = _unit.Account.GetAll();
			IEnumerable<SubmissionModel> modelList = _unit.Submission.GetAll();

			var submissionCount = modelList.Count();
			int accountCount = accountList.Select(u => u.Id).Distinct().Count();

			ViewBag.AccountCount = accountCount;
				ViewBag.SubmissionCount = submissionCount;

				//find last 5 grade from current user selected
				var sublist = _unit.Submission
					  .GetAll(u => u.SubmissionUserId == claim.Value)
					  .OrderByDescending(u => u.Id)
					  .Take(5)
					  .Select(u => u.Grade)
					  .ToList();


			submissionVM = new SubmissionVM()
			{
				ReminderList = _unit.Reminder.GetAll(u => u.UserId == claim.Value),
					GradeList = sublist,
					WorkloadList = _unit.Workload.GetAll(),
				};

			return submissionVM;
		}

        public async Task<ActionResult> RunConsoleApp()
        {
            try
            {
                // Initialize MLContext
                MLContext mlContext = new MLContext();

                // Generate model
                var model = Demo.GenerateModel(mlContext);

                // Optionally, you can return the results to a view
                return View();
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return Content($"An error occurred: {ex.Message}");
            }
        }



    }
}
