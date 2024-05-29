using Data.API;

namespace Data.Implementation;

internal class DataRepository : IDataRepository
{
    private IDataContext _context;

    public DataRepository(IDataContext context) 
    {
        this._context = context;
    }

    #region User CRUD

    public async Task AddUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        IUser user = new User(id, nickname, email, balance, dateOfBirth);

        await this._context.AddUserAsync(user);
    }

    public async Task<IUser> GetUserAsync(int id)
    {
        IUser? user = await this._context.GetUserAsync(id);

        if (user is null)
            throw new Exception("This user does not exist!");

        return user;
    }

    public async Task UpdateUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        IUser user = new User(id, nickname, email, balance, dateOfBirth);

        if (!await this.CheckIfUserExists(user.Id))
            throw new Exception("This user does not exist");

        await this._context.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        if (!await this.CheckIfUserExists(id))
            throw new Exception("This user does not exist");

        await this._context.DeleteUserAsync(id);
    }

    public async Task<Dictionary<int, IUser>> GetAllUsersAsync()
    {
        return await this._context.GetAllUsersAsync();
    }

    public async Task<int> GetUsersCountAsync()
    {
        return await this._context.GetUsersCountAsync();
    }

    #endregion


    #region Movie CRUD

    public async Task AddMovieAsync(int id, string name, double price, int ageRestriction)
    {
        IMovie movie = new Movie(id, name, price, ageRestriction);

        await this._context.AddMovieAsync(movie);
    }

    public async Task<IMovie> GetMovieAsync(int id)
    {
        IMovie? movie = await this._context.GetMovieAsync(id);

        if (movie is null)
            throw new Exception("This movie does not exist!");

        return movie;
    }

    public async Task UpdateMovieAsync(int id, string name, double price, int ageRestriction)
    {
        IMovie movie = new Movie(id, name, price, ageRestriction);

        if (!await this.CheckIfMovieExists(movie.Id))
            throw new Exception("This movie does not exist");

        await this._context.UpdateMovieAsync(movie);
    }

    public async Task DeleteMovieAsync(int id)
    {
        if (!await this.CheckIfMovieExists(id))
            throw new Exception("This movie does not exist");

        await this._context.DeleteMovieAsync(id);
    }

    public async Task<Dictionary<int, IMovie>> GetAllMoviesAsync()
    {
        return await this._context.GetAllMoviesAsync();
    }

    public async Task<int> GetMoviesCountAsync()
    {
        return await this._context.GetMoviesCountAsync();
    }

    #endregion


    #region State CRUD

    public async Task AddStateAsync(int id, int movieId, int movieQuantity)
    {
        if (!await this._context.CheckIfMovieExists(movieId))
            throw new Exception("This movie does not exist!");

        if (movieQuantity < 0)
            throw new Exception("Movie's quantity must be number greater that 0!");

        IState state = new State(id, movieId, movieQuantity);

        await this._context.AddStateAsync(state);
    }

    public async Task<IState> GetStateAsync(int id)
    {
        IState? state = await this._context.GetStateAsync(id);

        if (state is null)
            throw new Exception("This state does not exist!");

        return state;
    }

    public async Task UpdateStateAsync(int id, int movieId, int movieQuantity)
    {
        if (!await this._context.CheckIfMovieExists(movieId))
            throw new Exception("This movie does not exist!");

        if (movieQuantity <= 0)
            throw new Exception("Movie's quantity must be number greater that 0!");

        IState state = new State(id, movieId, movieQuantity);

        if (!await this.CheckIfStateExists(state.Id))
            throw new Exception("This state does not exist");

        await this._context.UpdateStateAsync(state);
    }

    public async Task DeleteStateAsync(int id)
    {
        if (!await this.CheckIfStateExists(id))
            throw new Exception("This state does not exist");

        await this._context.DeleteStateAsync(id);
    }

    public async Task<Dictionary<int, IState>> GetAllStatesAsync()
    {
        return await this._context.GetAllStatesAsync();
    }

    public async Task<int> GetStatesCountAsync()
    {
        return await this._context.GetStatesCountAsync();
    }

    #endregion


    #region Event CRUD

    public async Task AddEventAsync(int id, int stateId, int userId, string type, int quantity = 0)
    {
        IUser user = await this.GetUserAsync(userId);
        IState state = await this.GetStateAsync(stateId);
        IMovie movie = await this.GetMovieAsync(state.movieId);

        IEvent newEvent = new Event(id, stateId, userId, DateTime.Now, type, quantity);

        switch (type)
        {
            case "PurchaseEvent":
                if (DateTime.Now.Year - user.DateOfBirth.Year < movie.AgeRestriction)
                    throw new Exception("You are not old enough to purchase this movie!");

                if (state.movieQuantity == 0)
                    throw new Exception("Movie unavailable, please check later!");

                if (user.Balance < movie.Price)
                    throw new Exception("Not enough money to purchase this movie!");

                await UpdateStateAsync(stateId, movie.Id, state.movieQuantity - 1);
                await UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance - movie.Price, user.DateOfBirth);
                break;

            case "ReturnEvent":
                Dictionary<int, IEvent> events = await this.GetAllEventsAsync();
                Dictionary<int, IState> states = await this.GetAllStatesAsync();

                int copiesBought = 0;

                foreach (
                        IEvent even in
                        from even in events.Values
                        from stat in states.Values
                        where even.userId == user.Id &&
                              even.stateId == stat.Id &&
                              stat.movieId == movie.Id
                        select even
                    )
                    if (even.Type == "PurchaseEvent") copiesBought++;
                    else if (even.Type == "ReturnEvent") copiesBought--;

                copiesBought--;

                if (copiesBought < 0)
                    throw new Exception("You do not own this movie!");

                await UpdateStateAsync(stateId, movie.Id, state.movieQuantity + 1);
                await UpdateUserAsync(userId, user.Nickname, user.Email, user.Balance + movie.Price, user.DateOfBirth);
                break;

            case "SupplyEvent":
                if (quantity <= 0)
                    throw new Exception("Can not supply with this amount!");

                await UpdateStateAsync(stateId, movie.Id, state.movieQuantity + quantity);
                break;

            default:
                throw new Exception("This event type does not exist!");
        }

        await _context.AddEventAsync(newEvent);
    }

    public async Task<IEvent> GetEventAsync(int id)
    {
        IEvent? even = await this._context.GetEventAsync(id);

        if (even is null)
            throw new Exception("This event does not exist!");

        return even;
    }

    public async Task UpdateEventAsync(int id, int stateId, int userId, DateTime occurenceDate, string type, int? quantity)
    {
        IEvent newEvent = new Event(id, stateId, userId, occurenceDate, type, quantity);

        if (!await this.CheckIfEventExists(newEvent.Id, type))
            throw new Exception("This event does not exist");

        await this._context.UpdateEventAsync(newEvent);
    }

    public async Task DeleteEventAsync(int id)
    {
        if (!await this.CheckIfEventExists(id, "PurchaseEvent"))
            throw new Exception("This event does not exist");

        await this._context.DeleteEventAsync(id);
    }

    public async Task<Dictionary<int, IEvent>> GetAllEventsAsync()
    {
        return await this._context.GetAllEventsAsync();
    }

    public async Task<int> GetEventsCountAsync()
    {
        return await this._context.GetEventsCountAsync();
    }

    #endregion


    #region Utils

    public async Task<bool> CheckIfUserExists(int id)
    {
        return await this._context.CheckIfUserExists(id);
    }

    public async Task<bool> CheckIfMovieExists(int id)
    {
        return await this._context.CheckIfMovieExists(id);
    }

    public async Task<bool> CheckIfStateExists(int id)
    {
        return await this._context.CheckIfStateExists(id);
    }

    public async Task<bool> CheckIfEventExists(int id, string type)
    {
        return await this._context.CheckIfEventExists(id, type);
    }

    //public async Task<IEvent> GetLastMovieEvent(int movieId)

    //}

    //public async Task<Dictionary<int, IEvent>> GetMovieEventHistory(int movieId)
    //{

    //}

    //public async Task<IState> GetMovieState(int movieId)
    //{

    //}

    #endregion
}
