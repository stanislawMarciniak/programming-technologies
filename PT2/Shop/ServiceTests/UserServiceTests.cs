using Data.API;
using Data.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.API;

namespace ServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private readonly IDataRepository _repository = new MockDataRepository();

        [TestMethod]
        public async Task AddAndRetrieveUserTest()
        {
            IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(_repository);
            await userCrud.AddUserAsync(1, "John Doe", "john.doe@example.com", 1000, new DateTime(1990, 1, 1));
            
            IUserDTO retrievedUser = await userCrud.GetUserAsync(1);

            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(1, retrievedUser.Id);
            Assert.AreEqual("John Doe", retrievedUser.Nickname);
            Assert.AreEqual("john.doe@example.com", retrievedUser.Email);
            Assert.AreEqual(1000, retrievedUser.Balance);
            Assert.AreEqual(new DateTime(1990, 1, 1), retrievedUser.DateOfBirth);
        }

        [TestMethod]
        public async Task UpdateUserTest()
        {
            IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(_repository);
            await userCrud.AddUserAsync(2, "Jane Doe", "jane.doe@example.com", 1500, new DateTime(1985, 5, 5));
            
            await userCrud.UpdateUserAsync(2, "Jane Smith", "jane.smith@example.com", 2000, new DateTime(1985, 5, 5));
            IUserDTO updatedUser = await userCrud.GetUserAsync(2);

            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(2, updatedUser.Id);
            Assert.AreEqual("Jane Smith", updatedUser.Nickname);
            Assert.AreEqual("jane.smith@example.com", updatedUser.Email);
            Assert.AreEqual(2000, updatedUser.Balance);
            Assert.AreEqual(new DateTime(1985, 5, 5), updatedUser.DateOfBirth);
        }

        [TestMethod]
        public async Task DeleteUserTest()
        {
            IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(_repository);
            await userCrud.AddUserAsync(1, "John Doe", "john.doe@example.com", 1000, new DateTime(1990, 1, 1));

            IUserDTO testUser = await userCrud.GetUserAsync(1);
            Assert.IsNotNull(testUser);

            // Delete the user
            await userCrud.DeleteUserAsync(1);

            // User should not exist - cannot be retrieved
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await userCrud.GetUserAsync(1));
        }
    }
}
