using Microsoft.AspNetCore.Mvc;

namespace AnnouncementWebAPI
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
