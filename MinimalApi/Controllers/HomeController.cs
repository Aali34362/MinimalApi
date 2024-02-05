using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
