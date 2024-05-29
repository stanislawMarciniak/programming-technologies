using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;
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
            int testUserId = 144;
            int testMovieId = 144;
            int testStateId = 144;
            int testEventId = 144;

            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 1500, new DateTime(1985, 5, 15));
            await _dataRepository.AddMovieAsync(testMovieId, "Movie example", 200, 18);
            await _dataRepository.AddStateAsync(testStateId, testMovieId, 30);
            await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent");

            // Fetch data from the database
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            Assert.AreEqual(1300, testUser.Balance);
            Assert.AreEqual(29, testState.movieQuantity);

            await _dataRepository.DeleteEventAsync(testEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteMovieAsync(testMovieId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task InsufficientBalancePurchaseEventTest()
        {
            int testUserId = 144;
            int testMovieId = 144;
            int testStateId = 144;
            int testEventId = 144;

            // User with insufficient balance
            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 15, new DateTime(1985, 5, 15));
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);

            await _dataRepository.AddMovieAsync(testMovieId, "Movie example", 200, 18);
            IMovie testMovie = await _dataRepository.GetMovieAsync(testMovieId);

            await _dataRepository.AddStateAsync(testStateId, testMovieId, 30);
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            // Purchase event with insufficient balance should throw an exception
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent"));
            
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteMovieAsync(testMovieId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task InsufficientMovieQuantityPurchaseEventTest()
        {
            int testUserId = 144;
            int testMovieId = 144;
            int testStateId = 144;
            int testEventId = 144;

            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 15, new DateTime(1985, 5, 15));
            IUser testUser = await _dataRepository.GetUserAsync(testUserId);
            
            await _dataRepository.AddMovieAsync(testMovieId, "Movie example", 200, 18);
            IMovie testMovie = await _dataRepository.GetMovieAsync(testMovieId);
            
            // Movie with insufficient quantity
            await _dataRepository.AddStateAsync(testStateId, testMovieId, 0);
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(testEventId, testStateId, testUserId, "PurchaseEvent"));
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteMovieAsync(testMovieId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }

        [TestMethod]
        public async Task SupplyEventActionTest()
        {
            int testUserId = 144;
            int testMovieId = 144;
            int testStateId = 144;
            int testSupplyEventId = 321;

            await _dataRepository.AddUserAsync(testUserId, "Bob", "bob@example.com", 1500, new DateTime(1985, 5, 15));
            await _dataRepository.AddMovieAsync(testMovieId, "Movie example", 200, 18);
            await _dataRepository.AddStateAsync(testStateId, testMovieId, 30);
            await _dataRepository.AddEventAsync(testSupplyEventId, testStateId, testUserId, "SupplyEvent", 12);

            // Fetch data from the database
            IState testState = await _dataRepository.GetStateAsync(testStateId);

            Assert.AreEqual(42, testState.movieQuantity);     // Quantity = 30 + 12 (from SupplyEvent)

            await _dataRepository.DeleteEventAsync(testSupplyEventId);
            await _dataRepository.DeleteStateAsync(testStateId);
            await _dataRepository.DeleteMovieAsync(testMovieId);
            await _dataRepository.DeleteUserAsync(testUserId);
        }
    }
}