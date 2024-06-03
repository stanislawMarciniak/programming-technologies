using Presentation;
using Presentation.Model.API;
using Presentation.ViewModel;

namespace PresentationTests;

internal class RandomGenerator : IGenerator
{
    private readonly IErrorInformer _informer = new TextErrorInformer();
    private readonly Random _random = new Random();

    public void GenerateUserModels(IUserMasterViewModel viewModel)
    {
        IUserModelOperation operation = IUserModelOperation.CreateModelOperation(new MockUserCRUD());

        for (int i = 1; i <= 10; i++)
        {
            viewModel.Users.Add(IUserDetailViewModel.CreateViewModel(
                i,
                RandomString(10),
                RandomEmail(),
                _random.Next(0, 10000),
                RandomDate(),
                operation,
                _informer));
        }
    }

    public void GenerateProductModels(IProductMasterViewModel viewModel)
    {
        IProductModelOperation operation = IProductModelOperation.CreateModelOperation(new MockProductCRUD());

        for (int i = 1; i <= 10; i++)
        {
            viewModel.Products.Add(IProductDetailViewModel.CreateViewModel(
                i,
                RandomString(12),
                _random.NextDouble() * 1000,
                RandomPEGI(),
                operation,
                _informer));
        }
    }

    public void GenerateStateModels(IStateMasterViewModel viewModel)
    {
        IStateModelOperation operation = IStateModelOperation.CreateModelOperation(new MockStateCRUD());

        for (int i = 1; i <= 10; i++)
        {
            viewModel.States.Add(IStateDetailViewModel.CreateViewModel(
                i,
                i,
                _random.Next(1, 100),
                operation,
                _informer));
        }
    }

    public void GenerateEventModels(IEventMasterViewModel viewModel)
    {
        IEventModelOperation operation = IEventModelOperation.CreateModelOperation(new MockEventCRUD());

        for (int i = 1; i <= 10; i++)
        {
            viewModel.Events.Add(IEventDetailViewModel.CreateViewModel(
                i,
                i,
                i,
                DateTime.Now,
                RandomEventType(),
                _random.Next(1, 50),
                operation,
                _informer));
        }
    }

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        char[] stringChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[_random.Next(chars.Length)];
        }

        return new string(stringChars);
    }

    private string RandomEmail()
    {
        return $"{RandomString(5)}@{RandomString(5)}.com";
    }

    private DateTime RandomDate()
    {
        int year = _random.Next(1970, DateTime.Now.Year);
        int month = _random.Next(1, 13);
        int day = _random.Next(1, DateTime.DaysInMonth(year, month) + 1);

        return new DateTime(year, month, day);
    }

    private int RandomPEGI()
    {
        int[] pegiRatings = { 3, 7, 12, 16, 18 };
        return pegiRatings[_random.Next(pegiRatings.Length)];
    }

    private string RandomEventType()
    {
        string[] eventTypes = { "SupplyEvent", "PurchaseEvent", "ReturnEvent" };
        return eventTypes[_random.Next(eventTypes.Length)];
    }
}
