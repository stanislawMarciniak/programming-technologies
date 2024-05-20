namespace Data.API;

public interface IProduct
{
    int Id { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int Pegi { get; set; }
}
