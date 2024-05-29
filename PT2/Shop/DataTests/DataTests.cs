using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data.API;

namespace DataTests;

[TestClass]
[DeploymentItem("DatabaseForTests.mdf")]
public class DataTests
{
    private static string connectionString;

    private readonly IDataRepository _dataRepository = IDataRepository.CreateDatabase(IDataContext.CreateContext(connectionString));

    [ClassInitialize]
    public static void ClassInitializeMethod(TestContext context)
    {
        string _DBRelativePath = @"DatabaseForTests.mdf";
        string _projectRootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string _DBPath = Path.Combine(_projectRootDir, _DBRelativePath);
        FileInfo _databaseFile = new FileInfo(_DBPath);
        Assert.IsTrue(_databaseFile.Exists, $"{Environment.CurrentDirectory}");
        connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={_DBPath};Integrated Security = True; Connect Timeout = 30;";
    }

    [TestMethod]
    public async Task UserTests()
    {
        int userId = 1;

        await _dataRepository.AddUserAsync(userId, "John", "m_gapcio@gmail.com", 21, new DateTime(2015, 12, 25));

        IUser user = await _dataRepository.GetUserAsync(userId);

        Assert.IsNotNull(user);
        Assert.AreEqual(userId, user.Id);
        Assert.AreEqual("John", user.Nickname);
        Assert.AreEqual("m_gapcio@gmail.com", user.Email);
        Assert.AreEqual(21, user.Balance);
        Assert.AreEqual(new DateTime(2015, 12, 25), user.DateOfBirth);

        Assert.IsNotNull(await _dataRepository.GetAllUsersAsync());
        Assert.IsTrue(await _dataRepository.GetUsersCountAsync() > 0);

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetUserAsync(userId + 2));

        await _dataRepository.UpdateUserAsync(userId, "Tom", "t_pokot@gmail.com", 301, new DateTime(2015, 12, 12));

        IUser userUpdated = await _dataRepository.GetUserAsync(userId);

