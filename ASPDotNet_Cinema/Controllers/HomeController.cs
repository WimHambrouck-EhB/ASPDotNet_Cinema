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
        public async Task<IActionResult> Index()
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

        /// <summary>
        /// Triggered when typing the konami code on any page
        /// </summary>
        /// <returns>Your worst nightmare</returns>
        public async Task SelfDestruct()
        {
            var theWorstMovieEverMade = new Movie { Title = "The Star Wars Holiday Special", Director = "Steve Binder", Ranking = 2.1m, Duration = 97 };
            _context.Movies.RemoveRange(_context.Movies); // uiterst inefficiënt, maar gezien het een spielerei betreft, ga ik nu ook niet te veel moeite doen
            _context.Movies.Add(theWorstMovieEverMade);
            var screenings = await _context.Screenings.ToListAsync();
            foreach (var screening in screenings)
            {
                screening.Movie = theWorstMovieEverMade;
            }
            await _context.SaveChangesAsync();
        }
    }
}
