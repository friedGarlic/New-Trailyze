using Microsoft.AspNetCore.Mvc;
using ML_ASP.DataAccess.Repositories.IRepositories;
using System.Security.Claims;

namespace ML_ASP.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unit;
        public NotificationController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public ActionResult GetAllNotification()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string id = claim.Value;

            var sublist = _unit.Notification
                  .GetAll(u => u.NotifUserId == id)
                  .ToList();

            return Json(new { data = sublist});
        }
        #endregion
    }
}
