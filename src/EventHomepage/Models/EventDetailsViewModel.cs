using EventCore.Entities;

namespace EventHomepage.Models;

public class EventDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? MaxParticipants { get; set; }
    public int ParticipantCount { get; set; }

    public EventDetailsViewModel(Event _event)
    {
        Id = _event.Id;
        Name = _event.Name;
        Description = _event.Description;
        StartDateTime = _event.StartDateTime;
        EndDateTime = _event.StartDateTime;
        Location = _event.Location;
        MaxParticipants = _event.MaxParticipants;
        ParticipantCount = _event.Registrations.Count;
    }
}