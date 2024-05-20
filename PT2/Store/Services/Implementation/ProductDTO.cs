using Service.API;

namespace Service.Implementation;

internal class ProductDTO : IProductDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public int Pegi { get; set; }

    public ProductDTO(int id, string name, double price, int pegi)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
        this.Pegi = pegi;
    }
}
