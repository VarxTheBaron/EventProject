using EventCore.Entities;
using EventInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace EventHomepage.Controllers.Api;

[ApiController]
[Route("/api/v1/[controller]")]
public class EventsController(EventDbContext _db) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateEvent(EventDTO dto)
    {
        try
        {
            Event newEvent = new Event(
                dto.Name, dto.Description, dto.StartDateTime,
                dto.EndDateTime, dto.Location, dto.MaxParticipants);

            _db.Events.Add(newEvent);

            await _db.SaveChangesAsync();

            return Created($"/api/v1/events/{newEvent.Id}", newEvent);
        }
        catch (Exception e)
        {
            return BadRequest("Something went wrong when adding the new event.");
        }
    }
}