using Data.API;
using Service.API;

namespace Service.Implementation;

internal class EventCRUD : IEventCRUD
{
    private IDataRepository _repository;

    public EventCRUD(IDataRepository repository)
    {
        this._repository = repository;
    }

    public IEventDTO Map(IEvent even)
    {
        return new EventDTO(even.Id, even.stateId, even.userId, even.occurrenceDate, even.Type, even.Quantity);
    }

    public async Task AddEventAsync(int id, int stateId, int userId, string type, int quantity = 0)
    {
        await this._repository.AddEventAsync(id, stateId, userId, type, quantity);
    }

    public async Task<IEventDTO> GetEventAsync(int id)
    {
        return this.Map(await this._repository.GetEventAsync(id));
    }

    public async Task UpdateEventAsync(int id, int stateId, int userId, DateTime occurrenceDate, string type, int? quantity)
    {
        await this._repository.UpdateEventAsync(id, stateId, userId, occurrenceDate, type, quantity);
    }

    public async Task DeleteEventAsync(int id)
    {
        await this._repository.DeleteEventAsync(id);
    }

    public async Task<Dictionary<int, IEventDTO>> GetAllEventsAsync()
    {
        Dictionary<int, IEventDTO> result = new Dictionary<int, IEventDTO>();

        foreach (IEvent even in (await this._repository.GetAllEventsAsync()).Values)
        {
            result.Add(even.Id, this.Map(even));
        }

        return result;
    }

    public async Task<int> GetEventsCountAsync()
    {
        return await this._repository.GetEventsCountAsync();
    }
}
