using Presentation.Model.API;

namespace Presentation.Model.Implementation;

internal class MovieModel : IMovieModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public int AgeRestriction { get; set; }

    public MovieModel(int id, string name, double price, int ageRestriction)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
        this.AgeRestriction = ageRestriction;
    }
}
