using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPDotNet_Cinema.Data;
using ASPDotNet_Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ASPDotNet_Cinema.Controllers
{
    [Authorize(Roles = CinemaUser.STAFF_ROLE)]
    public class ReservationsController : Controller
    {
        private readonly CinemaIdentityContext _context;
        private readonly ILogger<ReservationsController> _logger;

        public ReservationsController(CinemaIdentityContext context, ILogger<ReservationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var aSPDotNet_CinemaContext = _context.Reservations.Include(r => r.Screening);
            return View(await aSPDotNet_CinemaContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Screening)
                .ThenInclude(s => s.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            if (User.IsInRole(CinemaUser.STAFF_ROLE))
            {
                return View(reservation);
            }
            else
            {
                return View("Confirmation", reservation);
            }
        }

        // GET: Reservations/Create
        [AllowAnonymous]
        public async Task<IActionResult> Create(int? screeningId)
        {
            var screening = await _context.Screenings.FindAsync(screeningId);
            if (screening == null || screening.StartTime < DateTime.Now)
            {
                return NotFound();
            }

            var screenings = _context.Screenings.Include(s => s.Movie)
                                                .Where(s => s.StartTime >= DateTime.Now)
                                                .OrderBy(s => s.StartTime);
            ViewData["ScreeningId"] = new SelectList(screenings, "Id", "FullName", screeningId);
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("Id,ScreeningId,Amount")] Reservation reservation)
        {
            var screening = await _context.Screenings
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(s => s.Id == reservation.ScreeningId);

            if (screening != null)
            {
                int amountOfReservationsForScreening = await _context.Reservations.Where(r => r.ScreeningId == reservation.ScreeningId).SumAsync(r => r.Amount);
                int amountLeft = screening.Screen.Capacity - amountOfReservationsForScreening;
                if (amountLeft - reservation.Amount < 0)
                {
                    string errorMessage;
                    if (amountLeft == 0)
                    {
                        errorMessage = "There are no tickets left for this screening.";
                    }
                    else if (amountLeft == 1)
                    {
                        errorMessage = "There is only 1 ticket left for this screening.";
                    }
                    else
                    {
                        errorMessage = $"There are only {amountLeft} tickets left for this screening, please reduce the amount of tickets.";
                    }
                    ModelState.AddModelError("Amount", errorMessage);
                }
            }
            else
            {
                // kan normaal niet gebeuren, maar voor de zekerheid toch even fout afhandelen
                ModelState.AddModelError("ScreeningId", "Screening was not found. Try again or contact staff if this keeps happening.");
                _logger.LogError("Tried to create Reservation for screening with id {0}, but could not find Screening in db.", reservation.ScreeningId);
            }

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                if (User.IsInRole(CinemaUser.STAFF_ROLE))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Details), new { id = reservation.Id }); ;
                }
            }

            var screenings = _context.Screenings.Include(s => s.Movie)
                                    .Where(s => s.StartTime >= DateTime.Now)
                                    .OrderBy(s => s.StartTime);
            ViewData["ScreeningId"] = new SelectList(screenings, "Id", "FullName", reservation.ScreeningId);

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ScreeningId"] = new SelectList(_context.Screenings, "Id", "Id", reservation.ScreeningId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScreeningId,Amount")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScreeningId"] = new SelectList(_context.Screenings, "Id", "Id", reservation.ScreeningId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Screening)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
