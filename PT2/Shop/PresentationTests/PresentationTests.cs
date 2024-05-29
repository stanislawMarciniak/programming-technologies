using Presentation;
using Presentation.Model.API;
using Presentation.ViewModel;
using PresentationTest;
using Service.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PresentationTests;

[TestClass]
public class PresentationTests
{
    private readonly IErrorInformer _informer = new TextErrorInformer();

    [TestMethod]
    public void UserMasterViewModelTests()
    {
        IUserCRUD fakeUserCrud = new FakeUserCRUD();

        IUserModelOperation operation = IUserModelOperation.CreateModelOperation(fakeUserCrud);

        IUserMasterViewModel viewModel = IUserMasterViewModel.CreateViewModel(operation, _informer);

        viewModel.Nickname = "Test";
        viewModel.Email = "Test.test@gmai.com";
        viewModel.Balance = 124;
        viewModel.DateOfBirth = DateTime.Now;

        Assert.IsNotNull(viewModel.CreateUser);
        Assert.IsNotNull(viewModel.RemoveUser);

        Assert.IsTrue(viewModel.CreateUser.CanExecute(null));

        viewModel.Balance = -1;

        Assert.IsFalse(viewModel.CreateUser.CanExecute(null));

        Assert.IsTrue(viewModel.RemoveUser.CanExecute(null));
    }

    [TestMethod]
    public void UserDetailViewModelTests()
    {
        IUserCRUD fakeUserCrud = new FakeUserCRUD();

        IUserModelOperation operation = IUserModelOperation.CreateModelOperation(fakeUserCrud);

        IUserDetailViewModel detail = IUserDetailViewModel.CreateViewModel(1, "Test", "test.email", 
            123, new DateTime(2001, 1, 1), operation, _informer);

        Assert.AreEqual(1, detail.Id);
        Assert.AreEqual("Test", detail.Nickname);
        Assert.AreEqual("test.email", detail.Email);
        Assert.AreEqual(123, detail.Balance);
        Assert.AreEqual(new DateTime(2001, 1, 1), detail.DateOfBirth);

        Assert.IsTrue(detail.UpdateUser.CanExecute(null));
    }

    [TestMethod]
    public void ProductMasterViewModelTests()
    {
        IProductCRUD fakeProductCrud = new FakeProductCRUD();

        IProductModelOperation operation = IProductModelOperation.CreateModelOperation(fakeProductCrud);

        IProductMasterViewModel master = IProductMasterViewModel.CreateViewModel(operation, _informer);

        master.Name = "Game";
        master.Price = 123;
        master.Pegi = 18;

        Assert.IsNotNull(master.CreateProduct);
        Assert.IsNotNull(master.RemoveProduct);

        Assert.IsTrue(master.CreateProduct.CanExecute(null));

        master.Price = -1;

        Assert.IsFalse(master.CreateProduct.CanExecute(null));

        Assert.IsTrue(master.RemoveProduct.CanExecute(null));
    }

    [TestMethod]
    public void ProductDetailViewModelTests()
    {
        IProductCRUD fakeProductCrud = new FakeProductCRUD();

        IProductModelOperation operation = IProductModelOperation.CreateModelOperation(fakeProductCrud);

        IProductDetailViewModel detail = IProductDetailViewModel.CreateViewModel(1, "kanapa", 200, 12, 
            operation, _informer);

        Assert.AreEqual(1, detail.Id);
        Assert.AreEqual("kanapa", detail.Name);
        Assert.AreEqual(200, detail.Price);
        Assert.AreEqual(12, detail.Pegi);

        Assert.IsTrue(detail.UpdateProduct.CanExecute(null));
    }

    [TestMethod]
    public void StateMasterViewModelTests()
    {
        IStateCRUD fakeStateCrud = new FakeStateCRUD();

        IStateModelOperation operation = IStateModelOperation.CreateModelOperation( fakeStateCrud);

        IStateMasterViewModel master = IStateMasterViewModel.CreateViewModel(operation, _informer);

        master.ProductId = 1;
        master.ProductQuantity = 1;

        Assert.IsNotNull(master.CreateState);
        Assert.IsNotNull(master.RemoveState);

        Assert.IsTrue(master.CreateState.CanExecute(null));

        master.ProductQuantity = -1;

        Assert.IsFalse(master.CreateState.CanExecute(null));

        Assert.IsTrue(master.RemoveState.CanExecute(null));
    }

    [TestMethod]
    public void StateDetailViewModelTests()
    {
        IStateCRUD fakeStateCrud = new FakeStateCRUD();

        IStateModelOperation operation = IStateModelOperation.CreateModelOperation(fakeStateCrud);

        IStateDetailViewModel detail = IStateDetailViewModel.CreateViewModel(1, 1, 1, operation, _informer);

        Assert.AreEqual(1, detail.Id);
        Assert.AreEqual(1, detail.ProductId);
        Assert.AreEqual(1, detail.ProductQuantity);

        Assert.IsTrue(detail.UpdateState.CanExecute(null));
    }

