namespace Data.API;

public interface IMovie
{
    int Id { get; set; }
    string MovieName { get; set; }
    double Price { get; set; }
    int AgeRestriction { get; set; }
}
