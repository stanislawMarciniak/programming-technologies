using Service.API;

namespace PresentationTests.Mocks.DTO
{
    internal class MockStateDTO : IStateDTO
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
}