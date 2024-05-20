using Data.API;

namespace Data.Implementation;

internal class Event : IEvent
{
    public Event(int id, int stateId, int userId, DateTime occurrenceDate, string type, int? quantity = null)
    {
        this.Id = id;
        this.stateId = stateId;
        this.userId = userId;
        this.occurrenceDate = occurrenceDate;
        this.Type = type;
        this.Quantity = quantity;
    }

    public int Id { get; set; }

    public int stateId { get; set; }

    public int userId { get; set; }

    public DateTime occurrenceDate { get; set; }

    public string Type { get; set; }

    public int? Quantity { get; set; }
}
