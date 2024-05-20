using Data.API;
using Service.Implementation;

namespace Service.API;

public interface IUserCRUD
{
    static IUserCRUD CreateUserCRUD(IDataRepository? dataRepository = null)
    {
        return new UserCRUD(dataRepository ?? IDataRepository.CreateDatabase());
    }

    Task AddUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth);

    Task<IUserDTO> GetUserAsync(int id);

    Task UpdateUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth);

    Task DeleteUserAsync(int id);

    Task<Dictionary<int, IUserDTO>> GetAllUsersAsync();

    Task<int> GetUsersCountAsync();
}
