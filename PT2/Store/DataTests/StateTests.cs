using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;
using Data.Database;

namespace DataTests
{
    [TestClass]
    [DeploymentItem("TestingDatabase.mdf")]
    public class StateTests
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
        public async Task AddAndRetrieveStateTest()
        {
            int testProductId = 11;
            int testStateId = 11;

            await _dataRepository.AddProductAsync(testProductId, "Product example", 250, 18);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 50);
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            Assert.IsNotNull(testState);
            Assert.AreEqual(testStateId, testState.Id);
            Assert.AreEqual(testProductId, testState.productId);
            Assert.AreEqual(50, testState.productQuantity);

            Assert.IsNotNull(await _dataRepository.GetAllStatesAsync());
            Assert.IsTrue(await _dataRepository.GetStatesCountAsync() > 0);

            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
        }

        [TestMethod]
        public async Task UpdateAndDeleteStateTest()
        {
            int testProductId = 180;
            int testStateId = 180;

            await _dataRepository.AddProductAsync(testProductId, "TestProduct", 100, 0);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 20);

            // Update state
            await _dataRepository.UpdateStateAsync(testStateId, testProductId, 30);
            IState updatedState = await _dataRepository.GetStateAsync(testStateId);

            Assert.IsNotNull(updatedState);
            Assert.AreEqual(testStateId, updatedState.Id);
            Assert.AreEqual(testProductId, updatedState.productId);
            Assert.AreEqual(30, updatedState.productQuantity);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateStateAsync(testStateId + 1, testProductId, 30));
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateStateAsync(testStateId, 404, 30));
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateStateAsync(testStateId, testProductId, -10));

            await _dataRepository.DeleteStateAsync(testStateId);
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetStateAsync(testStateId));

            await _dataRepository.DeleteProductAsync(testProductId);
        }
    }
}
