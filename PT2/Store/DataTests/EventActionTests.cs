using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataTests
{
    [TestClass]
    [DeploymentItem("TestingDatabase.mdf")]
    public class EventActionTests
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
        public async Task PurchaseEventActionTest()
        {
            int testUserId = 100;
            int testStateId = 100;
            int testEventId = 100;
            int testProductId = 100;

            await _dataRepository.AddProductAsync(testProductId, "Product example", 60, 16);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 100);
            await _dataRepository.AddUserAsync(testUserId, "Eve", "eve@example.com", 600, new DateTime(1995, 10, 10));
            await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent");

            Assert.AreEqual(540, (await _dataRepository.GetUserAsync(testUserId)).Balance);
            Assert.AreEqual(99, (await _dataRepository.GetStateAsync(testStateId)).productQuantity);

            await _dataRepository.DeleteEventAsync(testEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task InsufficientBalancePurchaseEventTest()
        {
            int testUserId = 100;
            int testStateId = 100;
            int testEventId = 100;
            int testProductId = 100;

            await _dataRepository.AddProductAsync(testProductId, "Product example", 20, 7);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 50);
            await _dataRepository.AddUserAsync(testUserId, "Charlie", "charlie@example.com", 10, new DateTime(2000, 2, 20));

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent"));
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task InsufficientProductQuantityPurchaseEventTest()
        {
            int testUserId = 100;
            int testStateId = 100;
            int testEventId = 100;
            int testProductId = 100;

            await _dataRepository.AddProductAsync(testProductId, "Product example", 0, 12);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 0);
            await _dataRepository.AddUserAsync(testUserId, "Dave", "dave@example.com", 50, new DateTime(1998, 7, 7));

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent"));
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task ReturnEventActionTest()
        {
            int testUserId = 100;
            int testStateId = 100;
            int testPurchaseEventId = 100;
            int testReturnEventId = 506;
            int testProductId = 100;

            await _dataRepository.AddProductAsync(testProductId, "Product example", 15, 13);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 10);
            await _dataRepository.AddUserAsync(testUserId, "Grace", "grace@example.com", 100, new DateTime(1997, 11, 30));

            await _dataRepository.AddEventAsync(testPurchaseEventId, testStateId, testUserId, "PurchaseEvent");
            await _dataRepository.AddEventAsync(testReturnEventId, testStateId, testUserId, "ReturnEvent");

            Assert.AreEqual(100, (await _dataRepository.GetUserAsync(testUserId)).Balance);
            Assert.AreEqual(10, (await _dataRepository.GetStateAsync(testStateId)).productQuantity);

            await _dataRepository.DeleteEventAsync(testReturnEventId);
            await _dataRepository.DeleteEventAsync(testPurchaseEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task SupplyEventActionTest()
        {
            int testUserId = 100;
            int testStateId = 100;
            int testSupplyEventId = 100;
            int testProductId = 100;

            await _dataRepository.AddProductAsync(testProductId, "Product example", 30, 10);
            await _dataRepository.AddStateAsync(testStateId, testProductId, 2);
            await _dataRepository.AddUserAsync(testUserId, "Ian", "ian@example.com", 150, new DateTime(1992, 12, 20));

            await _dataRepository.AddEventAsync(testSupplyEventId, testStateId, testUserId, "SupplyEvent", 12);
            Assert.AreEqual(14, (await _dataRepository.GetStateAsync(testStateId)).productQuantity);

            await _dataRepository.DeleteEventAsync(testSupplyEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteProductAsync(testProductId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }
    }
}