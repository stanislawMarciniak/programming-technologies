using Data.API;
using Service.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceTests;

[TestClass]
public class ServiceTests
{
    private readonly IDataRepository _repository = new FakeDataRepository();

    [TestMethod]
    public async Task UserServiceTests()
    {
        IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(this._repository);

        await userCrud.AddUserAsync(1, "Maciek", "maciek.kowalski@gmail.pl", 1200, new DateTime(2001, 10, 10));

        IUserDTO user = await userCrud.GetUserAsync(1);

        Assert.IsNotNull(user);
        Assert.AreEqual(1, user.Id);
        Assert.AreEqual("Maciek", user.Nickname);
        Assert.AreEqual("maciek.kowalski@gmail.pl", user.Email);
        Assert.AreEqual(1200, user.Balance);
        Assert.AreEqual(new DateTime(2001, 10, 10), user.DateOfBirth);

        Assert.IsNotNull(await userCrud.GetAllUsersAsync());
        Assert.IsTrue(await userCrud.GetUsersCountAsync() > 0);

        await userCrud.UpdateUserAsync(1, "Patrycja", "patrycja.palanowska@gmail.com", 300, new DateTime(2001, 1, 1));

        IUserDTO updatedUser = await userCrud.GetUserAsync(1);

        Assert.IsNotNull(updatedUser);
        Assert.AreEqual(1, updatedUser.Id);
        Assert.AreEqual("Patrycja", updatedUser.Nickname);
        Assert.AreEqual("patrycja.palanowska@gmail.com", updatedUser.Email);
        Assert.AreEqual(300, updatedUser.Balance);
        Assert.AreEqual(new DateTime(2001, 1, 1), updatedUser.DateOfBirth);

        await userCrud.DeleteUserAsync(1);
    }

    [TestMethod]
    public async Task ProductServiceTests()
    {
        IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(this._repository);

        await productCrud.AddProductAsync(1, "Kanapa", 200, 18);

        IProductDTO product = await productCrud.GetProductAsync(1);

        Assert.IsNotNull(product);
        Assert.AreEqual(1, product.Id);
        Assert.AreEqual("Kanapa", product.Name);
        Assert.AreEqual(200, product.Price);
        Assert.AreEqual(18, product.Pegi);

        Assert.IsNotNull(await productCrud.GetAllProductsAsync());
        Assert.IsTrue(await productCrud.GetProductsCountAsync() > 0);

        await productCrud.UpdateProductAsync(1, "Pralka", 300, 12);

        IProductDTO updatedProduct = await productCrud.GetProductAsync(1);

        Assert.IsNotNull(updatedProduct);
        Assert.AreEqual(1, updatedProduct.Id);
        Assert.AreEqual("Pralka", updatedProduct.Name);
        Assert.AreEqual(300, updatedProduct.Price);
        Assert.AreEqual(12, updatedProduct.Pegi);

        await productCrud.DeleteProductAsync(1);
    }

    [TestMethod]
    public async Task StateServiceTests()
    {
        IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(this._repository);

        await productCrud.AddProductAsync(1, "Kanapa", 200, 18);

        IProductDTO product = await productCrud.GetProductAsync(1);

        IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(this._repository);

        await stateCrud.AddStateAsync(1, product.Id, 10);

        IStateDTO state = await stateCrud.GetStateAsync(1);

        Assert.IsNotNull(state);
        Assert.AreEqual(1, state.Id);
        Assert.AreEqual(1, state.productId);
        Assert.AreEqual(10, state.productQuantity);

        await stateCrud.UpdateStateAsync(1, product.Id, 12);

        IStateDTO updatedState = await stateCrud.GetStateAsync(1);

        Assert.IsNotNull(updatedState);
        Assert.AreEqual(1, updatedState.Id);
        Assert.AreEqual(1, updatedState.productId);
        Assert.AreEqual(12, updatedState.productQuantity);

        await stateCrud.DeleteStateAsync(1);
        await productCrud.DeleteProductAsync(1);
    }

