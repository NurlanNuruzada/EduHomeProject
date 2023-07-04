using Microsoft.AspNetCore.Mvc;

namespace HomeEdu.UI.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
