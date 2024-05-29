using Data.API;
using Data.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.API;
using ServiceTests.Mocks;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace ServiceTests
{
    [TestClass]
    public class StateServiceTests
    {
        private readonly IDataRepository _repository = new MockDataRepository();

        [TestMethod]
        public async Task AddAndRetrieveStateTest()
        {
            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(1, "Movie1", 50, 6);
            IMovieDTO movie = await movieCrud.GetMovieAsync(1);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(1, movie.Id, 10);
            IStateDTO testedState = await stateCrud.GetStateAsync(1);

            Assert.IsNotNull(testedState);
            Assert.AreEqual(1, testedState.Id);
            Assert.AreEqual(1, testedState.movieId);
            Assert.AreEqual(10, testedState.movieQuantity);
        }

        [TestMethod]
        public async Task UpdateStateTest()
        {
            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(2, "Movie2", 50, 0);
            IMovieDTO movie = await movieCrud.GetMovieAsync(2);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(2, movie.Id, 10);

            await stateCrud.UpdateStateAsync(2, movie.Id, 100);
            IStateDTO updatedState = await stateCrud.GetStateAsync(2);

            Assert.IsNotNull(updatedState);
            Assert.AreEqual(2, updatedState.Id);
            Assert.AreEqual(2, updatedState.movieId);
            Assert.AreEqual(100, updatedState.movieQuantity);
        }

        [TestMethod]
        public async Task DeleteStateTest()
        {
            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(1, "Movie1", 50, 0);
            IMovieDTO movie = await movieCrud.GetMovieAsync(1);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(1, movie.Id, 10);

            // Delete the state
            await stateCrud.DeleteStateAsync(1);

            // State should not exist - cannot be retrieved
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () => await stateCrud.GetStateAsync(1));
        }
    }
}