        Assert.IsNotNull(userUpdated);
        Assert.AreEqual(userId, userUpdated.Id);
        Assert.AreEqual("Tom", userUpdated.Nickname);
        Assert.AreEqual("t_pokot@gmail.com", userUpdated.Email);
        Assert.AreEqual(301, userUpdated.Balance);
        Assert.AreEqual(new DateTime(2015, 12, 12), userUpdated.DateOfBirth);

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateUserAsync(userId + 2,
            "Tom", "t_pokot@gmail.com", 301, new DateTime(2015, 12, 12)));

        await _dataRepository.DeleteUserAsync(userId);
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetUserAsync(userId));
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.DeleteUserAsync(userId));
    }

    [TestMethod]
    public async Task ProductTests()
    {
        int productId = 2;

        await _dataRepository.AddProductAsync(productId, "Assassin's Creed Valhalla", 240, 18);

        IProduct product = await _dataRepository.GetProductAsync(productId);

        Assert.IsNotNull(product);
        Assert.AreEqual(productId, product.Id);
        Assert.AreEqual("Assassin's Creed Valhalla", product.Name);
        Assert.AreEqual(240, product.Price);
        Assert.AreEqual(18, product.Pegi);

        Assert.IsNotNull(await _dataRepository.GetAllProductsAsync());
        Assert.IsTrue(await _dataRepository.GetProductsCountAsync() > 0);

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetProductAsync(12));

        await _dataRepository.UpdateProductAsync(productId, "Cyberpunk 2077", 300, 15);

        IProduct productUpdated = await _dataRepository.GetProductAsync(productId);

        Assert.IsNotNull(productUpdated);
        Assert.AreEqual(productId, productUpdated.Id);
        Assert.AreEqual("Cyberpunk 2077", productUpdated.Name);
        Assert.AreEqual(300, productUpdated.Price);
        Assert.AreEqual(15, productUpdated.Pegi);

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateProductAsync(12, "Cyberpunk 2077", 300, 15));

        await _dataRepository.DeleteProductAsync(productId);
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetProductAsync(productId));
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.DeleteProductAsync(productId));
    }

    [TestMethod]
    public async Task StateTests()
    {
        int productId = 3;
        int stateId = 3;

        await _dataRepository.AddProductAsync(productId, "Assassin's Creed Valhalla", 240, 18);

        IProduct product = await _dataRepository.GetProductAsync(productId);

        await _dataRepository.AddStateAsync(stateId, productId, 12);

        IState state = await _dataRepository.GetStateAsync(stateId);

        Assert.IsNotNull(state);
        Assert.AreEqual(stateId, state.Id);
        Assert.AreEqual(productId, state.productId);
        Assert.AreEqual(12, state.productQuantity);

        Assert.IsNotNull(await _dataRepository.GetAllStatesAsync());
        Assert.IsTrue(await _dataRepository.GetStatesCountAsync() > 0);

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetStateAsync(stateId + 2));

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddStateAsync(stateId, 13, 12));

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddStateAsync(stateId, productId, -1));

        await _dataRepository.UpdateStateAsync(stateId, productId, 9);

        IState stateUpdated = await _dataRepository.GetStateAsync(stateId);

        Assert.IsNotNull(stateUpdated);
        Assert.AreEqual(stateId, stateUpdated.Id);
        Assert.AreEqual(productId, stateUpdated.productId);
        Assert.AreEqual(9, stateUpdated.productQuantity);

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateStateAsync(stateId + 2, productId, 12));
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateStateAsync(stateId, 13, 12));
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.UpdateStateAsync(stateId, productId, -12));

        await _dataRepository.DeleteStateAsync(stateId);
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.GetStateAsync(stateId));
        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.DeleteStateAsync(stateId));

        await _dataRepository.DeleteProductAsync(productId);
    }

    [TestMethod]
    public async Task EventTests()
    {
        int purchaseEventId = 4;
        int userId = 4;
        int productId = 4;
        int stateId = 4;

        await _dataRepository.AddProductAsync(productId, "Assassin's Creed Valhalla", 240, 18);
        await _dataRepository.AddStateAsync(stateId, productId, 12);
        await _dataRepository.AddUserAsync(userId, "John", "m_gapcio@gmail.com", 2100, new DateTime(2000, 12, 25));

        IProduct product = await _dataRepository.GetProductAsync(productId);
        IState state = await _dataRepository.GetStateAsync(stateId);
        IUser user = await _dataRepository.GetUserAsync(userId);

        await _dataRepository.AddEventAsync(purchaseEventId, stateId, userId, "PurchaseEvent");

        IEvent purchaseEvent = await _dataRepository.GetEventAsync(purchaseEventId);

        Assert.IsNotNull(purchaseEvent);
        Assert.AreEqual(purchaseEventId, purchaseEvent.Id);
        Assert.AreEqual(stateId, purchaseEvent.stateId);
        Assert.AreEqual(userId, purchaseEvent.userId);

        Assert.IsNotNull(await _dataRepository.GetAllEventsAsync());
        Assert.IsTrue(await _dataRepository.GetEventsCountAsync() > 0);

        await _dataRepository.UpdateEventAsync(purchaseEventId, stateId, userId, DateTime.Now,  "PurchaseEvent", null);

        IEvent eventUpdated = await _dataRepository.GetEventAsync(purchaseEventId);

        Assert.IsNotNull(eventUpdated);
        Assert.AreEqual(purchaseEventId, eventUpdated.Id);
        Assert.AreEqual(stateId, eventUpdated.stateId);
        Assert.AreEqual(userId, eventUpdated.userId);

        await _dataRepository.DeleteEventAsync(purchaseEventId);
        await _dataRepository.DeleteStateAsync(stateId);
        await _dataRepository.DeleteProductAsync(productId);
        await _dataRepository.DeleteUserAsync(userId);
    }

    [TestMethod]
    public async Task EventsActionTest()
    {
        int userId1 = 5;
        int stateId1 = 5;
        int purchaseEventId1 = 5;
        int productId1 = 5;

        await _dataRepository.AddProductAsync(productId1, "Assassin's Creed Valhalla", 240, 18);
        await _dataRepository.AddStateAsync(stateId1, productId1, 12);
        await _dataRepository.AddUserAsync(userId1, "John", "m_gapcio@gmail.com", 300, new DateTime(2000, 12, 25));
        await _dataRepository.AddEventAsync(purchaseEventId1, stateId1, userId1, "PurchaseEvent");

        Assert.AreEqual(60, (await _dataRepository.GetUserAsync(userId1)).Balance);
        Assert.AreEqual(11, (await _dataRepository.GetStateAsync(stateId1)).productQuantity);

        await _dataRepository.DeleteEventAsync(purchaseEventId1);
        await _dataRepository.DeleteStateAsync(stateId1);
        await _dataRepository.DeleteProductAsync(productId1);
        await _dataRepository.DeleteUserAsync(userId1);

        int userId2 = 5;
        int stateId2 = 5;
        int purchaseEventId2 = 5;
        int productId2 = 5;

        await _dataRepository.AddProductAsync(productId2, "Assassin's Creed Valhalla", 240, 18);
        await _dataRepository.AddStateAsync(stateId2, productId2, 12);
        await _dataRepository.AddUserAsync(userId2, "John", "m_gapcio@gmail.com", 300, new DateTime(2012, 12, 25));

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(purchaseEventId2, stateId2, userId2, "PurchaseEvent"));

        await _dataRepository.DeleteStateAsync(stateId2);
        await _dataRepository.DeleteProductAsync(productId2);
        await _dataRepository.DeleteUserAsync(userId2);

        int userId3 = 5;
        int stateId3 = 5;
        int purchaseEventId3 = 5;
        int productId3 = 5;

        await _dataRepository.AddProductAsync(productId3, "Assassin's Creed Valhalla", 240, 18);
        await _dataRepository.AddStateAsync(stateId3, productId3, 0);
        await _dataRepository.AddUserAsync(userId3, "John", "m_gapcio@gmail.com", 300, new DateTime(2000, 12, 25));

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(purchaseEventId3, stateId3, userId3, "PurchaseEvent"));

        await _dataRepository.DeleteStateAsync(stateId3);
        await _dataRepository.DeleteProductAsync(productId3);
        await _dataRepository.DeleteUserAsync(userId3);

        int userId4 = 5;
        int stateId4 = 5;
        int purchaseEventId4 = 5;
        int productId4 = 5;

        await _dataRepository.AddProductAsync(productId4, "Assassin's Creed Valhalla", 240, 18);
        await _dataRepository.AddStateAsync(stateId4, productId4, 2);
        await _dataRepository.AddUserAsync(userId4, "John", "m_gapcio@gmail.com", 120, new DateTime(2000, 12, 25));

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(purchaseEventId4, stateId4, userId4, "PurchaseEvent"));

        await _dataRepository.DeleteStateAsync(stateId4);
        await _dataRepository.DeleteProductAsync(productId4);
        await _dataRepository.DeleteUserAsync(userId4);

        int userId5 = 5;
        int stateId5 = 5;
        int purchaseEventId5 = 5;
        int returnEventId5 = 6;
        int productId5 = 5;

        await _dataRepository.AddProductAsync(productId5, "Assassin's Creed Valhalla", 240, 18);
        await _dataRepository.AddStateAsync(stateId5, productId5, 2);
        await _dataRepository.AddUserAsync(userId5, "John", "m_gapcio@gmail.com", 300, new DateTime(2000, 12, 25));

        await _dataRepository.AddEventAsync(purchaseEventId5, stateId5, userId5, "PurchaseEvent");
        await _dataRepository.AddEventAsync(returnEventId5, stateId5, userId5, "ReturnEvent");

        Assert.AreEqual(300, (await _dataRepository.GetUserAsync(userId5)).Balance);
        Assert.AreEqual(2, (await _dataRepository.GetStateAsync(stateId5)).productQuantity);

        await _dataRepository.DeleteEventAsync(returnEventId5);
        await _dataRepository.DeleteEventAsync(purchaseEventId5);
        await _dataRepository.DeleteStateAsync(stateId5);
        await _dataRepository.DeleteProductAsync(productId5);
        await _dataRepository.DeleteUserAsync(userId5);

        int userId6 = 7;
        int stateId6 = 7;
        int returnEventId6 = 7;
        int productId6 = 7;

        await _dataRepository.AddProductAsync(productId6, "Assassin's Creed Valhalla", 240, 18);
        await _dataRepository.AddStateAsync(stateId6, productId6, 2);
        await _dataRepository.AddUserAsync(userId6, "John", "m_gapcio@gmail.com", 300, new DateTime(2000, 12, 25));

        await Assert.ThrowsExceptionAsync<Exception>(async () => await _dataRepository.AddEventAsync(returnEventId6, stateId6, userId6, "ReturnEvent"));

        Assert.AreEqual(300, (await _dataRepository.GetUserAsync(userId6)).Balance);
        Assert.AreEqual(2, (await _dataRepository.GetStateAsync(stateId6)).productQuantity);


        await _dataRepository.DeleteStateAsync(stateId6);
        await _dataRepository.DeleteProductAsync(productId6);
        await _dataRepository.DeleteUserAsync(userId6);

        int userId7 = 8;
        int stateId7 = 8;
        int supplyEventId7 = 8;
        int productId7 = 8;

        await _dataRepository.AddProductAsync(productId7, "Assassin's Creed Valhalla", 240, 18);
        await _dataRepository.AddStateAsync(stateId7, productId7, 2);
        await _dataRepository.AddUserAsync(userId7, "John", "m_gapcio@gmail.com", 300, new DateTime(2000, 12, 25));

        await _dataRepository.AddEventAsync(supplyEventId7, stateId7, userId7, "SupplyEvent", 12);

        Assert.AreEqual(14, (await _dataRepository.GetStateAsync(stateId7)).productQuantity);

        await _dataRepository.DeleteEventAsync(supplyEventId7);
        await _dataRepository.DeleteStateAsync(stateId7);
        await _dataRepository.DeleteProductAsync(productId7);
        await _dataRepository.DeleteUserAsync(userId7);
    }
}
