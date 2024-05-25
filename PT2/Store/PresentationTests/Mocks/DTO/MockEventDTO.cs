using Service.API;

namespace PresentationTests.Mocks.DTO
{
    internal class MockEventDTO : IEventDTO
    {
        public MockEventDTO(int id, int stateId, int userId, string type, int? quantity = 0)
        {
            Id = id;
            StateId = stateId;
            UserId = userId;
            OccurrenceDate = DateTime.Now;
            Type = type;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public int StateId { get; set; }
        public int UserId { get; set; }
        public DateTime OccurrenceDate { get; set; }
        public string Type { get; set; }
        public int? Quantity { get; set; }
    }
}
