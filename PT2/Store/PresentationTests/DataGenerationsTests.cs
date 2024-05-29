using Presentation;
using Presentation.Model.API;
using Presentation.ViewModel;
using Service.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PresentationTests.Generators;
using PresentationTests.ErrorInformers;
using PresentationTests.Mocks;
using PresentationTests.Mocks.CRUD;

namespace PresentationTests 
{
    [TestClass]
    public class DataGenerationTests
    {
        private readonly IErrorInformer _informer = new TextErrorInformer();

        [TestMethod]
        public void DataFixedGenerationMethodTests()
        {
            IGenerator fixedGenerator = new FixedGenerator();
            MockDataRepository mockDataRepository = new MockDataRepository();

            IUserCRUD mockUserCrud = new MockUserCRUD();
            IUserModelOperation userOperation = IUserModelOperation.CreateModelOperation(mockUserCrud);
            IUserMasterViewModel userViewModel = IUserMasterViewModel.CreateViewModel(userOperation, _informer);

            IMovieCRUD mockMovieCrud = new MockMovieCRUD();
            IMovieModelOperation movieOperation = IMovieModelOperation.CreateModelOperation(mockMovieCrud);
            IMovieMasterViewModel movieViewModel = IMovieMasterViewModel.CreateViewModel(movieOperation, _informer);

            IStateCRUD mockStateCrud = new MockStateCRUD();
            IStateModelOperation stateOperation = IStateModelOperation.CreateModelOperation(mockStateCrud);
            IStateMasterViewModel stateViewModel = IStateMasterViewModel.CreateViewModel(stateOperation, _informer);

            IEventCRUD mockEventCrud = new MockEventCRUD();
            IEventModelOperation eventOperation = IEventModelOperation.CreateModelOperation(mockEventCrud);
            IEventMasterViewModel eventViewModel = IEventMasterViewModel.CreateViewModel(eventOperation, _informer);

            fixedGenerator.GenerateUserModels(userViewModel);
            fixedGenerator.GenerateMovieModels(movieViewModel);
            fixedGenerator.GenerateStateModels(stateViewModel);
            fixedGenerator.GenerateEventModels(eventViewModel);

            Assert.AreEqual(5, userViewModel.Users.Count);
            Assert.AreEqual(5, movieViewModel.Movies.Count);
            Assert.AreEqual(6, stateViewModel.States.Count);
            Assert.AreEqual(6, eventViewModel.Events.Count);
        }

        [TestMethod]
        public void DataRandomGenerationMethodTests()
        {
            IGenerator randomGenerator = new RandomGenerator();
            MockDataRepository mockDataRepository = new MockDataRepository();

            IUserCRUD mockUserCrud = new MockUserCRUD();
            IUserModelOperation userOperation = IUserModelOperation.CreateModelOperation(mockUserCrud);
            IUserMasterViewModel userViewModel = IUserMasterViewModel.CreateViewModel(userOperation, _informer);

            IMovieCRUD mockMovieCrud = new MockMovieCRUD();
            IMovieModelOperation movieOperation = IMovieModelOperation.CreateModelOperation(mockMovieCrud);
            IMovieMasterViewModel movieViewModel = IMovieMasterViewModel.CreateViewModel(movieOperation, _informer);

            IStateCRUD mockStateCrud = new MockStateCRUD();
            IStateModelOperation stateOperation = IStateModelOperation.CreateModelOperation(mockStateCrud);
            IStateMasterViewModel stateViewModel = IStateMasterViewModel.CreateViewModel(stateOperation, _informer);

            IEventCRUD mockEventCrud = new MockEventCRUD();
            IEventModelOperation eventOperation = IEventModelOperation.CreateModelOperation(mockEventCrud);
            IEventMasterViewModel eventViewModel = IEventMasterViewModel.CreateViewModel(eventOperation, _informer);

            randomGenerator.GenerateUserModels(userViewModel);
            randomGenerator.GenerateMovieModels(movieViewModel);
            randomGenerator.GenerateStateModels(stateViewModel);
            randomGenerator.GenerateEventModels(eventViewModel);

            Assert.AreEqual(10, userViewModel.Users.Count);
            Assert.AreEqual(10, movieViewModel.Movies.Count);
            Assert.AreEqual(10, stateViewModel.States.Count);
            Assert.AreEqual(10, eventViewModel.Events.Count);
        }
    }
}