using EventCore.Entities;

namespace EventHomepage.Models;

public class EventListViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public EventListViewModel(Event _event)
    {
        Name = _event.Name;
        Description = _event.Description;
        Location = _event.Location;
    }
}