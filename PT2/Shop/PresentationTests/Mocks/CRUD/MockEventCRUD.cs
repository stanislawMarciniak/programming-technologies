using Service.API;

namespace PresentationTests
{
    internal class MockEventCRUD : IEventCRUD
    {
        private readonly MockDataRepository _repository = new MockDataRepository();

        public MockEventCRUD()
        {

        }   

        public async Task AddEventAsync(int id, int stateId, int userId, string type, int quantity = 0)
        {
            await _repository.AddEventAsync(id, stateId, userId, type, quantity);
        }

        public async Task<IEventDTO> GetEventAsync(int id)
        {
            return await _repository.GetEventAsync(id);
        }

        public async Task UpdateEventAsync(int id, int stateId, int userId, DateTime occurrenceDate, string type, int? quantity)
        {
            await _repository.UpdateEventAsync(id, stateId, userId, occurrenceDate, type, quantity);
        }

        public async Task DeleteEventAsync(int id)
        {
            await _repository.DeleteEventAsync(id);
        }

        public async Task<Dictionary<int, IEventDTO>> GetAllEventsAsync()
        {
            Dictionary<int, IEventDTO> result = new Dictionary<int, IEventDTO>();

            foreach (IEventDTO even in (await _repository.GetAllEventsAsync()).Values)
            {
                result.Add(even.Id, even);
            }

            return result;
        }

        public async Task<int> GetEventsCountAsync()
        {
            return await _repository.GetEventsCountAsync();
        }
    }
}