using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventHomepage.Models;
using EventInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventHomepage.Controllers;

public class HomeController(EventDbContext _db) : Controller
{
    public IActionResult Index()
    {
        var events = _db.Events.Where(e => e.StartDateTime > DateTime.UtcNow).Select(e => new EventListViewModel(e)).ToList();

        return View(events);
    }

    [HttpGet, ActionName("Events")]
    public IActionResult Details(int? id)
    {
        if (id == null || id <= 0) return NotFound();

        var _event = _db.Events.Where(e => e.Id == id).Include(e => e.Registrations).FirstOrDefault();
        if (_event == null) return NotFound();

        return View("Details", new EventDetailsViewModel(_event));
    }

    [HttpPost]
    public async Task<IActionResult> Register(int id, [Bind("ParticipantName,ParticipantEmail")] RegistrationsDto dto)
    {
        if (id <= 0) return BadRequest();
        var _event = _db.Events.Include(e => e.Registrations).FirstOrDefault(e => e.Id == id);
        if (_event == null) return BadRequest();


        try
        {
            var registration = _event.RegisterParticipant(dto.ParticipantName, dto.ParticipantEmail);
            await _db.Registrations.AddAsync(registration);
            await _db.SaveChangesAsync();
        }
        catch
        {
            TempData["RegistrationError"] = $"Error when creating registration!";
            return RedirectToAction("Events", new { id = _event.Id });
        }

        TempData["RegistrationSuccess"] = $"Successfully made a registration on event: {_event.Name}";
        return RedirectToAction("Events", new { id = _event.Id });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
