using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace ML_ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unit;

        public HomeController(ILogger<HomeController> logger,
            IEmailSender emailSender,
            IUnitOfWork unit)
        {
            _emailSender = emailSender;
            _logger = logger;
            _unit = unit;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var getAcc = _unit.Account.GetFirstOrDefault(x => x.Id == claim.Value);
                string? imageUrl = getAcc.ImageUrl;

                ViewData["ImageUrl"] = imageUrl;

                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult SendEmail(string fullName, string email, string message)
        {
            // var emailTo = "ycabasug@gmail.com";

			var emailTo = "remcarlmerza@gmail.com";
            _emailSender.SendEmailAsync( emailTo, "Information", "FullName: " + fullName + " Email: " + email + "\n" + message);

            return RedirectToAction(nameof(Index));
        }
    }
}