using ASPDotNet_Cinema.Data;
using ASPDotNet_Cinema.Enums;
using ASPDotNet_Cinema.Models;
using ASPDotNet_Cinema.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Index(DateRange? dateRange, string sortOrder)
        {
            if (dateRange == null)
            {
                dateRange = DateRange.Today;
            }

            ViewData["CurrentSort"] = sortOrder;
            //ViewData["StartTimeSortParam"] = sortOrder ?? "StartTime";
            ViewData["StartTimeSortParam"] = sortOrder == "StartTime" ? "StartTime_desc" : "StartTime";
            ViewData["MovieTitleSortParam"] = sortOrder == "MovieTitle" ? "MovieTitle_desc" : "MovieTitle";
            ViewData["RankingSortParam"] = sortOrder == "MovieRanking" ? "MovieRanking_desc" : "MovieRanking";

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "StartTime";
            }

            GetDates(dateRange, out DateTime startDate, out DateTime endDate);

            return View(new HomeIndexViewModel { Range = dateRange.Value, Screenings = await GetScreeningsWithSums(startDate, endDate, sortOrder) });
        }

        private static void GetDates(DateRange? dateRange, out DateTime startDate, out DateTime endDate)
        {
            DateTime now = DateTime.Now;
            switch (dateRange.Value)
            {
                case DateRange.ThisWeek:
                    GetWeekDates(now, out _, out endDate);
                    startDate = now;    // films die al voorbij zijn niet tonen
                    break;
                case DateRange.NextWeek:
                    GetWeekDates(now.AddDays(7), out startDate, out endDate);
                    break;
                //case DateRange.CustomWeek:
                //    GetWeek(currentDate.AddDays(customOffset), out startOfWeek, out endOfWeek);
                //    //todo: checken of datum niet in het verleden ligt  
                //    break;
                case DateRange.Today:
                default:
                    startDate = now;
                    endDate = startDate.AddHours(23).AddMinutes(59);
                    break;
            }
        }

        /// <summary>
        /// Calculates start- and end date of current week.
        /// </summary>
        /// <param name="startOfWeek">A <see cref="DateTime"/> with monday's date and time 00:00</param>
        /// <param name="endOfWeek">A <see cref="DateTime"/> with sundays's date and time 23:59</param>
        private static void GetWeekDates(DateTime now, out DateTime startOfWeek, out DateTime endOfWeek)
        {
            var offset = DayOfWeek.Monday - now.DayOfWeek;
            startOfWeek = now.AddDays(offset).Date;                         // maandag 00u00
            endOfWeek = startOfWeek.AddDays(6).AddHours(23).AddMinutes(59); // zondag 23u59
        }

        private async Task<List<ScreeningWithSum>> GetScreeningsWithSums(DateTime startDate, DateTime endDate, string sortOrder)
        {
            List<ScreeningWithSum> screeningWithSums = new List<ScreeningWithSum>();

            var screenings = _context.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                .Where(s => s.StartTime >= startDate && s.StartTime < endDate)
                ;

            var orderedScreenings = sortOrder switch
            {
                "MovieTitle" => screenings.OrderBy(s => s.Movie.Title),
                "MovieTitle_desc" => screenings.OrderByDescending(s => s.Movie.Title),
                "MovieRanking" => screenings.OrderBy(s => s.Movie.Ranking),
                "MovieRanking_desc" => screenings.OrderByDescending(s => s.Movie.Ranking),
                "StartTime_desc" => screenings.OrderByDescending(s => s.StartTime),
                _ => screenings.OrderBy(s => s.StartTime)
            };

            await orderedScreenings.ForEachAsync(screening =>
            {
                int amountOfReservationsForScreening = _context.Reservations.Where(r => r.ScreeningId == screening.Id)
                                                                            .Sum(r => r.Amount);
                int amountLeft = screening.Screen.Capacity - amountOfReservationsForScreening;
                screeningWithSums.Add(new ScreeningWithSum
                {
                    Screening = screening,
                    TicketsLeft = amountLeft
                });
            });

            return screeningWithSums;
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
