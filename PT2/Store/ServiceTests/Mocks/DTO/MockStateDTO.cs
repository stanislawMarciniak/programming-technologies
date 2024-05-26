using Data.API;

namespace ServiceTests.Mocks.DTO;

internal class MockStateDTO : IState
{
    public MockStateDTO(int id, int productId, int productQuantity = 0)
    {
        Id = id;
        productId = productId;
        productQuantity = productQuantity;
    }

    public int Id { get; set; }
    public int productId { get; set; }
    public int productQuantity { get; set; }
}
