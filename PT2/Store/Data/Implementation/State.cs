using Data.API;

namespace Data.Implementation;

internal class State : IState
{
    public State(int id, int productId, int productQuantity = 0) 
    {
        this.Id = id;
        this.productId = productId;
        this.productQuantity = productQuantity;
    }

    public int Id { get; set; }

    public int productId { get; set; }

    public int productQuantity { get; set; }
}
