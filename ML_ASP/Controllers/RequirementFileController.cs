using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models.Models;
using ML_ASP.Models.ViewModel;
using ML_ASP.Utility;
using System.Security.Claims;

namespace ML_ASP.Controllers
{
    public class RequirementFileController : Controller
    {
        public readonly IUnitOfWork _unit;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        public RequirementVM requirementVM { get; set; }

        public RequirementFileController(IUnitOfWork unit, IWebHostEnvironment environment)
        {
            _unit = unit;
            _environment = environment;

        }


        [Authorize(Roles = SD.Role_Unregistered)]
        public IActionResult Index()
        {
            GetRequirementVM();

            return View(requirementVM);
        }
        public IActionResult Step2()
        {
            GetRequirementVM();

            return View(requirementVM);
        }
        public IActionResult Step3()
        {
            GetRequirementVM();

            return View(requirementVM);
        }

        public IActionResult Step4()
        {
            GetRequirementVM();

            return View(requirementVM); 
        }
        public IActionResult Step5()
        {
            GetRequirementVM();

            return View(requirementVM); 
        }

        //==============================================
        public IActionResult Step2Submit(IFormFile postedFiles1, string title2, string description2,string step)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            string projectPath = _environment.WebRootPath;
            string uploadFolderName = "RequirementFiles";
            var uploads = Path.Combine(projectPath, uploadFolderName);

            var userModel = _unit.Account.GetFirstOrDefault(x => x.Id == userId);
            var userForm = _unit.RequirementForm.GetFirstOrDefault(x => x.UserId == userId);
            var getAllForm = _unit.RequirementForm.GetAll(x => x.UserId == userId);

            RequirementFile_Model userFile = new();
            string fileName2 = "";
            //-----------------------------------POSTED FILE 1
            if (postedFiles1 != null && postedFiles1.Length > 0)
            {
                string file2Id = Guid.NewGuid().ToString();

                var file2Extension = Path.GetExtension(postedFiles1.FileName);

                fileName2 = postedFiles1.FileName;

                string newFileId2 = file2Id + file2Extension;

                using (var fileStream = new FileStream(Path.Combine(uploads, file2Id + file2Extension), FileMode.Create))
                {
                    postedFiles1.CopyTo(fileStream);
                }

                userFile.FileName = fileName2;
                userFile.UserId = userId;
                userFile.FileId = newFileId2;
                userFile.Title = title2;
                userFile.Description = description2;
                userFile.UserName = userModel.FullName;

                //----flagging the form
                
                if (step == "laststep")
                {
                    _unit.RequirementForm.UpdateFormState(userFile, 5);
                }
                else
                {
                    _unit.RequirementForm.UpdateFormState(userFile, 2);
                }

                _unit.RequirementFile.Add(userFile);
            }

            //needs foreach loop
            GetRequirementVM();

            if (postedFiles1 != null)
            {
                _unit.Save();
            }


            return RedirectToAction("index", "RequirementFile");
        }

        public IActionResult Step3Submit(IFormFile postedFiles2, string title3, string description3, string step3)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            string projectPath = _environment.WebRootPath;
            string uploadFolderName = "RequirementFiles";
            var uploads = Path.Combine(projectPath, uploadFolderName);

            var userId = claim.Value;
            var userModel = _unit.Account.GetFirstOrDefault(x => x.Id == userId);
            var userForm = _unit.RequirementForm.GetFirstOrDefault(x => x.UserId == userId);
            var getAllForm = _unit.RequirementForm.GetAll(x => x.UserId == userId);

            RequirementFile_Model userFile = new();
            string fileName3 = "";

            //-----------------------------------POSTED FILE 2
            if (postedFiles2 != null && postedFiles2.Length > 0)
            {
                string file3Id = Guid.NewGuid().ToString();

                var file3Extension = Path.GetExtension(postedFiles2.FileName);

                fileName3 = postedFiles2.FileName;

                string newFileId3 = file3Id + file3Extension; using (var fileStream = new FileStream(Path.Combine(uploads, file3Id + file3Extension), FileMode.Create))
                {
                    postedFiles2.CopyTo(fileStream);
                }

                userFile.FileName = fileName3;
                userFile.UserId = userId;
                userFile.FileId = newFileId3;
                userFile.Title = title3;
                userFile.Description = description3;
                userFile.UserName = userModel.FullName;

                if (step3 == "step3")
                {
                    _unit.RequirementForm.UpdateFormState(userFile, 3);
                }
                else
                {
                    _unit.RequirementForm.UpdateFormState(userFile, 4);
                }
                _unit.RequirementFile.Add(userFile);
            }

            //needs foreach loop

            //2views share this controoler becareful on passing in viewmodel
            GetRequirementVM();

            if (postedFiles2 != null)
            {
                _unit.Save();
            }