    [TestMethod]
    public void EventMasterViewModelTests()
    {
        IEventCRUD fakeEventCrud = new FakeEventCRUD();

        IEventModelOperation operation = IEventModelOperation.CreateModelOperation(fakeEventCrud);

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

        master.Quantity = 1;

        Assert.IsTrue(master.SupplyEvent.CanExecute(null));

        Assert.IsTrue(master.RemoveEvent.CanExecute(null));
    }

    [TestMethod]
    public void EventDetailViewModelTests()
    {
        IEventCRUD fakeEventCrud = new FakeEventCRUD();

        IEventModelOperation operation = IEventModelOperation.CreateModelOperation(fakeEventCrud);

        IEventDetailViewModel detail = IEventDetailViewModel.CreateViewModel(1, 1, 1, new DateTime(2001, 1, 1),
            "PurchaseEvent", null, operation, _informer);

        Assert.AreEqual(1, detail.Id);
        Assert.AreEqual(1, detail.StateId);
        Assert.AreEqual(1, detail.UserId);
        Assert.AreEqual(new DateTime(2001, 1, 1), detail.OccurrenceDate);
        Assert.AreEqual("PurchaseEvent", detail.Type);

        Assert.IsTrue(detail.UpdateEvent.CanExecute(null));
    }

    [TestMethod]
    public void DataFixedGenerationMethodTests()
    {
        IGenerator fixedGenerator = new FixedGenerator();

        IUserCRUD fakeUserCrud = new FakeUserCRUD();
        IUserModelOperation userOperation = IUserModelOperation.CreateModelOperation(fakeUserCrud);
        IUserMasterViewModel userViewModel = IUserMasterViewModel.CreateViewModel(userOperation, _informer);

        IProductCRUD fakeProductCrud = new FakeProductCRUD();
        IProductModelOperation productOperation = IProductModelOperation.CreateModelOperation(fakeProductCrud);
        IProductMasterViewModel productViewModel = IProductMasterViewModel.CreateViewModel(productOperation, _informer);


        IStateCRUD fakeStateCrud = new FakeStateCRUD();
        IStateModelOperation stateOperation = IStateModelOperation.CreateModelOperation(fakeStateCrud);
        IStateMasterViewModel stateViewModel = IStateMasterViewModel.CreateViewModel(stateOperation, _informer);

        IEventCRUD fakeEventCrud = new FakeEventCRUD();
        IEventModelOperation eventOperation = IEventModelOperation.CreateModelOperation(fakeEventCrud);
        IEventMasterViewModel eventViewModel = IEventMasterViewModel.CreateViewModel(eventOperation, _informer);

        fixedGenerator.GenerateUserModels(userViewModel);
        fixedGenerator.GenerateProductModels(productViewModel);
        fixedGenerator.GenerateStateModels(stateViewModel);
        fixedGenerator.GenerateEventModels(eventViewModel);

        Assert.AreEqual(5, userViewModel.Users.Count);
        Assert.AreEqual(6, productViewModel.Products.Count);
        Assert.AreEqual(6, stateViewModel.States.Count);
        Assert.AreEqual(6, eventViewModel.Events.Count);
    }

    [TestMethod]
    public void DataRandomGenerationMethodTests()
    {
        IGenerator randomGenerator = new RandomGenerator();

        IUserCRUD fakeUserCrud = new FakeUserCRUD();
        IUserModelOperation userOperation = IUserModelOperation.CreateModelOperation(fakeUserCrud);
        IUserMasterViewModel userViewModel = IUserMasterViewModel.CreateViewModel(userOperation, _informer);

        IProductCRUD fakeProductCrud = new FakeProductCRUD();
        IProductModelOperation productOperation = IProductModelOperation.CreateModelOperation(fakeProductCrud);
        IProductMasterViewModel productViewModel = IProductMasterViewModel.CreateViewModel(productOperation, _informer);


        IStateCRUD fakeStateCrud = new FakeStateCRUD();
        IStateModelOperation stateOperation = IStateModelOperation.CreateModelOperation(fakeStateCrud);
        IStateMasterViewModel stateViewModel = IStateMasterViewModel.CreateViewModel(stateOperation, _informer);

        IEventCRUD fakeEventCrud = new FakeEventCRUD();
        IEventModelOperation eventOperation = IEventModelOperation.CreateModelOperation(fakeEventCrud);
        IEventMasterViewModel eventViewModel = IEventMasterViewModel.CreateViewModel(eventOperation, _informer);

        randomGenerator.GenerateUserModels(userViewModel);
        randomGenerator.GenerateProductModels(productViewModel);
        randomGenerator.GenerateStateModels(stateViewModel);
        randomGenerator.GenerateEventModels(eventViewModel);

        Assert.AreEqual(10, userViewModel.Users.Count);
        Assert.AreEqual(10, productViewModel.Products.Count);
        Assert.AreEqual(10, stateViewModel.States.Count);
        Assert.AreEqual(10, eventViewModel.Events.Count);
    }
}
