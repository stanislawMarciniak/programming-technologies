using Service.API;

namespace PresentationTests;

internal class FakeProductCRUD : IProductCRUD
{
    private readonly FakeDataRepository _fakeRepository = new FakeDataRepository();

    public FakeProductCRUD()
    {

    }

    public async Task AddProductAsync(int id, string name, double price, int pegi)
    {
        await this._fakeRepository.AddProductAsync(id, name, price, pegi);
    }

    public async Task<IProductDTO> GetProductAsync(int id)
    {
        return await this._fakeRepository.GetProductAsync(id);
    }

    public async Task UpdateProductAsync(int id, string name, double price, int pegi)
    {
        await this._fakeRepository.UpdateProductAsync(id, name, price, pegi);
    }

    public async Task DeleteProductAsync(int id)
    {
        await this._fakeRepository.DeleteProductAsync(id);
    }

    public async Task<Dictionary<int, IProductDTO>> GetAllProductsAsync()
    {
        Dictionary<int, IProductDTO> result = new Dictionary<int, IProductDTO>();

        foreach (IProductDTO product in (await this._fakeRepository.GetAllProductsAsync()).Values)
        {
            result.Add(product.Id, product);
        }

        return result;
    }

    public async Task<int> GetProductsCountAsync()
    {
        return await this._fakeRepository.GetProductsCountAsync();
    }
}
