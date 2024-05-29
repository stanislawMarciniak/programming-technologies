namespace Service.API;

public interface IMovieDTO
{
    int Id { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int AgeRestriction { get; set; }
}
