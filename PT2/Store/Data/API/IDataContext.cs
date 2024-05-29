using Data.Implementation;

namespace Data.API;

public interface IDataContext
{
    static IDataContext CreateContext(string? connectionString = null)
    {
        return new DataContext(connectionString);
    }

    #region User CRUD

    Task AddUserAsync(IUser user);

    Task<IUser?> GetUserAsync(int id);

    Task UpdateUserAsync(IUser user);

    Task DeleteUserAsync(int id);

    Task<Dictionary<int, IUser>> GetAllUsersAsync();

    Task<int> GetUsersCountAsync();

    #endregion User CRUD


    #region Movie CRUD

    Task AddMovieAsync(IMovie movie);

    Task<IMovie?> GetMovieAsync(int id);

    Task UpdateMovieAsync(IMovie movie);

    Task DeleteMovieAsync(int id);

    Task<Dictionary<int, IMovie>> GetAllMoviesAsync();

    Task<int> GetMoviesCountAsync();

    #endregion


    #region State CRUD

    Task AddStateAsync(IState state);

    Task<IState?> GetStateAsync(int id);

    Task UpdateStateAsync(IState state);

    Task DeleteStateAsync(int id);

    Task<Dictionary<int, IState>> GetAllStatesAsync();

    Task<int> GetStatesCountAsync();

    #endregion


    #region Event CRUD

    Task AddEventAsync(IEvent even);

    Task<IEvent?> GetEventAsync(int id);

    Task UpdateEventAsync(IEvent even);

    Task DeleteEventAsync(int id);

    Task<Dictionary<int, IEvent>> GetAllEventsAsync();

    Task<int> GetEventsCountAsync();

    #endregion


    #region Utils

    Task<bool> CheckIfUserExists(int id);

    Task<bool> CheckIfMovieExists(int id);

    Task<bool> CheckIfStateExists(int id);

    Task<bool> CheckIfEventExists(int id, string type);

    #endregion
}
