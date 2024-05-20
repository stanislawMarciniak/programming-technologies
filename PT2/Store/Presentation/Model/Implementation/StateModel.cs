using Presentation.Model.API;

namespace Presentation.Model.Implementation;

internal class StateModel : IStateModel
{
    public int Id { get; set; }

    public int productId { get; set; }

    public int productQuantity { get; set; }

    public StateModel(int id, int productId, int productQuantity)
    {
        this.Id = id;
        this.productId = productId;
        this.productQuantity = productQuantity;
    }
}
