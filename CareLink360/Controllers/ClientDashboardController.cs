using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    [Authorize(Roles = "Client,Patient")]
    public class ClientDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

