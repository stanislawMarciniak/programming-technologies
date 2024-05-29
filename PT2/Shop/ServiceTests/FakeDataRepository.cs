using Data.API;

namespace ServiceTests;

internal class FakeDataRepository : IDataRepository
{
    public Dictionary<int, IUser> Users = new Dictionary<int, IUser>();

    public Dictionary<int, IProduct> Products = new Dictionary<int, IProduct>();

    public Dictionary<int, IEvent> Events = new Dictionary<int, IEvent>();

    public Dictionary<int, IState> States = new Dictionary<int, IState>();

    #region User CRUD

    public async Task AddUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        this.Users.Add(id, new FakeUser(id, nickname, email, balance, dateOfBirth));
    }

    public async Task<IUser> GetUserAsync(int id)
    {
        return await Task.FromResult(this.Users[id]);
    }

    public async Task UpdateUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        this.Users[id].Nickname = nickname;
        this.Users[id].Email = email;
        this.Users[id].Balance = balance;
        this.Users[id].DateOfBirth = dateOfBirth;
    }

    public async Task DeleteUserAsync(int id)
    {
        this.Users.Remove(id);
    }

    public async Task<Dictionary<int, IUser>> GetAllUsersAsync()
    {
        return await Task.FromResult(this.Users);
    }

    public async Task<int> GetUsersCountAsync()
    {
        return await Task.FromResult(this.Users.Count);
    }

    #endregion User CRUD


    #region Product CRUD

    public async Task AddProductAsync(int id, string name, double price, int pegi)
    {
        this.Products.Add(id, new FakeProduct(id, name, price, pegi));
    }

    public async Task<IProduct> GetProductAsync(int id)
    {
        return await Task.FromResult(this.Products[id]);
    }

    public async Task UpdateProductAsync(int id, string name, double price, int pegi)
    {
        this.Products[id].Name = name;
        this.Products[id].Price = price;
        this.Products[id].Pegi = pegi;
    }

    public async Task DeleteProductAsync(int id)
    {
        this.Products.Remove(id);
    }

    public async Task<Dictionary<int, IProduct>> GetAllProductsAsync()
    {
        return await Task.FromResult(this.Products);
    }

    public async Task<int> GetProductsCountAsync()
    {
        return await Task.FromResult(this.Products.Count);
    }

    #endregion


    #region State CRUD

    public async Task AddStateAsync(int id, int productId, int productQuantity)
    {
        this.States.Add(id, new FakeState(id, productId, productQuantity));
    }

    public async Task<IState> GetStateAsync(int id)
    {
        return await Task.FromResult(this.States[id]);
    }

    public async Task UpdateStateAsync(int id, int productId, int productQuantity)
    {
        this.States[id].productId = productId;
        this.States[id].productQuantity = productQuantity;
    }

    public async Task DeleteStateAsync(int id)
    {
        this.States.Remove(id);
    }

    public async Task<Dictionary<int, IState>> GetAllStatesAsync()
    {
        return await Task.FromResult(this.States);
    }

    public async Task<int> GetStatesCountAsync()
    {
        return await Task.FromResult(this.States.Count);
    }

    #endregion


    #region Event CRUD

    public async Task AddEventAsync(int id, int stateId, int userId, string type, int quantity = 0)
    {
        IUser user = await this.GetUserAsync(userId);
        IState state = await this.GetStateAsync(stateId);
        IProduct product = await this.GetProductAsync(state.productId);

        switch (type)
        {
            case "PurchaseEvent":
                if (DateTime.Now.Year - user.DateOfBirth.Year < product.Pegi)
                    throw new Exception("You are not old enough to purchase this game!");

                if (state.productQuantity == 0)
                    throw new Exception("Product unavailable, please check later!");

                if (user.Balance < product.Price)
                    throw new Exception("Not enough money to purchase this product!");

                await this.UpdateStateAsync(stateId, product.Id, state.productQuantity - 1);
                await this.UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance - product.Price, user.DateOfBirth);

                break;

            case "ReturnEvent":
                Dictionary<int, IEvent> events = await this.GetAllEventsAsync();
                Dictionary<int, IState> states = await this.GetAllStatesAsync();

                int copiesBought = 0;

                foreach
                (
                    IEvent even in
                    from even in events.Values
                    from stat in states.Values
                    where even.userId == user.Id &&
                          even.stateId == stat.Id &&
                          stat.productId == product.Id
                    select even
                )
                    if (((FakeEvent)even).Type == "PurchaseEvent")
                        copiesBought++;
                    else if (((FakeEvent)even).Type == "ReturnEvent")
                        copiesBought--;

                copiesBought--;

                if (copiesBought < 0)
                    throw new Exception("You do not own this product!");

                await this.UpdateStateAsync(stateId, product.Id, state.productQuantity + 1);
                await this.UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance + product.Price, user.DateOfBirth);

                break;

            case "SupplyEvent":
                if (quantity <= 0)
                    throw new Exception("Can not supply with this amount!");

                await this.UpdateStateAsync(stateId, product.Id, state.productQuantity + quantity);

                break;
        }

        this.Events.Add(id, new FakeEvent(id, stateId, userId, type, quantity));
    }

    public async Task<IEvent> GetEventAsync(int id)
    {
        return await Task.FromResult(this.Events[id]);
    }

    public async Task UpdateEventAsync(int id, int stateId, int userId, DateTime occurrenceDate, string type, int? quantity)
    {
        ((FakeEvent)this.Events[id]).stateId = stateId;
        ((FakeEvent)this.Events[id]).userId = userId;
        ((FakeEvent)this.Events[id]).occurrenceDate = occurrenceDate;
        ((FakeEvent)this.Events[id]).Type = type;
        ((FakeEvent)this.Events[id]).Quantity = quantity ?? ((FakeEvent)this.Events[id]).Quantity;
    }

    public async Task DeleteEventAsync(int id)
    {
        this.Events.Remove(id);
    }

    public async Task<Dictionary<int, IEvent>> GetAllEventsAsync()
    {
        return await Task.FromResult(this.Events);
    }

    public async Task<int> GetEventsCountAsync()
    {
        return await Task.FromResult(this.Events.Count);
    }

    #endregion
}
