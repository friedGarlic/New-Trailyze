using Microsoft.AspNetCore.Mvc;
using ML_ASP.DataAccess.Repositories.IRepositories;
using System.Security.Claims;

namespace ML_ASP.Controllers
{
    public class RequirementController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        public RequirementController(IUnitOfWork unit, IWebHostEnvironment environment)
        {
            _unit = unit;
            _environment = environment;

        }

        [HttpPost]
        public ActionResult UploadRequirementFile(IFormFile fileEnrollment, IFormFile fileMedical)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var account = _unit.Account.GetFirstOrDefault(u => u.Id == claim.Value);

            //----------------FIRST FILE
            if (fileEnrollment != null && fileEnrollment.Length > 0 && fileMedical != null && fileMedical.Length > 0)
            {
                string projectPath = _environment.WebRootPath;
                string uploadFolderName = "RequirementFiles";
                var uploads = Path.Combine(projectPath, uploadFolderName);

                string fileName = Guid.NewGuid().ToString();
                string secondFileName = Guid.NewGuid().ToString();

                var extension = Path.GetExtension(fileEnrollment.FileName);
                var secondExtension = Path.GetExtension(fileMedical.FileName);

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                string newEnrollmentFileName = fileName + extension;
                string newMedicalFileName = secondFileName + secondExtension;

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    fileEnrollment.CopyTo(fileStream);
                }
                using (var fileStream = new FileStream(Path.Combine(uploads, secondFileName + secondExtension), FileMode.Create))
                {
                    fileMedical.CopyTo(fileStream);
                }

                try
                {
                    _unit.Account.UpdateRequirementFile(newEnrollmentFileName, newMedicalFileName, account.Id);

                    _unit.Save();
                }
                catch { }
            }

            return View();
        }
    }
}
