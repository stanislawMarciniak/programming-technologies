using Azure;
using Presentation.Model.API;
using Presentation.ViewModel;

namespace PresentationTests
{
    internal class FixedGenerator : IGenerator
    {
        private readonly IErrorInformer _informer = new TextErrorInformer();

        public void GenerateUserModels(IUserMasterViewModel viewModel)
        {
            IUserModelOperation operation = IUserModelOperation.CreateModelOperation(new MockUserCRUD());

            viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(1, "Alice", "alice@example.com", 500, new DateTime(1990, 1, 1), operation, _informer));
            viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(2, "Bob", "bob@example.com", 300, new DateTime(1985, 5, 15), operation, _informer));
            viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(3, "Charlie", "charlie@example.com", 700, new DateTime(2000, 10, 20), operation, _informer));
            viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(4, "Diana", "diana@example.com", 450, new DateTime(1995, 3, 30), operation, _informer));
            viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(5, "Eve", "eve@example.com", 600, new DateTime(1988, 7, 25), operation, _informer));
        }

        public void GenerateProductModels(IProductMasterViewModel viewModel)
        {
            IProductModelOperation operation = IProductModelOperation.CreateModelOperation(new MockProductCRUD());

            viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(1, "Product1", 999.99, 16, operation, _informer));
            viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(2, "Product2", 799.99, 12, operation, _informer));
            viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(3, "Product3", 499.99, 8, operation, _informer));
            viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(4, "Product4", 199.99, 3, operation, _informer));
            viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(5, "Product5", 299.99, 5, operation, _informer));
        }

        public void GenerateStateModels(IStateMasterViewModel viewModel)
        {
            IStateModelOperation operation = IStateModelOperation.CreateModelOperation(new MockStateCRUD());

            viewModel.States.Add(IStateDetailViewModel.CreateViewModel(1, 1, 100, operation, _informer));
            viewModel.States.Add(IStateDetailViewModel.CreateViewModel(2, 2, 200, operation, _informer));
            viewModel.States.Add(IStateDetailViewModel.CreateViewModel(3, 3, 150, operation, _informer));
            viewModel.States.Add(IStateDetailViewModel.CreateViewModel(4, 4, 300, operation, _informer));
            viewModel.States.Add(IStateDetailViewModel.CreateViewModel(5, 5, 250, operation, _informer));
            viewModel.States.Add(IStateDetailViewModel.CreateViewModel(6, 6, 50, operation, _informer));
        }

        public void GenerateEventModels(IEventMasterViewModel viewModel)
        {
            IEventModelOperation operation = IEventModelOperation.CreateModelOperation(new MockEventCRUD());

            viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(1, 1, 1, DateTime.Now, "SupplyEvent", 10, operation, _informer));
            viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(2, 2, 2, DateTime.Now, "SupplyEvent", 123, operation, _informer));
            viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(3, 3, 3, DateTime.Now, "SupplyEvent", 3, operation, _informer));
            viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(4, 4, 4, DateTime.Now, "SupplyEvent", 5, operation, _informer));
            viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(5, 5, 5, DateTime.Now, "SupplyEvent", 15, operation, _informer));
            viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(6, 6, 6, DateTime.Now, "SupplyEvent", 23, operation, _informer));
        }
    }
}