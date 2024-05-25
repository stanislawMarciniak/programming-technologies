using Service.API;

namespace Service.Implementation;

internal class EventDTO : IEventDTO
{
    public int Id { get; set; }

    public int StateId { get; set; }

    public int UserId { get; set; }

    public DateTime OccurrenceDate { get; set; }

    public string Type { get; set; }

    public int? Quantity { get; set; }

    public EventDTO(int id, int stateId, int userId, DateTime occurrenceDate, string type, int? quantity)
    {
        this.Id = id;
        this.StateId = stateId;
        this.UserId = userId;
        this.OccurrenceDate = occurrenceDate;
        this.Type = type;
        this.Quantity = quantity;
    }
}