            return RedirectToAction("index", "RequirementFile");
        }

        public ActionResult SubmitDocument(IFormFile postedFiles0, string title, string description,string campusOption, string CampusValue, string step)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            string projectPath = _environment.WebRootPath;
            string uploadFolderName = "RequirementFiles";
            var uploads = Path.Combine(projectPath, uploadFolderName);

            var userId = claim.Value;
            var userModel = _unit.Account.GetFirstOrDefault(x => x.Id == userId);
            var userForm = _unit.RequirementForm.GetFirstOrDefault(x => x.UserId == userId);
            var getAllForm = _unit.RequirementForm.GetAll(x => x.UserId == userId);

            RequirementFile_Model userFile = new();
            string fileName = "";

            if(campusOption != null || CampusValue != null) {
                _unit.Account.UpdateAccountTrainingLocation(userId, campusOption, CampusValue);
            }

            _unit.Save();

            //--------------------------------------------------------------------------POSTED FILE 0
            if (postedFiles0 != null && postedFiles0.Length > 0)
            {
                string fileId = Guid.NewGuid().ToString();

                var fileExtension = Path.GetExtension(postedFiles0.FileName);

                fileName = postedFiles0.FileName;

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                string newFileId = fileId + fileExtension;

                using (var fileStream = new FileStream(Path.Combine(uploads, fileId + fileExtension), FileMode.Create))
                {
                    postedFiles0.CopyTo(fileStream);
                }

                //----requirement file
                userFile.FileName = fileName;
                userFile.UserId = userId;
                userFile.FileId = newFileId;
                userFile.Title = title;
                userFile.Description = description;
                userFile.UserName = userModel.FullName;

                //----flagging the form
                _unit.RequirementForm.UpdateFormState(userFile, 1);
                _unit.RequirementFile.Add(userFile);
            }

            //--------------------------------------------------------------------------
            GetRequirementVM();

            //--------------------------------------------------------------------------

            if (postedFiles0 != null)
            {
                _unit.Save();
            }

            if (step == "laststep")
            {
                return RedirectToAction("index", "Home");
            }
            else
            {
                return RedirectToAction("index", "RequirementFile");
            }
        }



        //------------------------------------------
        //helper METHODS ---------------------------
        public ActionResult ViewImage(string fileName)
        {
            if (fileName != null)
            {
                string path = Path.Combine(_environment.WebRootPath + "\\RequirementFiles", fileName);
                string contentType = GetContentType(fileName);

                if (System.IO.File.Exists(path))
                {
                    return File(System.IO.File.ReadAllBytes(path), contentType);
                }
                else
                {
                    TempData["failed"] = "File Not Found";
                    return NotFound();
                }
            }
            else
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
        }

        public ActionResult ViewPdf(string fileName)
        {
            string path = Path.Combine(_environment.WebRootPath + "\\RequirementFiles", fileName);

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

        public ActionResult ViewTemplatePdf(string fileName)
        {
            string path = Path.Combine(_environment.WebRootPath + "\\RequirementFiles" + "\\TemplateFiles", fileName);

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

        private string GetContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName)?.ToLowerInvariant();

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                // add more cases for other image formats as needed
                default:
                    return "application/octet-stream";
            }
        }

        public IActionResult PermissionDenied()
        {
            return View();
        }

        public RequirementVM GetRequirementVM()
        {
            // Get the user's identity claims
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Retrieve all relevant forms and files for the user
            var getAllForm = _unit.RequirementForm.GetAll(u => u.UserId == claim.Value);
            var getAllFile = _unit.RequirementFile.GetAll(u => u.UserId == claim.Value);

            // Initialize variables to hold file IDs
            string form1FileId = null;
            string form1FileId2 = null;
            string form1FileId3 = null;
            string form1FileId4 = null;
            string form1FileId5 = null;

            if (getAllFile.Any())
            {
                foreach (var form in getAllForm)
                {
                    if (form.FormNumber == 1 && getAllFile != null) //1
                    {
                        var file = getAllFile.FirstOrDefault(u => u.FileId == form.FileId);
                        if (file != null)
                        {
                            form1FileId = file.FileId;
                        }
                    }
                    if (form.FormNumber == 2 && getAllFile != null) //2
                    {
                        var file = getAllFile.FirstOrDefault(u => u.FileId == form.FileId);
                        if (file != null)
                        {
                            form1FileId2 = file.FileId;
                        }
                    }
                    if (form.FormNumber == 3 && getAllFile != null) //3
                    {
                        var file = getAllFile.FirstOrDefault(u => u.FileId == form.FileId);
                        if (file != null)
                        {
                            form1FileId3 = file.FileId;
                        }
                    }
                    if (form.FormNumber == 4 && getAllFile != null) //4
                    {
                        var file = getAllFile.FirstOrDefault(u => u.FileId == form.FileId);
                        if (file != null)
                        {
                            form1FileId4 = file.FileId;
                        }
                    }
                    if (form.FormNumber == 5 && getAllFile != null) //5
                    {
                        var file = getAllFile.FirstOrDefault(u => u.FileId == form.FileId);
                        if (file != null)
                        {
                            form1FileId5 = file.FileId;
                        }
                    }
                }
            }

            requirementVM = new RequirementVM
            {
                FileName1 = form1FileId,
                FileName2 = form1FileId2,
                FileName3 = form1FileId3,
                FileName4 = form1FileId4,
                FileName5 = form1FileId5,
            };

            return requirementVM;
        }
	}
}
