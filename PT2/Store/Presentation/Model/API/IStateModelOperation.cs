using System.Collections.Generic;
using System.Threading.Tasks;
using Presentation.Model.Implementation;
using Service.API;

namespace Presentation.Model.API;

public interface IStateModelOperation
{
    static IStateModelOperation CreateModelOperation(IStateCRUD? stateCrud = null)
    {
        return new StateModelOperation(stateCrud ?? IStateCRUD.CreateStateCRUD());
    }

    Task AddAsync(int id, int productId, int productQuantity);

    Task<IStateModel> GetAsync(int id);

    Task UpdateAsync(int id, int productId, int productQuantity);

    Task DeleteAsync(int id);

    Task<Dictionary<int, IStateModel>> GetAllAsync();

    Task<int> GetCountAsync();
}
