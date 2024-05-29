using Service.API;

namespace Service.Implementation;

internal class StateDTO : IStateDTO
{
    public int Id { get; set; }

    public int movieId { get; set; }

    public int movieQuantity { get; set; }

    public StateDTO(int id, int movieId, int movieQuantity)
    {
        this.Id = id;
        this.movieId = movieId;
        this.movieQuantity = movieQuantity;
    }
}
