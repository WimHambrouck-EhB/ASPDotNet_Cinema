using ASPDotNet_Cinema.Data;
using ASPDotNet_Cinema.Enums;
using ASPDotNet_Cinema.Models;
using ASPDotNet_Cinema.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
        public async Task<IActionResult> Index(DateRange? dateRange)
        {
            if (dateRange == null)
            {
                dateRange = DateRange.Today;
            }

            DateTime startOfWeek, endOfWeek;
            DateTime currentDate = DateTime.Now;
            switch (dateRange.Value)
            {
                case DateRange.ThisWeek:
                    GetWeek(currentDate, out startOfWeek, out endOfWeek);
                    break;
                case DateRange.NextWeek:
                    GetWeek(currentDate.AddDays(7), out startOfWeek, out endOfWeek);
                    break;
                //case DateRange.CustomWeek:
                //    GetWeek(currentDate.AddDays(customOffset), out startOfWeek, out endOfWeek);
                //    break;
                case DateRange.Today:
                default:
                    startOfWeek = currentDate.Date;
                    endOfWeek = startOfWeek.AddHours(23).AddMinutes(59);
                    break;
            }

            var screenings = _context.Screenings.Include(s => s.Movie)
                                                .Where(s => s.StartTime >= startOfWeek && s.StartTime < endOfWeek)
                                                .OrderBy(s => s.StartTime);


            return View(new HomeIndexViewModel { Range = dateRange.Value, Screenings = await screenings.ToListAsync() });
        }

        /// <summary>
        /// Calculates start- and end date of current week.
        /// </summary>
        /// <param name="startOfWeek">A <see cref="DateTime"/> with monday's date and time 00:00</param>
        /// <param name="endOfWeek">A <see cref="DateTime"/> with sundays's date and time 23:59</param>
        private static void GetWeek(DateTime now, out DateTime startOfWeek, out DateTime endOfWeek)
        {
            var offset = DayOfWeek.Monday - now.DayOfWeek;
            startOfWeek = now.AddDays(offset).Date;                         // maandag 00u00
            endOfWeek = startOfWeek.AddDays(6).AddHours(23).AddMinutes(59); // zondag 23u59
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
