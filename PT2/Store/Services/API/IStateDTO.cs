namespace Service.API;

public interface IStateDTO
{
    int Id { get; set; }

    int ProductId { get; set; }

    int ProductQuantity { get; set; }
}
