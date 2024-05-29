using Data.API;

namespace Data.Implementation;

internal class State : IState
{
    public State(int id, int movieId, int movieQuantity = 0) 
    {
        this.Id = id;
        this.movieId = movieId;
        this.movieQuantity = movieQuantity;
    }

    public int Id { get; set; }

    public int movieId { get; set; }

    public int movieQuantity { get; set; }
}
