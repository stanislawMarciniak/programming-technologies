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
    public class StateViewModelTests
    {
        private readonly IErrorInformer _informer = new TextErrorInformer();

        [TestMethod]
        public void StateMasterViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IStateCRUD mockStateCrud = new MockStateCRUD();

            IStateModelOperation operation = IStateModelOperation.CreateModelOperation(mockStateCrud);
            IStateMasterViewModel master = IStateMasterViewModel.CreateViewModel(operation, _informer);

            master.ProductId = 1;
            master.ProductQuantity = 10;

            Assert.IsNotNull(master.CreateState);
            Assert.IsNotNull(master.RemoveState);

            Assert.IsTrue(master.CreateState.CanExecute(null));

            master.ProductQuantity = -1;
            Assert.IsFalse(master.CreateState.CanExecute(null));

            master.ProductQuantity = 10;
            Assert.IsTrue(master.RemoveState.CanExecute(null));
        }

        [TestMethod]
        public void StateDetailViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IStateCRUD mockStateCrud = new MockStateCRUD();

            IStateModelOperation operation = IStateModelOperation.CreateModelOperation(mockStateCrud);
            IStateDetailViewModel detail = IStateDetailViewModel.CreateViewModel(1, 1, 10, operation, _informer);

            Assert.AreEqual(1, detail.Id);
            Assert.AreEqual(1, detail.ProductId);
            Assert.AreEqual(10, detail.ProductQuantity);

            Assert.IsTrue(detail.UpdateState.CanExecute(null));
        }
    }
}
