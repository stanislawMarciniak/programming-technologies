using Service.API;

namespace Service.Implementation;

internal class StateDTO : IStateDTO
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ProductQuantity { get; set; }

    public StateDTO(int id, int productId, int productQuantity)
    {
        this.Id = id;
        this.ProductId = productId;
        this.ProductQuantity = productQuantity;
    }
}
