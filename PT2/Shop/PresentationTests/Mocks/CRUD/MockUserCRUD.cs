using Service.API;

namespace PresentationTests
{
    internal class MockUserCRUD : IUserCRUD
    {
        private readonly MockDataRepository _repository = new MockDataRepository();

        public MockUserCRUD()
        {

        }

        public async Task AddUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
        {
            await _repository.AddUserAsync(id, nickname, email, balance, dateOfBirth);
        }
        public async Task UpdateUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
        {
            await _repository.UpdateUserAsync(id, nickname, email, balance, dateOfBirth);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _repository.DeleteUserAsync(id);
        }

        public async Task<IUserDTO> GetUserAsync(int id)
        {
            return await _repository.GetUserAsync(id);
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await _repository.GetUsersCountAsync();
        }

        public async Task<Dictionary<int, IUserDTO>> GetAllUsersAsync()
        {
            Dictionary<int, IUserDTO> result = new Dictionary<int, IUserDTO>();

            foreach (IUserDTO user in (await _repository.GetAllUsersAsync()).Values)
            {
                result.Add(user.Id, user);
            }

            return result;
        }

    }
}