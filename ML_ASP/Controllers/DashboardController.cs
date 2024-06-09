using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models.Models;
using ML_ASP.Models.ViewModel;
using System.Security.Claims;
//using ML_net.ModelSession_1;
using ML_net.ModelSession_2;

//
using QRCoder;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iText.Barcodes.Qrcode;
using ML_net.ModelSession_3;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.ComponentModel.DataAnnotations;
using Tensorflow;
using ML_ASP.Utility;
using ML_ASP.Models.Models.UserDashboard;
using GroupDocs.Editor.Options;
using GroupDocs.Editor;
using GroupDocs.Editor.Formats;

namespace ML_ASP.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly MLContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        public SubmissionVM submissionVM { get; set; }

        public DashboardController(Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment,
            IUnitOfWork unit)
        {
            _unit = unit;
            _environment = environment;
            _context = new MLContext(); //was supposed to be DB, but the architecture was applied late
        } //construction of 2 model are on filemanagement controller

        // TODO add another template for printing the table of Timeinout

        [Authorize(Roles = SD.Role_User)]
        [HttpGet]
        public IActionResult Dashboard()
        {
            GetSubmissionVM();

            return View(submissionVM);
        }

        public IActionResult EditTemplate()
        { return View(); }

        public IActionResult DTRTemplate()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value); //userid

            submissionVM = new SubmissionVM()
            {
                LogList = _unit.Log.GetAll(u => u.LogId == claim.Value),
            };



            return View(submissionVM);
        }

        // --------- ONLY FOR TIME LOG PURPOSES -----------------
        [Authorize(Roles = SD.Role_User)]
        [HttpPost]
        [Authorize]
        public IActionResult Dashboard(bool IsTimedIn)
        {
            //stuck 2nd when trying to time out IsTimedIn stays true
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value);

            var accountName = account.FullName;

            ViewBag.AccountName = accountName;

            LogModel logModel = new LogModel();

            if (IsTimedIn == false)
            {
                logModel.LogId = account.Id;
                logModel.DateTime = DateTime.Now;
                logModel.Log = "Timed In";
                logModel.FullName = account.FullName;

                TempData["success"] = "Timed In Succesfully!";
            }
            else
            {
                logModel.LogId = account.Id;
                logModel.DateTime = DateTime.Now;
                logModel.Log = "Timed Out";
                logModel.FullName = account.FullName;

                TempData["success"] = "Timed Out Succesfully!";
            }

            submissionVM = new SubmissionVM()
            {
                TimeLog = logModel.Log,
                LogList = _unit.Log.GetAll(u => u.LogId == claim.Value)
            };


            _unit.Log.Add(logModel);
            _unit.Save();

            if (IsTimedIn == true)
            {
                InputTimeDuration();
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadImage(IFormFile file) //for profile image
        {
            //find the unique user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value);
            //

            if (file != null && file.Length > 0)
            {
                string projectPath = _environment.WebRootPath;
                string uploadFolderName = "ProfileImages";
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(projectPath, uploadFolderName);
                var extension = Path.GetExtension(file.FileName);

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                if (account.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(projectPath, account.ImageUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                // Update the image URL in the model
                var imageUrl = Path.Combine("/", uploadFolderName, fileName + extension);

                _unit.Account.Update(imageUrl, account.Id);

                _unit.Save();
                TempData["success"] = "Uploaded Successfully!";
            }
            else
            {
                TempData["error"] = "No file uploaded.";
            }

            return RedirectToAction(nameof(Dashboard));
        }

        //-----------------ADD ON FEATURES ---------------------
        [HttpPost]
        public ActionResult AddReminder(string nameOfReminder, string iconType, string iconClass, DateTime dateTime)
        {
            //find the unique current user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Reminder_Model reminder_Model = new Reminder_Model();

            DateTime currentTime = DateTime.Now;
            TimeSpan duration = dateTime - currentTime;
            double durationInSeconds = duration.TotalMilliseconds;

            if (durationInSeconds > 0 && !string.IsNullOrEmpty(nameOfReminder) && !string.IsNullOrEmpty(iconClass) &&
                    !string.IsNullOrEmpty(iconType) && !string.IsNullOrEmpty(claim.Value) && dateTime != null)
            {
                reminder_Model.ReminderDuration = durationInSeconds;
                reminder_Model.Name = nameOfReminder;
                reminder_Model.UserId = claim.Value;
                reminder_Model.IconClass = iconClass;
                reminder_Model.IconType = iconType;
                reminder_Model.ReminderDateTime = dateTime;

                _unit.Reminder.Add(reminder_Model);
                _unit.Save();
            }

            TempData["success"] = "Added Reminder Succesfully!";

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public ActionResult AddOvertimeRequest(string description, TimeSpan endTime, DateTime overtimeDate, IFormFile postedFiles, DateTime startDate, DateTime endDate)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value);

            string fileName = "";
            string fileId = "";

            if (postedFiles != null && postedFiles.Length > 0)
            {
                string projectPath = _environment.WebRootPath;
                string uploadFolderName = "Overtime";


                var uploads = Path.Combine(projectPath, uploadFolderName);
                var extension = Path.GetExtension(postedFiles.FileName);

                fileId = Guid.NewGuid().ToString();
                fileName = postedFiles.FileName + extension;
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, fileId + extension), FileMode.Create))
                {
                    postedFiles.CopyTo(fileStream);
                }

                TempData["success"] = "Uploaded Successfully!";
            }

            var request = new Overtime_Model
            {
                UserId = account.Id,
                Description = description,
                OvertimeEndTime = endTime,
                RequestDate = overtimeDate,
                UserName = account.FullName,
                FileName = fileName,
                FileId = fileId,
                EndDate = endDate,
                StartDate = startDate,
            };

            try
            {
                _unit.Overtime.Add(request);

                _unit.Save();
            }
            catch (Exception e) { }

            GetSubmissionVM();

            return View(nameof(Dashboard), submissionVM);
        }

        [HttpPost]
        public ActionResult SubmitExit(IFormFile postedFiles1, IFormFile postedFiles2)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value);

            string fileName = "";
            string fileId = "";

            string fileName2 = "";
            string fileId2 = "";

            if (postedFiles1 != null && postedFiles1.Length > 0)
            {
                string projectPath = _environment.WebRootPath;
                string uploadFolderName = "RequirementFiles";


                var uploads = Path.Combine(projectPath, uploadFolderName);
                var extension = Path.GetExtension(postedFiles1.FileName);

                fileId = Guid.NewGuid().ToString();
                fileName = fileId + extension;
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, fileId + extension), FileMode.Create))
                {
                    postedFiles1.CopyTo(fileStream);
                }

                TempData["success"] = "Uploaded Successfully!";
            }
            if (postedFiles2 != null && postedFiles2.Length > 0)
            {
                string projectPath = _environment.WebRootPath;
                string uploadFolderName = "RequirementFiles";


                var uploads = Path.Combine(projectPath, uploadFolderName);
                var extension = Path.GetExtension(postedFiles2.FileName);

                fileId2 = Guid.NewGuid().ToString();
                fileName2 = fileId2 + extension;
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, fileId2 + extension), FileMode.Create))
                {
                    postedFiles2.CopyTo(fileStream);
                }

                TempData["success"] = "Uploaded Successfully!";
            }



            try
            {
                _unit.Account.UpdateExit(account.Id, fileName);
                _unit.Account.UpdateEvaluation(account.Id, fileName2);

                _unit.Save();
            }
            catch (Exception e) { }

            GetSubmissionVM();

            return View(nameof(Dashboard), submissionVM);
        }
        //-----------------HELPER FUNCTIONS OR METHODS--------------------------
        [HttpPost]
        public ActionResult DeleteReminder(int id)
        {
            var killFile = _unit.Reminder.GetFirstOrDefault(u => u.Id == id);
            _unit.Reminder.Remove(killFile);
            _unit.Save();

            TempData["success"] = "Delete Reminder Succesfully!";

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public ActionResult DeleteOvertime(int id)
        {
            //string path = Path.Combine(_environment.ContentRootPath + "\\Uploads", fileId);
            //if (System.IO.File.Exists(path))
            //{
            //    System.IO.File.Delete(path);
            //}

            var killFile = _unit.Overtime.GetFirstOrDefault(u => u.id == id);
            _unit.Overtime.Remove(killFile);
            _unit.Save();

            TempData["success"] = "Delete Overtime Request Succesfully!";

            return RedirectToAction(nameof(Dashboard));
        }

        //for calculation in time duration between Timein/timout
        public void InputTimeDuration()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value);

            //populate account hrs if null
            PopulateTime();

            var lastTimedInEntry = _unit.Log.GetAll(u => u.LogId == claim.Value && u.Log == "Timed In")
                                    .OrderByDescending(u => u.DateTime)
                                    .FirstOrDefault();

            var lastTimedOutEntry = _unit.Log.GetAll(u => u.LogId == claim.Value && u.Log == "Timed Out")
                                     .OrderByDescending(u => u.DateTime)
                                     .FirstOrDefault();


            TimeSpan fullDuration = lastTimedOutEntry.DateTime - lastTimedInEntry.DateTime;

            int totalDurationSeconds = (int)fullDuration.TotalSeconds;
            int hours = totalDurationSeconds / 3600; // Calculate hours from total seconds
            totalDurationSeconds %= 3600;            // Remaining seconds after subtracting hours
            int minutes = totalDurationSeconds / 60; // Calculate minutes from remaining seconds
            int seconds = totalDurationSeconds % 60;

            //completed
            var totalHours = account.HoursCompleted + hours;
            var totalMinutes = account.MinutesCompleted + minutes;
            var totalSeconds = account.SecondsCompleted + seconds;

            //remianing
            var totalRemainingH = account.HoursRemaining - hours;
            var totalRemainingM = account.MinutesRemaining - minutes;
            var totalRemainingS = account.SecondsRemaining - seconds;

            if (totalRemainingS < 0)
            {
                totalRemainingM -= 1;
                totalRemainingS += 60;
            }

            if (totalRemainingM < 0)
            {
                totalRemainingH -= 1;
                totalRemainingM += 60;
            }

            _unit.Account.UpdateTime(totalHours, totalMinutes, totalSeconds,
                totalRemainingH, totalRemainingM, totalRemainingS,
                fullDuration, account.Id);
            _unit.Save();
        }

        //check if h/m/s duration is null
        public void PopulateTime()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value);

            //completed
            if (account.HoursCompleted == null)
            {
                account.HoursCompleted = 0;
            }
            if (account.MinutesCompleted == null)
            {
                account.MinutesCompleted = 0;
            }
            if (account.SecondsCompleted == null)
            {
                account.SecondsCompleted = 0;
            }

            //remaining
            if (account.HoursRemaining == null)
            {
                account.HoursRemaining = 0;
            }
            if (account.MinutesRemaining == null)
            {
                account.MinutesRemaining = 0;
            }
            if (account.SecondsRemaining == null)
            {
                account.SecondsRemaining = 0;
            }
        }

        //for displaying dashboard
        public SubmissionVM GetSubmissionVM()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value); //userid

            var userId = claim.Value;
            var accountName = account.FullName;

            ViewBag.AccountName = accountName;

			//calculation of estimated datetime
			int TrainingHoursPerWeek = 40; 
            DateTime EndDate = new DateTime(DateTime.Now.Year, 12, 1); //december 1, end of summer

            int totalCompletedSeconds =
				(account.HoursCompleted.Value * 3600) +
				(account.MinutesCompleted.Value * 60) +
				account.SecondsCompleted.Value;

			// Calculate total required time in seconds
			int totalRequiredSeconds = account.HoursRequired.Value * 3600;

			int remainingSeconds = totalRequiredSeconds - totalCompletedSeconds;

			int trainingHoursPerWeekInSeconds = TrainingHoursPerWeek * 3600;

			double weeksNeeded = (double)remainingSeconds / trainingHoursPerWeekInSeconds;

			DateTime startDate = DateTime.Now;
			DateTime estimatedEndDate = startDate.AddDays(weeksNeeded * 7); //end date of training base on how many trainings per week.----------
            //

            int remainingDays = (EndDate - startDate).Days;
            int remainingWeeks = remainingDays / 7;

            int totalAvailableTrainingSeconds = remainingWeeks * trainingHoursPerWeekInSeconds;

            double overtimeNeeded = 0;
            if (remainingSeconds > totalAvailableTrainingSeconds)
            {
                int remainingSecondsAfterAvailable = remainingSeconds - totalAvailableTrainingSeconds;
                overtimeNeeded = Math.Ceiling(remainingSecondsAfterAvailable / 3600.0);
            }

            ViewBag.OvertimeNeeded = overtimeNeeded;
            ViewBag.EstimateEndTraining = estimatedEndDate;



            //for grades
            var sublist = _unit.Submission
                  .GetAll(u => u.SubmissionUserId == claim.Value)
                  .Take(5)
                  .Select(u => u.Grade)
                  .ToList();
            //-------------------grade ends

            //OVERTIME 
            var overtimeList = _unit.Overtime.GetAll(u => u.UserId == account.Id);

            var overtimeToday = DateTime.Now;

            foreach (var overtime in overtimeList)
            {
                if (DateTime.Now.Date == overtime.RequestDate?.Date) // Ensure RequestDate is not null before accessing its Date property
                {
                    overtimeToday = (DateTime)overtime.RequestDate;
                    if (overtime.OvertimeEndTime != null) // Ensure OvertimeEndTime is not null
                    {
                        // Assuming overtime.OvertimeEndTime is a TimeSpan
                        TimeSpan overtimeEndTime = (TimeSpan)overtime.OvertimeEndTime;
                        DateTime currentDateTime = DateTime.Today; // Get today's date without the time
                        DateTime overtimeEndDateTime = currentDateTime.Add(overtimeEndTime); // Combine today's date with the TimeSpan

                        // Pass the combined DateTime to the ViewBag
                        ViewBag.OvertimeEndTime = overtimeEndDateTime;
                    }
                }
            }

            var allOvertimeDates = new List<DateTime>();
            foreach (var overtime in overtimeList)
            {
                if (overtime.StartDate.HasValue && overtime.EndDate.HasValue)
                {
                    var datesInRange = GetDateRange(overtime.StartDate.Value, overtime.EndDate.Value);
                    var validDates = datesInRange.Where(d => !IsWeekend(d) && !IsHoliday(d)).ToList();
                    allOvertimeDates.AddRange(validDates);
                }
            }

            bool isTodayOvertime = allOvertimeDates.Any(d => d.Date == DateTime.Now.Date);

            ViewBag.IsTodayOvertime = isTodayOvertime;
            ViewBag.OvertimeToday = overtimeToday;

            //reminder alarm
            var getaccounttime = _unit.Reminder.GetAll(u => u.UserId == userId);

            foreach (var i in getaccounttime.ToList())
            {
                DateTime currentTime = DateTime.Now;
                if (i.ReminderDateTime > currentTime)
                {
                    TimeSpan duration = (TimeSpan)(i.ReminderDateTime - currentTime);
                    double durationInSeconds = duration.TotalMilliseconds;

                    _unit.Reminder.Update(i.Id, durationInSeconds);
                }
            }
            //-------------------reminder alarm ends

            submissionVM = new SubmissionVM()
            {
                LogList = _unit.Log.GetAll(u => u.LogId == claim.Value),
                ReminderList = _unit.Reminder.GetAll(u => u.UserId == claim.Value),
                GradeList = sublist,
                CurrentUserId = userId,
                OvertimeList = overtimeList,
            };
            //Get Account List and name ends --------------------------

            //retrieve last entry
            //so it stays as Timed in if timed in-------------------
            var lastLogEntry = _unit.Log.GetAll(u => u.LogId == claim.Value)
                                   .OrderByDescending(u => u.DateTime)
                                   .FirstOrDefault();

            bool initialIsTimedIn = lastLogEntry != null && lastLogEntry.Log == "Timed In";

            ViewBag.InitialIsTimedIn = initialIsTimedIn;
            //-------------------log ends

            //getters
            var submission = _unit.Submission.GetAll(u => u.SubmissionUserId == userId);
            int submissionCount = submission.Count();

            ViewBag.SubmissionCount = submissionCount;


            int hoursPerDay = 8;
            int daysPerWeek = 5;
            int hoursPerWeek = hoursPerDay * daysPerWeek;
            
            ViewBag.RemainingReports = account.HoursRequired / hoursPerWeek;

            //remaining
            ViewBag.RemainingHours = account.HoursRemaining;
            ViewBag.RemainingMinutes = account.MinutesRemaining;
            ViewBag.RemainingSeconds = account.SecondsRemaining;
            //completed
            ViewBag.HoursCompleted = account.HoursCompleted;
            ViewBag.MinutesCompleted = account.MinutesCompleted;
            ViewBag.SecondsCompleted = account.SecondsCompleted;

            ViewBag.HoursRequired = account.HoursRequired;

            //check if theres log in account
            //post picture if there is
            if (claim != null)
            {
                var getAcc = _unit.Account.GetFirstOrDefault(x => x.Id == claim.Value);
                string? imageUrl = getAcc.ImageUrl;

                ViewData["ImageUrl"] = imageUrl;
            }

            _unit.Save();

            return submissionVM;
        }

        #region
        public IActionResult GetOvertime()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value);

            var getAllOvertime = _unit.Overtime.GetAll(u => u.UserId == account.Id);

            return Json(new { data = getAllOvertime });
        }
        #endregion

        private List<DateTime> GetDateRange(DateTime start, DateTime end)
        {
            var dates = new List<DateTime>();
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            return dates;
        }

        private bool IsHoliday(DateTime date)
        {
            return _holidays.Contains(date.Date);
        }

        private bool IsWeekend(DateTime date)
        {
            var day = date.DayOfWeek;
            return day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;
        }

        private readonly List<DateTime> _holidays = new List<DateTime>
    {
        new DateTime(2024, 1, 1), // New Year's Day
        new DateTime(2024, 4, 9), // Araw ng Kagitingan
        new DateTime(2024, 4, 10), // Maundy Thursday
        new DateTime(2024, 4, 11), // Good Friday
        new DateTime(2024, 5, 1), // Labor Day
        new DateTime(2024, 6, 12), // Independence Day
        new DateTime(2024, 8, 21), // Ninoy Aquino Day
        new DateTime(2024, 8, 26), // National Heroes Day
        new DateTime(2024, 11, 1), // All Saints' Day
        new DateTime(2024, 11, 30), // Bonifacio Day
        new DateTime(2024, 12, 25), // Christmas Day
        new DateTime(2024, 12, 30)  // Rizal Day
    };

	}
}

