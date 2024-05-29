using Service.API;

namespace PresentationTests;

internal class FakeEventDTO : IEventDTO
{
    public FakeEventDTO(int id, int stateId, int userId, string type, int? quantity = 0)
    {
        this.Id = id;
        this.stateId = stateId;
        this.userId = userId;
        this.occurrenceDate = DateTime.Now;
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
