using Service.API;

namespace Service.Implementation;

internal class StateDTO : IStateDTO
{
    public int Id { get; set; }

    public int productId { get; set; }

    public int productQuantity { get; set; }

    public StateDTO(int id, int productId, int productQuantity)
    {
        this.Id = id;
        this.productId = productId;
        this.productQuantity = productQuantity;
    }
}
