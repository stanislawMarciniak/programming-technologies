using Service.API;

namespace PresentationTests.Mocks.CRUD
{
    internal class MockProductCRUD : IProductCRUD
    {
        private readonly MockDataRepository _repository = new MockDataRepository();

        public MockProductCRUD()
        {

        }

        public async Task AddProductAsync(int id, string name, double price, int ageRestriction)
        {
            await _repository.AddProductAsync(id, name, price, ageRestriction);
        }

        public async Task<IProductDTO> GetProductAsync(int id)
        {
            return await _repository.GetProductAsync(id);
        }

        public async Task UpdateProductAsync(int id, string name, double price, int ageRestriction)
        {
            await _repository.UpdateProductAsync(id, name, price, ageRestriction);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _repository.DeleteProductAsync(id);
        }

        public async Task<Dictionary<int, IProductDTO>> GetAllProductsAsync()
        {
            Dictionary<int, IProductDTO> result = new Dictionary<int, IProductDTO>();

            foreach (IProductDTO movie in (await _repository.GetAllProductsAsync()).Values)
            {
                result.Add(movie.Id, movie);
            }

            return result;
        }

        public async Task<int> GetProductsCountAsync()
        {
            return await _repository.GetProductsCountAsync();
        }
    }
}