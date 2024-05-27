using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;
using Data.Database;

namespace DataTests
{
    [TestClass]
    [DeploymentItem("TestingDatabase.mdf")]
    public class UserTests
    {
        private static string connectionString;
        private readonly IDataRepository _dataRepository = IDataRepository.CreateDatabase(IDataContext.CreateContext(connectionString));

        [ClassInitialize]
        public static void ClassInitializeMethod(TestContext context)
        {
            string dbRelativePath = @"TestingDatabase.mdf";
            string projectRootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dbPath = Path.Combine(projectRootDir, dbRelativePath);
            FileInfo databaseFile = new FileInfo(dbPath);
            Assert.IsTrue(databaseFile.Exists, $"{Environment.CurrentDirectory}");
            connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;";
        }

        [TestMethod]
        public async Task AddAndRetrieveUserTest()
        {
            int testUserId = 200;
            
            await _dataRepository.AddUserAsync(testUserId, "Alice", "alice@example.com", 500, new DateTime(2003, 1, 1));
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);

            Assert.IsNotNull(testUser);
            Assert.AreEqual(testUserId, testUser.Id);
            Assert.AreEqual("Alice", testUser.Nickname);
            Assert.AreEqual("alice@example.com", testUser.Email);
            Assert.AreEqual(500, testUser.Balance);
            Assert.AreEqual(new DateTime(2003, 1, 1), testUser.DateOfBirth);

            Assert.IsNotNull(await _dataRepository.GetAllUsersAsync());
            Assert.IsTrue(await _dataRepository.GetUsersCountAsync() > 0);
            
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task UpdateAndDeleteUserTest()
        {
            int testUserId = 120;
            
            await _dataRepository.AddUserAsync(testUserId, "Alice", "alice@example.com", 500, new DateTime(2003, 1, 1));
            await _dataRepository.UpdateUserAsync(testUserId, "AliceUpdated", "alice_updated@example.com", 750, new DateTime(2003, 1, 2));
            
            IUser updatedUser = await _dataRepository.GetUserAsync(testUserId);

            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(testUserId, updatedUser.Id);
            Assert.AreEqual("AliceUpdated", updatedUser.Nickname);
            Assert.AreEqual("alice_updated@example.com", updatedUser.Email);
            Assert.AreEqual(750, updatedUser.Balance);
            Assert.AreEqual(new DateTime(2003, 1, 2), updatedUser.DateOfBirth);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateUserAsync(testUserId + 1, "AliceUpdated", "alice_updated@example.com", 750, new DateTime(2003, 1, 2)));
            await _dataRepository.DeleteUserAsync(testUserId);
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetUserAsync(testUserId));
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.DeleteUserAsync(testUserId));
        }
    }
}