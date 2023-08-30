using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
