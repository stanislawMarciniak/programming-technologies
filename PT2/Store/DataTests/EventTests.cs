using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;
namespace DataTests
{
    [TestClass]
    [DeploymentItem("TestingDatabase.mdf")]
    public class EventTests
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
        public async Task AddAndRetrieveEventTest()
        {
            int testEventId = 100;
            int testUserId = 100;
            int testProductId = 100;
            int testStateId = 100;

            await _dataRepository.AddProductAsync(testProductId, "Product example", 200, 18);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 30);
            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 1500, new DateTime(1985, 5, 15));

            IProduct testProduct = await _dataRepository.GetProductAsync(testProductId);
            IState testState = await _dataRepository.GetStateAsync(testStateId);
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);

            await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent");
            IEvent testEvent = await _dataRepository.GetEventAsync(testEventId);

            Assert.IsNotNull(testEvent);
            Assert.AreEqual(testEventId, testEvent.Id);
            Assert.AreEqual(testStateId, testEvent.stateId);
            Assert.AreEqual(testUserId, testEvent.userId);

            Assert.IsNotNull(await _dataRepository.GetAllEventsAsync());
            Assert.IsTrue(await _dataRepository.GetEventsCountAsync() > 0);
        }

        [TestMethod]
        public async Task UpdateAndDeleteEventTest()
        {
            int testEventId = 100;
            int testUserId = 100;
            int testStateId = 100;
            int testProductId = 100;

            await _dataRepository.UpdateEventAsync(testEventId, testStateId, testUserId, DateTime.Now, "PurchaseEvent", null);
            IEvent updatedEvent = await _dataRepository.GetEventAsync(testEventId);

            Assert.IsNotNull(updatedEvent);
            Assert.AreEqual(testEventId, updatedEvent.Id);
            Assert.AreEqual(testStateId, updatedEvent.stateId);
            Assert.AreEqual(testUserId, updatedEvent.userId);

            await _dataRepository.DeleteEventAsync(testEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }
    }
}
