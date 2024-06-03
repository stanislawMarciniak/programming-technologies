using Microsoft.VisualStudio.TestTools.UnitTesting;
using Presentation;
using Presentation.Model.API;
using Presentation.ViewModel;
using Service.API;

namespace PresentationTests
{
    [TestClass]
    public class UserViewModelTests
    {
        private readonly IErrorInformer _informer = new TextErrorInformer();

        [TestMethod]
        public void UserMasterViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IUserCRUD mockUserCrud = new MockUserCRUD();

            IUserModelOperation operation = IUserModelOperation.CreateModelOperation(mockUserCrud);
            IUserMasterViewModel viewModel = IUserMasterViewModel.CreateViewModel(operation, _informer);

            viewModel.Nickname = "TestUser";
            viewModel.Email = "test.user@example.com";
            viewModel.Balance = 100;
            viewModel.DateOfBirth = new DateTime(1995, 5, 15);

            Assert.IsNotNull(viewModel.CreateUser);
            Assert.IsNotNull(viewModel.RemoveUser);

            Assert.IsTrue(viewModel.CreateUser.CanExecute(null));

            viewModel.Balance = -1;
            Assert.IsFalse(viewModel.CreateUser.CanExecute(null));

            viewModel.Balance = 100;
            Assert.IsTrue(viewModel.RemoveUser.CanExecute(null));
        }

        [TestMethod]
        public void UserDetailViewModelTests()
        {
            MockDataRepository mockDataRepository = new MockDataRepository();
            IUserCRUD mockUserCrud = new MockUserCRUD();

            IUserModelOperation operation = IUserModelOperation.CreateModelOperation(mockUserCrud);
            IUserDetailViewModel detail = IUserDetailViewModel.CreateViewModel(1, "TestUser", "test.user@example.com",
                100, new DateTime(1995, 5, 15), operation, _informer);

            Assert.AreEqual(1, detail.Id);
            Assert.AreEqual("TestUser", detail.Nickname);
            Assert.AreEqual("test.user@example.com", detail.Email);
            Assert.AreEqual(100, detail.Balance);
            Assert.AreEqual(new DateTime(1995, 5, 15), detail.DateOfBirth);

            Assert.IsTrue(detail.UpdateUser.CanExecute(null));
        }
    }
}
