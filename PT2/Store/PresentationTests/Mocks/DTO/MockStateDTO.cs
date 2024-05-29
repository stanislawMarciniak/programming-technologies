using Service.API;

namespace PresentationTests.Mocks.DTO
{
    internal class MockStateDTO : IStateDTO
    {
        public MockStateDTO(int id, int movieId, int movieQuantity = 0)
        {
            Id = id;
            movieId = movieId;
            movieQuantity = movieQuantity;
        }

        public int Id { get; set; }
        public int movieId { get; set; }
        public int movieQuantity { get; set; }
    }
}