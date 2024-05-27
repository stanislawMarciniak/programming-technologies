using Data.API;

namespace Data.Implementation;

internal class Movie : IMovie
{
    public Movie(int id, string movieName, double price, int ageRestriction)
    {
        this.Id = id;
        this.MovieName = movieName;
        this.Price = price;
        this.AgeRestriction = ageRestriction;
    }

    public int Id { get; set; }

    public string MovieName { get; set; }

    public double Price { get; set; }

    public int AgeRestriction { get; set; }
}
