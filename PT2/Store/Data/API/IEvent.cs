namespace Data.API;

public interface IEvent
{
    int Id { get; set; }

    int stateId { get; set; }

    int userId { get; set; }

    DateTime occurrenceDate { get; set; }

    public string Type { get; set; }

    public int? Quantity { get; set; }
}
