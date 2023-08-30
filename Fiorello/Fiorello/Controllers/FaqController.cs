using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Controllers
{
    public class FaqController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
