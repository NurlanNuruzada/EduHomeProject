using Microsoft.AspNetCore.Mvc;

namespace HomeEdu.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
