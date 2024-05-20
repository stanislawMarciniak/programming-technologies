using Data.API;
using Service.Implementation;

namespace Service.API;

public interface IProductCRUD
{
    static IProductCRUD CreateProductCRUD(IDataRepository? dataRepository = null)
    {
        return new ProductCRUD(dataRepository ?? IDataRepository.CreateDatabase());
    }

    Task AddProductAsync(int id, string name, double price, int pegi);

    Task<IProductDTO> GetProductAsync(int id);

    Task UpdateProductAsync(int id, string name, double price, int pegi);

    Task DeleteProductAsync(int id);

    Task<Dictionary<int, IProductDTO>> GetAllProductsAsync();

    Task<int> GetProductsCountAsync();
}
