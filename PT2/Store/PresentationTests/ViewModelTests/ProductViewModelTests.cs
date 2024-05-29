using Microsoft.VisualStudio.TestTools.UnitTesting;
using Presentation;
using Presentation.Model.API;
using Presentation.ViewModel;
using PresentationTests.ErrorInformers;
using PresentationTests.Mocks;
using PresentationTests.Mocks.CRUD;
using Service.API;

namespace PresentationTests.ViewModelTests
{
    [TestClass]
    public class MovieViewModelTests
    {
        private readonly IErrorInformer _informer = new TextErrorInformer();

        [TestMethod]
        public void MovieMasterViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IMovieCRUD mockMovieCrud = new MockMovieCRUD();

            IMovieModelOperation operation = IMovieModelOperation.CreateModelOperation(mockMovieCrud);
            IMovieMasterViewModel master = IMovieMasterViewModel.CreateViewModel(operation, _informer);

            master.Name = "TestMovie";
            master.Price = 150;
            master.AgeRestriction = 16;

            Assert.IsNotNull(master.CreateMovie);
            Assert.IsNotNull(master.RemoveMovie);

            Assert.IsTrue(master.CreateMovie.CanExecute(null));

            master.Price = -1;
            Assert.IsFalse(master.CreateMovie.CanExecute(null));

            master.Price = 150;
            Assert.IsTrue(master.RemoveMovie.CanExecute(null));
        }

        [TestMethod]
        public void MovieDetailViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IMovieCRUD mockMovieCrud = new MockMovieCRUD();

            IMovieModelOperation operation = IMovieModelOperation.CreateModelOperation(mockMovieCrud);
            IMovieDetailViewModel detail = IMovieDetailViewModel.CreateViewModel(1, "TestMovie", 150, 16,
                operation, _informer);

            Assert.AreEqual(1, detail.Id);
            Assert.AreEqual("TestMovie", detail.Name);
            Assert.AreEqual(150, detail.Price);
            Assert.AreEqual(16, detail.AgeRestriction);

            Assert.IsTrue(detail.UpdateMovie.CanExecute(null));
        }
    }
}
