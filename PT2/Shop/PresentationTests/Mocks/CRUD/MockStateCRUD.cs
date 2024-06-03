using Service.API;

namespace PresentationTests
{
    internal class MockStateCRUD : IStateCRUD
    {
        private readonly MockDataRepository _repository = new MockDataRepository();

        public MockStateCRUD()
        {

        }

        public async Task AddStateAsync(int id, int productId, int productQuantity)
        {
            await _repository.AddStateAsync(id, productId, productQuantity);
        }

        public async Task<IStateDTO> GetStateAsync(int id)
        {
            return await _repository.GetStateAsync(id);
        }

        public async Task UpdateStateAsync(int id, int productId, int productQuantity)
        {
            await _repository.UpdateStateAsync(id, productId, productQuantity);
        }

        public async Task DeleteStateAsync(int id)
        {
            await _repository.DeleteStateAsync(id);
        }

        public async Task<Dictionary<int, IStateDTO>> GetAllStatesAsync()
        {
            Dictionary<int, IStateDTO> result = new Dictionary<int, IStateDTO>();

            foreach (IStateDTO state in (await _repository.GetAllStatesAsync()).Values)
            {
                result.Add(state.Id, state);
            }

            return result;
        }

        public async Task<int> GetStatesCountAsync()
        {
            return await _repository.GetStatesCountAsync();
        }
    }
}