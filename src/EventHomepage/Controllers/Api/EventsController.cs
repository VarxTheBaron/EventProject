using EventCore.Entities;
using EventInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using SQLitePCL;

namespace EventHomepage.Controllers.Api;

[ApiController]
[Route("/api/v1/[controller]")]
public class EventsController(EventDbContext _db) : ControllerBase
{
    private string _correctPw = "ThinkpadsOverMac";

    [HttpPost]
    public async Task<IActionResult> CreateEvent(EventDTO dto)
    {
        StringValues pw;

        if (!Request.Headers.TryGetValue("password", out pw))
        {
            return Unauthorized();
        }

        if (pw != _correctPw)
        {
            return Unauthorized();
        }

        try
        {
            Event newEvent = new Event(
                dto.Name, dto.Description, dto.StartDateTime,
                dto.EndDateTime, dto.Location, dto.MaxParticipants);

            _db.Events.Add(newEvent);

            await _db.SaveChangesAsync();

            return Created($"/api/v1/events/{newEvent.Id}", newEvent);
        }
        catch
        {
            return BadRequest("Something went wrong when adding the new event.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        StringValues pw;

        if (!Request.Headers.TryGetValue("password", out pw))
        {
            return Unauthorized();
        }

        if (pw != _correctPw)
        {
            return Unauthorized();
        }

        if (await _db.Events.Where(e => e.Id == id).ExecuteDeleteAsync() == 0)
            return NotFound();

        await _db.Registrations.Where(r => r.EventId == id).ExecuteDeleteAsync();
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, EventDTO dto)
    {
        StringValues pw;

        if (!Request.Headers.TryGetValue("password", out pw))
        {
            return Unauthorized();
        }

        if (pw != _correctPw)
        {
            return Unauthorized();
        }

        var _event = await _db.Events.FindAsync(id);

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