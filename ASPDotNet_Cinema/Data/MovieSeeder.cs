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
            //await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();

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
                var vanavond23u = new DateTime(nu.Year, nu.Month, nu.Day, 23, 0, 0);

                var morgen = vanavond20u.AddDays(1);
                var overmorgen = vanavond20u.AddDays(2);

                var volgendeWeek = vanavond20u.AddDays(7);
                var volgendeWeek1 = vanavond20u.AddDays(8);
                var volgendeWeek2 = vanavond20u.AddDays(9);

                var screenings = new Screening[]
                {
                    new Screening { ScreenId = 1, Movie = movies[0], StartTime = vanavond20u },
                    new Screening { ScreenId = 2, Movie = movies[0], StartTime = vanavond22u },
                    new Screening { ScreenId = 1, Movie = movies[1], StartTime = vanavond20u },
                    new Screening { ScreenId = 3, Movie = movies[1], StartTime = vanavond23u },
                    new Screening { ScreenId = 1, Movie = movies[1], StartTime = morgen },
                    new Screening { ScreenId = 2, Movie = movies[2], StartTime = morgen },
                    new Screening { ScreenId = 2, Movie = movies[2], StartTime = overmorgen },
                    new Screening { ScreenId = 1, Movie = movies[0], StartTime = volgendeWeek },
                    new Screening { ScreenId = 2, Movie = movies[1], StartTime = volgendeWeek1 },
                    new Screening { ScreenId = 3, Movie = movies[2], StartTime = volgendeWeek2 }
                };

                context.Screenings.AddRange(screenings);

                context.Reservations.Add(new Reservation { Screening = screenings[1], Amount = 100 });

                await context.SaveChangesAsync();

            }
        }
    }
}
