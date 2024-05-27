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
            await Task.CompletedTask;
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
            await Task.CompletedTask;
        }

        public async Task<Dictionary<int, IUserDTO>> GetAllUsersAsync()
        {
            return await Task.FromResult(Users);
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await Task.FromResult(Users.Count);
        }


        // Movie CRUD

        public async Task AddProductAsync(int id, string name, double price, int ageRestriction)
        {
            Products.Add(id, new MockProductDTO(id, name, price, ageRestriction));
            await Task.CompletedTask;
        }

        public async Task<IProductDTO> GetProductAsync(int id)
        {
            return await Task.FromResult(Products[id]);
        }

        public async Task UpdateProductAsync(int id, string name, double price, int ageRestriction)
        {
            Products[id].Name = name;
            Products[id].Price = price;
            Products[id].AgeRestriction = ageRestriction;
            await Task.CompletedTask;
        }

        public async Task DeleteProductAsync(int id)
        {
            Products.Remove(id);
            await Task.CompletedTask;
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
            await Task.CompletedTask;
        }

        public async Task<IStateDTO> GetStateAsync(int id)
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
            IProductDTO movie = await GetProductAsync(state.productId);

            switch (type)
            {
                case "PurchaseEvent":
                    if (DateTime.Now.Year - user.DateOfBirth.Year < movie.AgeRestriction)
                        throw new Exception("You are not allowed to buy this movie");

                    if (state.productQuantity == 0)
                        throw new Exception("Movie unavailable.");

                    if (user.Balance < movie.Price)
                        throw new Exception("Not enough balance!");

                    await UpdateStateAsync(stateId, movie.Id, state.productQuantity - 1);
                    await UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance - movie.Price, user.DateOfBirth);
                    break;

                case "ReturnEvent":
                    var events = await GetAllEventsAsync();
                    var states = await GetAllStatesAsync();

                    int copiesBought = 0;

                    foreach (var even in events.Values)
                    {
                        if (even.userId == user.Id && states[even.stateId].productId == movie.Id)
                        {
                            if (((MockEventDTO)even).Type == "PurchaseEvent")
                                copiesBought++;
                            else if (((MockEventDTO)even).Type == "ReturnEvent")
                                copiesBought--;
                        }
                    }

                    if (copiesBought <= 0)
                        throw new Exception("You do not own this movie!");

                    await UpdateStateAsync(stateId, movie.Id, state.productQuantity + 1);
                    await UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance + movie.Price, user.DateOfBirth);
                    break;

                case "SupplyEvent":
                    if (quantity <= 0)
                        throw new Exception("Can not supply with this amount!");

                    await UpdateStateAsync(stateId, movie.Id, state.productQuantity + quantity);
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
