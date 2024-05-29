using Presentation.Model.API;

namespace Presentation.Model.Implementation;

internal class StateModel : IStateModel
{
    public int Id { get; set; }

    public int movieId { get; set; }

    public int movieQuantity { get; set; }

    public StateModel(int id, int movieId, int movieQuantity)
    {
        this.Id = id;
        this.movieId = movieId;
        this.movieQuantity = movieQuantity;
    }
}
