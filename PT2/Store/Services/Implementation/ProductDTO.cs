using Service.API;

namespace Service.Implementation;

internal class MovieDTO : IMovieDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public int AgeRestriction { get; set; }

    public MovieDTO(int id, string name, double price, int ageRestriction)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
        this.AgeRestriction = ageRestriction;
    }
}
