using Data.API;

namespace ServiceTests.Mocks.DTO;

internal class MockStateDTO : IState
{
    public MockStateDTO(int id, int productId, int productQuantity = 0)
    {
        this.Id = id;
        this.productId = productId;
        this.productQuantity = productQuantity;
    }

    public int Id { get; set; }
    public int productId { get; set; }
    public int productQuantity { get; set; }
}
