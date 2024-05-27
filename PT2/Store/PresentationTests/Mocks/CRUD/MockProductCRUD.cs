using Service.API;

namespace PresentationTests.Mocks.CRUD
{
    internal class MockProductCRUD : IProductCRUD
    {
        private readonly MockDataRepository _repository = new MockDataRepository();

        public MockProductCRUD()
        {

        }

        public async Task AddProductAsync(int id, string name, double price, int pegi)
        {
            await _repository.AddProductAsync(id, name, price, pegi);
        }

        public async Task<IProductDTO> GetProductAsync(int id)
        {
            return await _repository.GetProductAsync(id);
        }

        public async Task UpdateProductAsync(int id, string name, double price, int pegi)
        {
            await _repository.UpdateProductAsync(id, name, price, pegi);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _repository.DeleteProductAsync(id);
        }

        public async Task<Dictionary<int, IProductDTO>> GetAllProductsAsync()
        {
            Dictionary<int, IProductDTO> result = new Dictionary<int, IProductDTO>();

            foreach (IProductDTO product in (await _repository.GetAllProductsAsync()).Values)
            {
                result.Add(product.Id, product);
            }

            return result;
        }

        public async Task<int> GetProductsCountAsync()
        {
            return await _repository.GetProductsCountAsync();
        }
    }
}