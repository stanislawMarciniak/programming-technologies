using Data.API;
using Service.Implementation;

namespace Service.API;

public interface IStateCRUD
{
    static IStateCRUD CreateStateCRUD(IDataRepository? dataRepository = null)
    {
        return new StateCRUD(dataRepository ?? IDataRepository.CreateDatabase());
    }

    Task AddStateAsync(int id, int movieId, int movieQuantity);

    Task<IStateDTO> GetStateAsync(int id);

    Task UpdateStateAsync(int id, int movieId, int movieQuantity);

    Task DeleteStateAsync(int id);

    Task<Dictionary<int, IStateDTO>> GetAllStatesAsync();

    Task<int> GetStatesCountAsync();
}