    [TestMethod]
    public async Task PurchaseEventServiceTests()
    {
        IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(this._repository);

        await productCrud.AddProductAsync(1, "Kanapa", 200, 18);

        IProductDTO product = await productCrud.GetProductAsync(1);

        IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(this._repository);

        await stateCrud.AddStateAsync(1, product.Id, 10);

        IStateDTO state = await stateCrud.GetStateAsync(1);

        IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(this._repository);

        await userCrud.AddUserAsync(1, "Maciek", "maciek.kowalski@gmail.pl", 1200, new DateTime(1990, 10, 10));

        IUserDTO user = await userCrud.GetUserAsync(1);

        IEventCRUD eventCrud = IEventCRUD.CreateEventCRUD(this._repository);

        await eventCrud.AddEventAsync(1, state.Id, user.Id, "PurchaseEvent");

        user = await userCrud.GetUserAsync(1);
        state = await stateCrud.GetStateAsync(1);

        Assert.AreEqual(1000, user.Balance);
        Assert.AreEqual(9, state.productQuantity);

        await eventCrud.DeleteEventAsync(1);
        await stateCrud.DeleteStateAsync(1);
        await productCrud.DeleteProductAsync(1);
        await userCrud.DeleteUserAsync(1);
    }

    [TestMethod]
    public async Task ReturnEventServiceTests()
    {
        IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(this._repository);

        await productCrud.AddProductAsync(2, "Kanapa", 200, 18);

        IProductDTO product = await productCrud.GetProductAsync(2);

        IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(this._repository);

        await stateCrud.AddStateAsync(2, product.Id, 10);

        IStateDTO state = await stateCrud.GetStateAsync(2);

        IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(this._repository);

        await userCrud.AddUserAsync(2, "Maciek", "maciek.kowalski@gmail.pl", 1200, new DateTime(1990, 10, 10));

        IUserDTO user = await userCrud.GetUserAsync(2);

        IEventCRUD eventCrud = IEventCRUD.CreateEventCRUD(this._repository);

        await eventCrud.AddEventAsync(2, state.Id, user.Id, "PurchaseEvent");

        await eventCrud.AddEventAsync(3, state.Id, user.Id, "ReturnEvent");

        user = await userCrud.GetUserAsync(2);
        state = await stateCrud.GetStateAsync(2);

        Assert.AreEqual(1200, user.Balance);
        Assert.AreEqual(10, state.productQuantity);

        await eventCrud.DeleteEventAsync(2);
        await eventCrud.DeleteEventAsync(3);
        await stateCrud.DeleteStateAsync(2);
        await productCrud.DeleteProductAsync(2);
        await userCrud.DeleteUserAsync(2);
    }

    [TestMethod]
    public async Task SupplyEventServiceTests()
    {
        IProductCRUD productCrud = IProductCRUD.CreateProductCRUD(this._repository);

        await productCrud.AddProductAsync(4, "Kanapa", 200, 18);

        IProductDTO product = await productCrud.GetProductAsync(4);

        IStateCRUD stateCrud = IStateCRUD.CreateStateCRUD(this._repository);

        await stateCrud.AddStateAsync(4, product.Id, 10);

        IStateDTO state = await stateCrud.GetStateAsync(4);

        IUserCRUD userCrud = IUserCRUD.CreateUserCRUD(this._repository);

        await userCrud.AddUserAsync(4, "Maciek", "maciek.kowalski@gmail.pl", 1200, new DateTime(1990, 10, 10));

        IUserDTO user = await userCrud.GetUserAsync(4);

        IEventCRUD eventCrud = IEventCRUD.CreateEventCRUD(this._repository);

        await eventCrud.AddEventAsync(4, state.Id, user.Id, "SupplyEvent", 20);

        state = await stateCrud.GetStateAsync(4);

        Assert.AreEqual(30, state.productQuantity);

        await eventCrud.DeleteEventAsync(4);
        await stateCrud.DeleteStateAsync(4);
        await productCrud.DeleteProductAsync(4);
        await userCrud.DeleteUserAsync(4);
    }
}
