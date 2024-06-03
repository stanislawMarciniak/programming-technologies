using Data.API;

namespace ServiceTests
{
    internal class MockDataRepository : IDataRepository
    {
        // In-memory storage for mock data
        public Dictionary<int, IUser> Users = new Dictionary<int, IUser>();
        public Dictionary<int, IProduct> Products = new Dictionary<int, IProduct>();
        public Dictionary<int, IEvent> Events = new Dictionary<int, IEvent>();
        public Dictionary<int, IState> States = new Dictionary<int, IState>();

        // User CRUD

        public async Task AddUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
        {
            Users.Add(id, new MockUserDTO(id, nickname, email, balance, dateOfBirth));
            await Task.CompletedTask;
        }

        public async Task<IUser> GetUserAsync(int id)
        {
            return await Task.FromResult(Users[id]);
        }

        public async Task UpdateUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
        {
            Users[id].Nickname = nickname;
            Users[id].Email = email;
            Users[id].Balance = balance;
            Users[id].DateOfBirth = dateOfBirth;
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(int id)
        {
            Users.Remove(id);
            await Task.CompletedTask;
        }

        public async Task<Dictionary<int, IUser>> GetAllUsersAsync()
        {
            return await Task.FromResult(Users);
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await Task.FromResult(Users.Count);
        }

        // Product CRUD

        public async Task AddProductAsync(int id, string name, double price, int pegi)
        {
            Products.Add(id, new MockProductDTO(id, name, price, pegi));
            await Task.CompletedTask;
        }

        public async Task<IProduct> GetProductAsync(int id)
        {
            return await Task.FromResult(Products[id]);
        }

        public async Task UpdateProductAsync(int id, string name, double price, int pegi)
        {
            Products[id].Name = name;
            Products[id].Price = price;
            Products[id].Pegi = pegi;
            await Task.CompletedTask;
        }

        public async Task DeleteProductAsync(int id)
        {
            Products.Remove(id);
            await Task.CompletedTask;
        }

        public async Task<Dictionary<int, IProduct>> GetAllProductsAsync()
        {
            return await Task.FromResult(Products);
        }

        public async Task<int> GetProductsCountAsync()
        {
            return await Task.FromResult(Products.Count);
        }

        // State CRUD

        public async Task AddStateAsync(int id, int productId, int productQuantity)
        {
            States.Add(id, new MockStateDTO(id, productId, productQuantity));
            await Task.CompletedTask;
        }

        public async Task<IState> GetStateAsync(int id)
        {
            return await Task.FromResult(States[id]);
        }

        public async Task UpdateStateAsync(int id, int productId, int productQuantity)
        {
            States[id].productId = productId;
            States[id].productQuantity = productQuantity;
            await Task.CompletedTask;
        }

        public async Task DeleteStateAsync(int id)
        {
            States.Remove(id);
            await Task.CompletedTask;
        }

        public async Task<Dictionary<int, IState>> GetAllStatesAsync()
        {
            return await Task.FromResult(States);
        }

        public async Task<int> GetStatesCountAsync()
        {
            return await Task.FromResult(States.Count);
        }

        // Event CRUD

        public async Task AddEventAsync(int id, int stateId, int userId, string type, int quantity = 0)
        {
            IUser user = await GetUserAsync(userId);
            IState state = await GetStateAsync(stateId);
            IProduct product = await GetProductAsync(state.productId);

            switch (type)
            {
                case "PurchaseEvent":
                    if (DateTime.Now.Year - user.DateOfBirth.Year < product.Pegi)
                        throw new Exception("You are not allowed to buy this product");

                    if (state.productQuantity == 0)
                        throw new Exception("Product unavailable.");

                    if (user.Balance < product.Price)
                        throw new Exception("Not enough balance!");

                    await UpdateStateAsync(stateId, product.Id, state.productQuantity - 1);
                    await UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance - product.Price, user.DateOfBirth);
                    break;

                case "ReturnEvent":
                    var events = await GetAllEventsAsync();
                    var states = await GetAllStatesAsync();

                    int copiesBought = 0;

                    foreach (var even in events.Values)
                    {
                        if (even.userId == user.Id && states[even.stateId].productId == product.Id)
                        {
                            if (((MockEventDTO)even).Type == "PurchaseEvent")
                                copiesBought++;
                            else if (((MockEventDTO)even).Type == "ReturnEvent")
                                copiesBought--;
                        }
                    }

                    if (copiesBought <= 0)
                        throw new Exception("You do not own this product!");

                    await UpdateStateAsync(stateId, product.Id, state.productQuantity + 1);
                    await UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance + product.Price, user.DateOfBirth);
                    break;

                case "SupplyEvent":
                    if (quantity <= 0)
                        throw new Exception("Can not supply with this amount!");

                    await UpdateStateAsync(stateId, product.Id, state.productQuantity + quantity);
                    break;
            }

            Events.Add(id, new MockEventDTO(id, stateId, userId, type, quantity));
        }

        public async Task<IEvent> GetEventAsync(int id)
        {
            return await Task.FromResult(Events[id]);
        }

        public async Task UpdateEventAsync(int id, int stateId, int userId, DateTime occurrenceDate, string type, int? quantity)
        {
            var evnt = (MockEventDTO)Events[id];
            evnt.stateId = stateId;
            evnt.userId = userId;
            evnt.occurrenceDate = occurrenceDate;
            evnt.Type = type;
            evnt.Quantity = quantity ?? evnt.Quantity;
            await Task.CompletedTask;
        }

        public async Task DeleteEventAsync(int id)
        {
            Events.Remove(id);
            await Task.CompletedTask;
        }

        public async Task<Dictionary<int, IEvent>> GetAllEventsAsync()
        {
            return await Task.FromResult(Events);
        }

        public async Task<int> GetEventsCountAsync()
        {
            return await Task.FromResult(Events.Count);
        }
    }
}
