using Data.API;
using Service.API;

namespace Service.Implementation;

internal class ProductCRUD : IProductCRUD
{
    private IDataRepository _repository;

    public ProductCRUD(IDataRepository repository)
    {
        this._repository = repository;
    }

    private IProductDTO Map(IMovie movie)
    {
        return new ProductDTO(movie.Id, movie.MovieName, movie.Price, movie.AgeRestriction);
    }

    public async Task AddProductAsync(int id, string name, double price, int ageRestriction)
    {
        await this._repository.AddProductAsync(id, name, price, ageRestriction);
    }

    public async Task<IProductDTO> GetProductAsync(int id)
    {
        return this.Map(await this._repository.GetProductAsync(id));
    }

    public async Task UpdateProductAsync(int id, string name, double price, int ageRestriction)
    {
        await this._repository.UpdateProductAsync(id, name, price, ageRestriction);
    }

    public async Task DeleteProductAsync(int id)
    {
        await this._repository.DeleteProductAsync(id);
    }

    public async Task<Dictionary<int, IProductDTO>> GetAllProductsAsync()
    {
        Dictionary<int, IProductDTO> result = new Dictionary<int, IProductDTO>();

        foreach (IMovie movie in (await this._repository.GetAllProductsAsync()).Values)
        {
            result.Add(movie.Id, this.Map(movie));
        }

        return result;
    }

    public async Task<int> GetProductsCountAsync()
    {
        return await this._repository.GetProductsCountAsync();
    }
}
