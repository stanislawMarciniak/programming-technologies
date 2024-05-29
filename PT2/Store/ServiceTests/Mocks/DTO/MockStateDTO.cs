using Data.API;

namespace ServiceTests.Mocks.DTO;

internal class MockStateDTO : IState
{
    public MockStateDTO(int id, int movieId, int movieQuantity = 0)
    {
        this.Id = id;
        this.movieId = movieId;
        this.movieQuantity = movieQuantity;
    }

    public int Id { get; set; }
    public int movieId { get; set; }
    public int movieQuantity { get; set; }
}
