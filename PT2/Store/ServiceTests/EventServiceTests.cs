using Data.API;
using Data.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.API;
using ServiceTests.Mocks;
using System;
using System.Threading.Tasks;

namespace ServiceTests
{
    [TestClass]
    public class EventServiceTests
    {
        private readonly IDataRepository _repository = new MockDataRepository();

        [TestMethod]
        public async Task PurchaseEventTest()
        {
            IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(_repository);
            await userCrud.AddUserAsync(1, "John Doe", "john.doe@example.com", 1000, new DateTime(1990, 1, 1));
            IUserDTO testedUser = await userCrud.GetUserAsync(1);

            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(1, "Movie1", 100, 0);
            IMovieDTO testedMovie = await movieCrud.GetMovieAsync(1);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(1, testedMovie.Id, 10);
            IStateDTO testedState = await stateCrud.GetStateAsync(1);

            Assert.IsNotNull(testedMovie);

            IEventCRUD eventCrud = IEventCRUD.CreateEventCRUD(_repository);
            await eventCrud.AddEventAsync(1, testedState.Id, testedState.Id, "PurchaseEvent");

            testedUser = await userCrud.GetUserAsync(1);
            testedState = await stateCrud.GetStateAsync(1);

            Assert.AreEqual(900, testedUser.Balance);           // purchase reduces user's balance
            Assert.AreEqual(9, testedState.movieQuantity);    // purchase reduces movie's quantity
        }

        [TestMethod]
        public async Task ReturnEventTest()
        {
            IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(_repository);
            await userCrud.AddUserAsync(1, "John Doe", "john.doe@example.com", 1000, new DateTime(1990, 1, 1));
            IUserDTO testedUser = await userCrud.GetUserAsync(1);

            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(1, "Movie1", 100, 0);
            IMovieDTO testedMovie = await movieCrud.GetMovieAsync(1);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(1, testedMovie.Id, 10);
            IStateDTO testedState = await stateCrud.GetStateAsync(1);

            Assert.IsNotNull(testedMovie);

            IEventCRUD eventCrud = IEventCRUD.CreateEventCRUD(_repository);
            await eventCrud.AddEventAsync(1, testedState.Id, testedState.Id, "PurchaseEvent");

            // Return the movie
            await eventCrud.AddEventAsync(2, testedState.Id, testedState.Id, "ReturnEvent");

            testedUser = await userCrud.GetUserAsync(1);
            testedState = await stateCrud.GetStateAsync(1);

            Assert.AreEqual(1000, testedUser.Balance);          // return restores user's balance
            Assert.AreEqual(10, testedState.movieQuantity);    // return restores movie's quantity

            await eventCrud.DeleteEventAsync(1);
            await eventCrud.DeleteEventAsync(2);
            await stateCrud.DeleteStateAsync(2);
            await movieCrud.DeleteMovieAsync(2);
            await userCrud.DeleteUserAsync(2);
        }

        [TestMethod]
        public async Task SupplyEventTest()
        {
            IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(_repository);
            await userCrud.AddUserAsync(1, "John Doe", "john.doe@example.com", 1000, new DateTime(1990, 1, 1));
            IUserDTO testedUser = await userCrud.GetUserAsync(1);

            IMovieCRUD movieCrud = IMovieCRUD.CreateMovieCRUD(_repository);
            await movieCrud.AddMovieAsync(1, "Movie1", 100, 0);
            IMovieDTO testedMovie = await movieCrud.GetMovieAsync(1);

            IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(_repository);
            await stateCrud.AddStateAsync(1, testedMovie.Id, 2);
            IStateDTO testedState = await stateCrud.GetStateAsync(1);

            Assert.IsNotNull(testedMovie);

            IEventCRUD eventCrud = IEventCRUD.CreateEventCRUD(_repository);
            await eventCrud.AddEventAsync(1, testedState.Id, testedState.Id, "SupplyEvent", 10);

            testedState = await stateCrud.GetStateAsync(1);

            Assert.AreEqual(12, testedState.movieQuantity);                // quantity = 2 + 10 (from supply event)

            await eventCrud.DeleteEventAsync(1);
            await stateCrud.DeleteStateAsync(1);
            await movieCrud.DeleteMovieAsync(1);
            await userCrud.DeleteUserAsync(1);
        }
    }
}
