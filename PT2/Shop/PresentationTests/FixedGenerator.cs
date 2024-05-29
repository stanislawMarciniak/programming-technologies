using Presentation;
using Presentation.Model.API;
using Presentation.ViewModel;
using PresentationTest;

namespace PresentationTests;

internal class FixedGenerator : IGenerator
{
    private readonly IErrorInformer _informer = new TextErrorInformer();

    public void GenerateUserModels(IUserMasterViewModel viewModel)
    {
        IUserModelOperation operation = IUserModelOperation.CreateModelOperation(new FakeUserCRUD());

        viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(1, "Mateusz", "m_kowalski@gmail.com", 353, new DateTime(2013, 5, 21), operation, _informer));
        viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(2, "Kamil", "k_miglanc@gmail.com", 222, new DateTime(2011, 11, 22), operation, _informer));
        viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(3, "Wladyslaw", "w_tomislawowski@gmail.com", 1205, new DateTime(2006, 4, 12), operation, _informer));
        viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(4, "Joanna", "j_dzoanna@gmail.com", 332, new DateTime(2001, 10, 1), operation, _informer));
        viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(5, "Michal", "m_Zahenkos@gmail.com", 1245, new DateTime(1999, 12, 2), operation, _informer));
    }

    public void GenerateProductModels(IProductMasterViewModel viewModel)
    {
        IProductModelOperation operation = IProductModelOperation.CreateModelOperation(new FakeProductCRUD());

        viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(1, "Starcraft", 61.99, 16, operation, _informer));
        viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(2, "Assassin's Creed Valhalla", 239.99, 18, operation, _informer));
        viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(3, "Cyberpunk 2077", 299.99, 18, operation, _informer));
        viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(4, "The Sims 3", 109.99, 7, operation, _informer));
        viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(5, "Witcher 3", 129.99, 18, operation, _informer));
        viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(6, "Diablo 3", 339.99, 18, operation, _informer));
    }

    public void GenerateStateModels(IStateMasterViewModel viewModel)
    {
        IStateModelOperation operation = IStateModelOperation.CreateModelOperation(new FakeStateCRUD());

        viewModel.States.Add(IStateDetailViewModel.CreateViewModel(1, 1, 3, operation, _informer));
        viewModel.States.Add(IStateDetailViewModel.CreateViewModel(2, 2, 9, operation, _informer));
        viewModel.States.Add(IStateDetailViewModel.CreateViewModel(3, 3, 11, operation, _informer));
        viewModel.States.Add(IStateDetailViewModel.CreateViewModel(4, 4, 12, operation, _informer));
        viewModel.States.Add(IStateDetailViewModel.CreateViewModel(5, 5, 22, operation, _informer));
        viewModel.States.Add(IStateDetailViewModel.CreateViewModel(6, 6, 33, operation, _informer));
    }

    public void GenerateEventModels(IEventMasterViewModel viewModel)
    {
        IEventModelOperation operation = IEventModelOperation.CreateModelOperation(new FakeEventCRUD());

        viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(1, 1, 1, DateTime.Now, "SupplyEvent", 10, operation, _informer));
        viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(2, 2, 2, DateTime.Now, "SupplyEvent", 123, operation, _informer));
        viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(3, 3, 3, DateTime.Now, "SupplyEvent", 3, operation, _informer));
        viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(4, 4, 4, DateTime.Now, "SupplyEvent", 5, operation, _informer));
        viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(5, 5, 5, DateTime.Now, "SupplyEvent", 15, operation, _informer));
        viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(6, 6, 6, DateTime.Now, "SupplyEvent", 23, operation, _informer));

    }
}
