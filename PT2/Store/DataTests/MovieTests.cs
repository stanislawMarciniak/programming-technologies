using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;
using Data.Database;

namespace DataTests
{
    [TestClass]
    [DeploymentItem("TestingDatabase.mdf")]
    public class MovieTests
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
        public async Task AddAndRetrieveMovieTest()
        {
            int testMovieId = 202;
            
            await _dataRepository.AddMovieAsync(testMovieId, "Movie example", 300, 18);
            IMovie testMovie = await _dataRepository.GetMovieAsync(testMovieId);

            Assert.IsNotNull(testMovie);
            Assert.AreEqual(testMovieId, testMovie.Id);
            Assert.AreEqual("Movie example", testMovie.MovieName);
            Assert.AreEqual(300, testMovie.Price);
            Assert.AreEqual(18, testMovie.AgeRestriction);

            Assert.IsNotNull(await _dataRepository.GetAllMoviesAsync());
            Assert.IsTrue(await _dataRepository.GetMoviesCountAsync() > 0);

            await _dataRepository.DeleteMovieAsync(testMovieId);
        }

        [TestMethod]
        public async Task UpdateAndDeleteMovieTest()
        {
            int testMovieId = 1;

            await _dataRepository.AddMovieAsync(testMovieId, "Movie example", 300, 18);
            await _dataRepository.UpdateMovieAsync(testMovieId, "Movie example - updated", 350, 16);
            
            IMovie updatedMovie = await _dataRepository.GetMovieAsync(testMovieId);

            Assert.IsNotNull(updatedMovie);
            Assert.AreEqual(testMovieId, updatedMovie.Id);
            Assert.AreEqual("Movie example - updated", updatedMovie.MovieName);
            Assert.AreEqual(350, updatedMovie.Price);
            Assert.AreEqual(16, updatedMovie.AgeRestriction);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateMovieAsync(999, "Movie example - updated", 350, 16));
            await _dataRepository.DeleteMovieAsync(testMovieId);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetMovieAsync(testMovieId));
            await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.DeleteMovieAsync(testMovieId));
        }
    }
}
