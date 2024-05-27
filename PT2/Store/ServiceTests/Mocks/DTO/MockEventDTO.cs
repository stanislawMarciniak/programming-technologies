using Data.API;

namespace ServiceTests.Mocks.DTO;

internal class MockEventDTO : IEvent
{
    public MockEventDTO(int id, int stateId, int userId, string type, int? quantity = 0)
    {
        Id = id;
        this.stateId = stateId;
        this.userId = userId;
        occurrenceDate = DateTime.Now;
        Type = type;
        Quantity = quantity;
    }

    public int Id { get; set; }
    public int stateId { get; set; }
    public int userId { get; set; }
    public DateTime occurrenceDate { get; set; }
    public string Type { get; set; }
    public int? Quantity { get; set; }
}
