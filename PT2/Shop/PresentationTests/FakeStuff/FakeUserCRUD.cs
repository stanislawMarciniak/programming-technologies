using PresentationTests;
using Service.API;

namespace PresentationTest;

internal class FakeUserCRUD : IUserCRUD
{
    private readonly FakeDataRepository _fakeRepository = new FakeDataRepository();

    public FakeUserCRUD()
    {

    }

    public async Task AddUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        await this._fakeRepository.AddUserAsync(id, nickname, email, balance, dateOfBirth);
    }

    public async Task<IUserDTO> GetUserAsync(int id)
    {
        return await this._fakeRepository.GetUserAsync(id);
    }

    public async Task UpdateUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        await this._fakeRepository.UpdateUserAsync(id, nickname, email, balance, dateOfBirth);
    }

    public async Task DeleteUserAsync(int id)
    {
        await this._fakeRepository.DeleteUserAsync(id);
    }

    public async Task<Dictionary<int, IUserDTO>> GetAllUsersAsync()
    {
        Dictionary<int, IUserDTO> result = new Dictionary<int, IUserDTO>();

        foreach (IUserDTO user in (await this._fakeRepository.GetAllUsersAsync()).Values)
        {
            result.Add(user.Id, user);
        }

        return result;
    }

    public async Task<int> GetUsersCountAsync()
    {
        return await this._fakeRepository.GetUsersCountAsync();
    }
}
