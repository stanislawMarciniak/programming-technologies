using Data.API;

namespace ServiceTests;

internal class MockState : IState
{
    public MockState(int id, int productId, int productQuantity = 0)
    {
        Id = id;
        this.productId = productId;
        this.productQuantity = productQuantity;
    }

    public int Id { get; set; }
    public int productId { get; set; }
    public int productQuantity { get; set; }
}
