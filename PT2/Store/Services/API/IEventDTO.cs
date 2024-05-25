namespace Service.API;

public interface IEventDTO
{
    int Id { get; set; }

    int StateId { get; set; }

    int UserId { get; set; }

    DateTime OccurrenceDate { get; set; }

    string Type { get; set; }

    int? Quantity { get; set; }
}
