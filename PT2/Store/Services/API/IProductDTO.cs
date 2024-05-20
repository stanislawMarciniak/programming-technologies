namespace Service.API;

public interface IProductDTO
{
    int Id { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int Pegi { get; set; }
}
