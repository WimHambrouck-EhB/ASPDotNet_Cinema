using ASPDotNet_Cinema.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASPDotNet_Cinema.Data
{
    public static class MovieSeeder
    {
        public static async Task Initialize(CinemaIdentityContext context)
        {
            if (!context.Movies.Any() && !context.Screens.Any() && !context.Screenings.Any())
            {
                Movie[] movies =
                {
                    new Movie{ Title = "De sterrenoorlog", Director = "George Lucas", Ranking = 8.6m, Duration = 121 },
                    new Movie{ Title = "De tegenaanval van het rijk", Director = "Irvin Kershner", Ranking = 8.7m, Duration = 124 },
                    new Movie{ Title = "De terugkeer van de Jedi", Director = "Richard Marquand", Ranking = 8.3m, Duration = 131 },
                };

                context.Movies.AddRange(movies);

                Screen[] screens =
                {
                    new Screen { Number = 1, Capacity = 150},
                    new Screen { Number = 2, Capacity = 100},
                    new Screen { Number = 3, Capacity = 15}
                };

                context.Screens.AddRange(screens);

                var nu = DateTime.Now;
                var vanavond20u = new DateTime(nu.Year, nu.Month, nu.Day, 20, 0, 0);
                var vanavond22u = new DateTime(nu.Year, nu.Month, nu.Day, 22, 0, 0);

                var morgen = new DateTime(nu.Year, nu.Month, nu.Day + 1, 20, 0, 0);
                var overmorgen = new DateTime(nu.Year, nu.Month, nu.Day + 2, 20, 0, 0);

                var volgendeWeek = new DateTime(nu.Year, nu.Month, nu.Day + 7, 20, 0, 0);
                var volgendeWeek1 = new DateTime(nu.Year, nu.Month, nu.Day + 8, 20, 0, 0);
                var volgendeWeek2 = new DateTime(nu.Year, nu.Month, nu.Day + 9, 20, 0, 0);


                context.Screenings.AddRange(new Screening[]
                {
                    new Screening { ScreenId = 1, Movie = movies[0], StartTime = vanavond20u },
                    new Screening { ScreenId = 2, Movie = movies[0], StartTime = vanavond22u },
                    new Screening { ScreenId = 3, Movie = movies[1], StartTime = vanavond22u }
                });

                await context.SaveChangesAsync();

            }
        }
    }
}
