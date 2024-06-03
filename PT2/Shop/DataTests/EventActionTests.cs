using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;

namespace DataTests
{
    [TestClass]
    [DeploymentItem("DatabaseForTests.mdf")]
    public class EventActionTests
    {
        private static string connectionString;
        private readonly IDataRepository _dataRepository = IDataRepository.CreateDatabase(IDataContext.CreateContext(connectionString));

        [ClassInitialize]
        public static void ClassInitializeMethod(TestContext context)
        {
            string dbRelativePath = @"DatabaseForTests.mdf";
            string projectRootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dbPath = Path.Combine(projectRootDir, dbRelativePath);
            FileInfo databaseFile = new FileInfo(dbPath);
            Assert.IsTrue(databaseFile.Exists, $"{Environment.CurrentDirectory}");
            connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;Connect Timeout=30;";
        }

        [TestMethod]
        public async Task PurchaseEventActionTest()
        {
            int testUserId = 122;
            int testProductId = 122;
            int testStateId = 122;
            int testEventId = 122;

            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 1500, new DateTime(1985, 5, 15));
            await _dataRepository.AddProductAsync(testProductId, "Product example", 200, 18);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 30);
            await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent");

            // Fetch data from the database
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            Assert.AreEqual(1300, testUser.Balance);
            Assert.AreEqual(29, testState.productQuantity);

            await _dataRepository.DeleteEventAsync(testEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }


        [TestMethod]
        public async Task InsufficientBalancePurchaseEventTest()
        {
            int testUserId = 12;
            int testProductId = 12;
            int testStateId = 12;
            int testEventId = 12;

            // User with insufficient balance
            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 15, new DateTime(1985, 5, 15));
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);

            await _dataRepository.AddProductAsync(testProductId, "Product example", 200, 18);
            IProduct testProduct = await _dataRepository.GetProductAsync(testProductId);

            await _dataRepository.AddStateAsync(testStateId, testProductId, 30);
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            // Purchase event with insufficient balance should throw an exception
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent"));
            
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task InsufficientProductQuantityPurchaseEventTest()
        {
            int testUserId = 12;
            int testProductId = 12;
            int testStateId = 12;
            int testEventId = 12;

            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 15, new DateTime(1985, 5, 15));
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);
            
            await _dataRepository.AddProductAsync(testProductId, "Product example", 200, 18);
            IProduct testProduct = await _dataRepository.GetProductAsync(testProductId);
            
            // Product with insufficient quantity
            await _dataRepository.AddStateAsync(testStateId, testProductId, 0);
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent"));
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task ReturnEventActionTest()
        {
            int testUserId = 300;
            int testProductId = 300;
            int testStateId = 300;
            int testPurchaseEventId = 300;
            int testReturnEventId = 301;

            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 1500, new DateTime(1985, 5, 15));
            await _dataRepository.AddProductAsync(testProductId, "Product example", 200, 18);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 30);
            await _dataRepository.AddEventAsync(testPurchaseEventId, testStateId, testUserId, "PurchaseEvent");

            // Return product
            await _dataRepository.AddEventAsync(testReturnEventId, testStateId, testUserId, "ReturnEvent");

            // Fetch data from the database
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);
            IState testState = await _dataRepository.GetStateAsync(testStateId);
            
            Assert.AreEqual(1500, testUser.Balance);                // Restored balance
            Assert.AreEqual(30, testState.productQuantity);         // Restored product quantity

            await _dataRepository.DeleteEventAsync(testReturnEventId);
            await _dataRepository.DeleteEventAsync(testPurchaseEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task SupplyEventActionTest()
        {
            int testUserId = 321;
            int testProductId = 321;
            int testStateId = 321;
            int testSupplyEventId = 321;

            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 1500, new DateTime(1985, 5, 15));
            await _dataRepository.AddProductAsync(testProductId, "Product example", 200, 18);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 30);
            await _dataRepository.AddEventAsync(testSupplyEventId, testStateId, testUserId, "SupplyEvent", 12);

            // Fetch data from the database
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            Assert.AreEqual(42, testState.productQuantity);     // Quantity = 30 + 12 (from SupplyEvent)

            await _dataRepository.DeleteEventAsync(testSupplyEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }
    }
}