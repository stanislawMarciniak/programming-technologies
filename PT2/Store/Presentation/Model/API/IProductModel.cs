namespace Presentation.Model.API;

public interface IMovieModel
{
    int Id { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int AgeRestriction { get; set; }
}
