using Presentation.Model.API;
using Service.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Model.Implementation;

internal class ProductModelOperation : IProductModelOperation
{
    private IProductCRUD _productCRUD;

    public ProductModelOperation(IProductCRUD? productCrud = null)
    {
        this._productCRUD = productCrud ?? IProductCRUD.CreateProductCRUD();
    }

    private IProductModel Map(IProductDTO movie)
    {
        return new ProductModel(movie.Id, movie.Name, movie.Price, movie.AgeRestriction);
    }

    public async Task AddAsync(int id, string name, double price, int ageRestriction)
    {
        await this._productCRUD.AddProductAsync(id, name, price, ageRestriction);
    }

    public async Task<IProductModel> GetAsync(int id)
    {
        return this.Map(await this._productCRUD.GetProductAsync(id));
    }

    public async Task UpdateAsync(int id, string name, double price, int ageRestriction)
    {
        await this._productCRUD.UpdateProductAsync(id, name, price, ageRestriction);
    }

    public async Task DeleteAsync(int id)
    {
        await this._productCRUD.DeleteProductAsync(id);
    }

    public async Task<Dictionary<int, IProductModel>> GetAllAsync()
    {
        Dictionary<int, IProductModel> result = new Dictionary<int, IProductModel>();

        foreach (IProductDTO movie in (await this._productCRUD.GetAllProductsAsync()).Values)
        {
            result.Add(movie.Id, this.Map(movie));
        }

        return result;
    }

    public async Task<int> GetCountAsync()
    {
        return await this._productCRUD.GetProductsCountAsync();
    }
}
