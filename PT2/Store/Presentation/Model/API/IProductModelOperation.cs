using System.Collections.Generic;
using System.Threading.Tasks;
using Presentation.Model.Implementation;
using Service.API;

namespace Presentation.Model.API;

public interface IProductModelOperation
{
    static IProductModelOperation CreateModelOperation(IProductCRUD? productCrud = null)
    {
        return new ProductModelOperation(productCrud ?? IProductCRUD.CreateProductCRUD());
    }

    Task AddAsync(int id, string name, double price, int pegi);

    Task<IProductModel> GetAsync(int id);

    Task UpdateAsync(int id, string name, double price, int pegi);

    Task DeleteAsync(int id);

    Task<Dictionary<int, IProductModel>> GetAllAsync();

    Task<int> GetCountAsync();
}
