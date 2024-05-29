using Service.API;

namespace PresentationTests;

internal class FakeStateCRUD : IStateCRUD
{
    private readonly FakeDataRepository _fakeRepository = new FakeDataRepository();

    public FakeStateCRUD()
    {

    }

    public async Task AddStateAsync(int id, int productId, int productQuantity)
    {
        await _fakeRepository.AddStateAsync(id, productId, productQuantity);
    }

    public async Task<IStateDTO> GetStateAsync(int id)
    {
        return await this._fakeRepository.GetStateAsync(id);
    }

    public async Task UpdateStateAsync(int id, int productId, int productQuantity)
    {
        await this._fakeRepository.UpdateStateAsync(id, productId, productQuantity);
    }

    public async Task DeleteStateAsync(int id)
    {
        await this._fakeRepository.DeleteStateAsync(id);
    }

    public async Task<Dictionary<int, IStateDTO>> GetAllStatesAsync()
    {
        Dictionary<int, IStateDTO> result = new Dictionary<int, IStateDTO>();

        foreach (IStateDTO state in (await this._fakeRepository.GetAllStatesAsync()).Values)
        {
            result.Add(state.Id, state);
        }

        return result;
    }

    public async Task<int> GetStatesCountAsync()
    {
        return await this._fakeRepository.GetStatesCountAsync();
    }
}
