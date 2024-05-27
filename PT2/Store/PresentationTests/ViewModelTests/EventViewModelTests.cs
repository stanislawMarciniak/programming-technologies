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
    public class EventViewModelTests
    {
        private readonly IErrorInformer _informer = new TextErrorInformer();

        [TestMethod]
        public void EventMasterViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IEventCRUD mockEventCrud = new MockEventCRUD();

            IEventModelOperation operation = IEventModelOperation.CreateModelOperation(mockEventCrud);
            IEventMasterViewModel master = IEventMasterViewModel.CreateViewModel(operation, _informer);

            master.StateId = 1;
            master.UserId = 1;

            Assert.IsNotNull(master.PurchaseEvent);
            Assert.IsNotNull(master.ReturnEvent);
            Assert.IsNotNull(master.SupplyEvent);
            Assert.IsNotNull(master.RemoveEvent);

            Assert.IsTrue(master.PurchaseEvent.CanExecute(null));
            Assert.IsTrue(master.ReturnEvent.CanExecute(null));
            Assert.IsFalse(master.SupplyEvent.CanExecute(null));

            master.Quantity = 10;
            Assert.IsTrue(master.SupplyEvent.CanExecute(null));

            Assert.IsTrue(master.RemoveEvent.CanExecute(null));
        }

        [TestMethod]
        public void EventDetailViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IEventCRUD mockEventCrud = new MockEventCRUD();

            IEventModelOperation operation = IEventModelOperation.CreateModelOperation(mockEventCrud);
            IEventDetailViewModel detail = IEventDetailViewModel.CreateViewModel(1, 1, 1, new DateTime(2022, 1, 1),
                "PurchaseEvent", 10, operation, _informer);

            Assert.AreEqual(1, detail.Id);
            Assert.AreEqual(1, detail.StateId);
            Assert.AreEqual(1, detail.UserId);
            Assert.AreEqual(new DateTime(2022, 1, 1), detail.OccurrenceDate);
            Assert.AreEqual("PurchaseEvent", detail.Type);

            Assert.IsTrue(detail.UpdateEvent.CanExecute(null));
        }
    }
}
