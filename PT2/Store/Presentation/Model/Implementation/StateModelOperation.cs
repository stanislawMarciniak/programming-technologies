using Presentation.Model.API;
using Service.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Model.Implementation;

internal class StateModelOperation : IStateModelOperation
{
    private IStateCRUD _stateCrud;

    public StateModelOperation(IStateCRUD? stateCrud = null)
    {
        this._stateCrud = stateCrud ?? IStateCRUD.CreateStateCRUD();
    }

    private IStateModel Map(IStateDTO state)
    {
        return new StateModel(state.Id, state.productId, state.productQuantity);
    }

    public async Task AddAsync(int id, int productId, int productQuantity)
    {
        await this._stateCrud.AddStateAsync(id, productId, productQuantity);
    }

    public async Task<IStateModel> GetAsync(int id)
    {
        return this.Map(await this._stateCrud.GetStateAsync(id));
    }

    public async Task UpdateAsync(int id, int productId, int productQuantity)
    {
        await this._stateCrud.UpdateStateAsync(id, productId, productQuantity);
    }

    public async Task DeleteAsync(int id)
    {
        await this._stateCrud.DeleteStateAsync(id);
    }

    public async Task<Dictionary<int, IStateModel>> GetAllAsync()
    {
        Dictionary<int, IStateModel> result = new Dictionary<int, IStateModel>();

        foreach (IStateDTO state in (await this._stateCrud.GetAllStatesAsync()).Values)
        {
            result.Add(state.Id, this.Map(state));
        }

        return result;
    }

    public async Task<int> GetCountAsync()
    {
        return await this._stateCrud.GetStatesCountAsync();
    }
}
