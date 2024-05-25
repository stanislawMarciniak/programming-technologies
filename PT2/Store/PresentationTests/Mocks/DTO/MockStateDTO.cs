using Service.API;

namespace PresentationTests.Mocks.DTO
{
    internal class MockStateDTO : IStateDTO
    {
        public MockStateDTO(int id, int productId, int productQuantity = 0)
        {
            Id = id;
            ProductId = productId;
            ProductQuantity = productQuantity;
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}