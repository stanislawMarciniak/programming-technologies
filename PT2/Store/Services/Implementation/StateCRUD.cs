using Data.API;
using Service.API;

namespace Service.Implementation;

internal class StateCRUD : IStateCRUD
{
    private IDataRepository _repository;

    public StateCRUD(IDataRepository repository)
    {
        this._repository = repository;
    }

    private IStateDTO Map(IState state)
    {
        return new StateDTO(state.Id, state.movieId, state.movieQuantity);
    }

    public async Task AddStateAsync(int id, int movieId, int movieQuantity)
    {
        await _repository.AddStateAsync(id, movieId, movieQuantity);
    }

    public async Task<IStateDTO> GetStateAsync(int id)
    {
        return this.Map(await this._repository.GetStateAsync(id));
    }

    public async Task UpdateStateAsync(int id, int movieId, int movieQuantity)
    {
        await this._repository.UpdateStateAsync(id, movieId, movieQuantity);
    }

    public async Task DeleteStateAsync(int id)
    {
        await this._repository.DeleteStateAsync(id);
    }

    public async Task<Dictionary<int, IStateDTO>> GetAllStatesAsync()
    {
        Dictionary<int, IStateDTO> result = new Dictionary<int, IStateDTO>();

        foreach (IState state in (await this._repository.GetAllStatesAsync()).Values)
        {
            result.Add(state.Id, this.Map(state));
        }

        return result;
    }

    public async Task<int> GetStatesCountAsync()
    {
        return await this._repository.GetStatesCountAsync();
    }
}
