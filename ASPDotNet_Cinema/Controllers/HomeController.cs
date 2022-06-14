using Microsoft.AspNetCore.Mvc;

namespace ASPDotNet_Cinema.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToActionPermanent(nameof(MoviesController.Index), nameof(MoviesController).Replace("Controller", ""));
        }
    }
}
