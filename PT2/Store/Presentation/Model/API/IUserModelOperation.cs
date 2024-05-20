using Service.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Presentation.Model.Implementation;

namespace Presentation.Model.API;

public interface IUserModelOperation
{
    static IUserModelOperation CreateModelOperation(IUserCRUD? userCrud = null)
    {
        return new UserModelOperation(userCrud);
    }

    Task AddAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth);

    Task<IUserModel> GetAsync(int id);

    Task UpdateAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth);

    Task DeleteAsync(int id);

    Task<Dictionary<int, IUserModel>> GetAllAsync();

    Task<int> GetCountAsync();
}
