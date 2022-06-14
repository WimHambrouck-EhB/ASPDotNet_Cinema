using ASPDotNet_Cinema.Data;
using ASPDotNet_Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ASPDotNet_Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly CinemaIdentityContext _context;

        public HomeController(CinemaIdentityContext context)
        {
            _context = context;

        }
        public async Task<IActionResult>Index()
        {
            var screenings = _context.Screenings
                .Include(s => s.Movie)
                ;
            return View(await screenings.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
