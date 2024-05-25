using PresentationTests.Mocks.DTO;
using Service.API;

namespace PresentationTests.Mocks
{
    internal class MockDataRepository
    {
        public Dictionary<int, IUserDTO> Users = new Dictionary<int, IUserDTO>();
        public Dictionary<int, IProductDTO> Products = new Dictionary<int, IProductDTO>();
        public Dictionary<int, IEventDTO> Events = new Dictionary<int, IEventDTO>();
        public Dictionary<int, IStateDTO> States = new Dictionary<int, IStateDTO>();


        // User CRUD

        public async Task AddUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
        {
            Users.Add(id, new MockUserDTO(id, nickname, email, balance, dateOfBirth));
        }

        public async Task<IUserDTO> GetUserAsync(int id)
        {
            return await Task.FromResult(Users[id]);
        }

        public async Task UpdateUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
        {
            Users[id].Nickname = nickname;
            Users[id].Email = email;
            Users[id].Balance = balance;
            Users[id].DateOfBirth = dateOfBirth;
        }   

        public async Task DeleteUserAsync(int id)
        {
            Users.Remove(id);
        }

        public async Task<Dictionary<int, IUserDTO>> GetAllUsersAsync()
        {
            return await Task.FromResult(Users);
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await Task.FromResult(Users.Count);
        }

        public bool CheckIfUserExists(int id)
        {
            return Users.ContainsKey(id);
        }


        // Product CRUD

        public async Task AddProductAsync(int id, string name, double price, int pegi)
        {
            Products.Add(id, new MockProductDTO(id, name, price, pegi));
        }

        public async Task<IProductDTO> GetProductAsync(int id)
        {
            return await Task.FromResult(Products[id]);
        }

        public async Task UpdateProductAsync(int id, string name, double price, int pegi)
        {
            Products[id].Name = name;
            Products[id].Price = price;
            Products[id].Pegi = pegi;
        }

        public async Task DeleteProductAsync(int id)
        {
            Products.Remove(id);
        }

        public async Task<Dictionary<int, IProductDTO>> GetAllProductsAsync()
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
        }

        public async Task<IStateDTO> GetStateAsync(int id)
        {
            return await Task.FromResult(States[id]);
        }

        public async Task UpdateStateAsync(int id, int productId, int productQuantity)
        {
            States[id].ProductId = productId;
            States[id].ProductQuantity = productQuantity;
        }

        public async Task DeleteStateAsync(int id)
        {
            States.Remove(id);
        }

        public async Task<Dictionary<int, IStateDTO>> GetAllStatesAsync()
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
            IUserDTO user = await GetUserAsync(userId);
            IStateDTO state = await GetStateAsync(stateId);
            IProductDTO product = await GetProductAsync(state.ProductId);

            switch (type)
            {
                case "PurchaseEvent":
                    if (DateTime.Now.Year - user.DateOfBirth.Year < product.Pegi)
                        throw new Exception("You are not old enough to purchase this game!");

                    if (state.ProductQuantity == 0)
                        throw new Exception("Product unavailable, please check later!");

                    if (user.Balance < product.Price)
                        throw new Exception("Not enough money to purchase this product!");

                    await UpdateStateAsync(stateId, product.Id, state.ProductQuantity - 1);
                    await UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance - product.Price, user.DateOfBirth);

                    break;

                case "ReturnEvent":
                    Dictionary<int, IEventDTO> events = await GetAllEventsAsync();
                    Dictionary<int, IStateDTO> states = await GetAllStatesAsync();

                    int copiesBought = 0;

                    foreach
                    (
                        IEventDTO even in
                        from even in events.Values
                        from stat in states.Values
                        where even.UserId == user.Id &&
                              even.StateId == stat.Id &&
                              stat.ProductId == product.Id
                        select even
                    )
                        if (((MockEventDTO)even).Type == "PurchaseEvent")
                            copiesBought++;
                        else if (((MockEventDTO)even).Type == "ReturnEvent")
                            copiesBought--;

                    copiesBought--;

                    if (copiesBought < 0)
                        throw new Exception("You do not own this product!");

                    await UpdateStateAsync(stateId, product.Id, state.ProductQuantity + 1);
                    await UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance + product.Price, user.DateOfBirth);

                    break;

                case "SupplyEvent":
                    if (quantity <= 0)
                        throw new Exception("Can not supply with this amount!");

                    await UpdateStateAsync(stateId, product.Id, state.ProductQuantity + quantity);

                    break;
            }

            Events.Add(id, new MockEventDTO(id, stateId, userId, type, quantity));
        }

        public async Task<IEventDTO> GetEventAsync(int id)
        {
            return await Task.FromResult(Events[id]);
        }

        public async Task UpdateEventAsync(int id, int stateId, int userId, DateTime occurrenceDate, string type, int? quantity)
        {
            ((MockEventDTO)Events[id]).StateId = stateId;
            ((MockEventDTO)Events[id]).UserId = userId;
            ((MockEventDTO)Events[id]).OccurrenceDate = occurrenceDate;
            ((MockEventDTO)Events[id]).Type = type;
            ((MockEventDTO)Events[id]).Quantity = quantity ?? ((MockEventDTO)Events[id]).Quantity;
        }

        public async Task DeleteEventAsync(int id)
        {
            Events.Remove(id);
        }

        public async Task<Dictionary<int, IEventDTO>> GetAllEventsAsync()
        {
            return await Task.FromResult(Events);
        }

        public async Task<int> GetEventsCountAsync()
        {
            return await Task.FromResult(Events.Count);
        }
    }
}
