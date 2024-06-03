using Data.API;

namespace ServiceTests;

internal class MockProductDTO : IProduct
{
    public MockProductDTO(int id, string name, double price, int pegi)
    {
        Id = id;
        Name = name;
        Price = price;
        Pegi = pegi;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Pegi { get; set; }
}
