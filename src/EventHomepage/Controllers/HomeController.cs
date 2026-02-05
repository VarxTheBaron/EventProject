using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventHomepage.Models;
using EventInfrastructure.Data;

namespace EventHomepage.Controllers;

public class HomeController(EventDbContext _db) : Controller
{
    public IActionResult Index()
    {
        var events = _db.Events.Where(e => e.StartDateTime > DateTime.Now).Select(e => new EventListViewModel(e)).ToList();

        return View(events);
    }

    [HttpGet, ActionName("Events")]
    public IActionResult Details(int? id)
    {
        if (id == null || id <= 0) return NotFound();

        var _event = _db.Events.FirstOrDefault(e => e.Id == id);
        if (_event == null) return NotFound();

        return View("Details", new EventDetailsViewModel(_event));
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
