namespace Presentation.Model.API;

public interface IProductModel
{
    int Id { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int Pegi { get; set; }
}
