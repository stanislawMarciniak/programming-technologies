using Presentation.Model.API;
using Service.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Presentation.Model.Implementation;

internal class UserModelOperation : IUserModelOperation
{
    private IUserCRUD _userCRUD;

    public UserModelOperation(IUserCRUD? userCrud)
    {
        this._userCRUD = userCrud ?? IUserCRUD.CreateUserCRUD();
    }

    private IUserModel Map(IUserDTO user)
    {
        return new UserModel(user.Id, user.Nickname, user.Email, user.Balance, user.DateOfBirth);
    }

    public async Task AddAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        await this._userCRUD.AddUserAsync(id, nickname, email, balance, dateOfBirth);
    }

    public async Task<IUserModel> GetAsync(int id)
    {
        return this.Map(await this._userCRUD.GetUserAsync(id));
    }

    public async Task UpdateAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        await this._userCRUD.UpdateUserAsync(id, nickname, email, balance, dateOfBirth);
    }

    public async Task DeleteAsync(int id)
    {
        await this._userCRUD.DeleteUserAsync(id);
    }

    public async Task<Dictionary<int, IUserModel>> GetAllAsync()
    {
        Dictionary<int, IUserModel> result = new Dictionary<int, IUserModel>();

        foreach (IUserDTO user in (await this._userCRUD.GetAllUsersAsync()).Values)
        {
            result.Add(user.Id, this.Map(user));
        }

        return result;
    }

    public async Task<int> GetCountAsync()
    {
        return await this._userCRUD.GetUsersCountAsync();
    }
}
