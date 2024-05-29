using Data.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.API;
using ServiceTests.Mocks;

namespace ServiceTests
{
    [TestClass]
    public class MovieServiceTests
    {
        private readonly IDataRepository _repository = new MockDataRepository();

        [TestMethod]
        public async Task AddAndRetrieveMovieTest()
        {
            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(1, "Movie1", 100, 0);

            IMovieDTO retrievedMovie = await movieCrud.GetMovieAsync(1);

            Assert.IsNotNull(retrievedMovie);
            Assert.AreEqual(1, retrievedMovie.Id);
            Assert.AreEqual("Movie1", retrievedMovie.Name);
            Assert.AreEqual(100, retrievedMovie.Price);
            Assert.AreEqual(0, retrievedMovie.AgeRestriction);
        }

        [TestMethod]
        public async Task UpdateMovieTest()
        {
            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(2, "Movie2", 50, 0);
            
            await movieCrud.UpdateMovieAsync(2, "Movie2Updated", 70, 0);
            IMovieDTO updatedMovie = await movieCrud.GetMovieAsync(2);

            Assert.IsNotNull(updatedMovie);
            Assert.AreEqual(2, updatedMovie.Id);
            Assert.AreEqual("Movie2Updated", updatedMovie.Name);
            Assert.AreEqual(70, updatedMovie.Price);
            Assert.AreEqual(0, updatedMovie.AgeRestriction);
        }

        [TestMethod]
        public async Task DeleteMovieTest()
        {
            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(3, "Movie3", 200, 0);

            IMovieDTO testMovie = await movieCrud.GetMovieAsync(3);
            Assert.IsNotNull(testMovie);

            // Delete the movie
            await movieCrud.DeleteMovieAsync(3);

            // Movie should not exist - cannot be retrieved
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await movieCrud.GetMovieAsync(3));
        }
    }
}
