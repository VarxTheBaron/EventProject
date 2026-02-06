using EventCore.Entities;
using EventInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {

        if (await _db.Events.Where(e => e.Id == id).ExecuteDeleteAsync() == 0)
            return NotFound();

        await _db.Registrations.Where(r => r.EventId == id).ExecuteDeleteAsync();
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, EventDTO dto)
    {
        var _event = _db.Events.Find(id);

        if (_event == null) return NotFound();

        try
        {
            var validate = new Event(dto.Name, dto.Description, dto.StartDateTime, dto.EndDateTime, dto.Location, dto.MaxParticipants);
        }
        catch
        {
            return BadRequest();
        }

        _db.Events.Entry(_event).CurrentValues.SetValues(dto);

        await _db.SaveChangesAsync();

        return NoContent();
    }
}