using Microsoft.AspNetCore.Mvc;

namespace BlogAPP.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Error(int? id)
        {
            return View();
        }
        
    }
}
