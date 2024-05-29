using Service.API;

namespace PresentationTests;

internal class FakeEventCRUD : IEventCRUD
{
    private readonly FakeDataRepository _fakeRepository = new FakeDataRepository();

    public FakeEventCRUD()
    {

    }
    
    public async Task AddEventAsync(int id, int stateId, int userId, string type, int quantity = 0)
    {
        await this._fakeRepository.AddEventAsync(id, stateId, userId, type, quantity);
    }

    public async Task<IEventDTO> GetEventAsync(int id)
    {
        return await this._fakeRepository.GetEventAsync(id);
    }

    public async Task UpdateEventAsync(int id, int stateId, int userId, DateTime occurrenceDate, string type, int? quantity)
    {
        await this._fakeRepository.UpdateEventAsync(id, stateId, userId, occurrenceDate, type, quantity);
    }

    public async Task DeleteEventAsync(int id)
    {
        await this._fakeRepository.DeleteEventAsync(id);
    }

    public async Task<Dictionary<int, IEventDTO>> GetAllEventsAsync()
    {
        Dictionary<int, IEventDTO> result = new Dictionary<int, IEventDTO>();

        foreach (IEventDTO even in (await this._fakeRepository.GetAllEventsAsync()).Values)
        {
            result.Add(even.Id, even);
        }

        return result;
    }

    public async Task<int> GetEventsCountAsync()
    {
        return await this._fakeRepository.GetEventsCountAsync();
    }
}
