﻿using Data.Implementation;

namespace Data.API;

public interface IDataRepository
{
    static IDataRepository CreateDatabase(IDataContext? dataContext = null)
    {
        return new DataRepository(dataContext ?? new DataContext());
    }

    #region User CRUD

    Task AddUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth);

    Task<IUser> GetUserAsync(int id);

    Task UpdateUserAsync(int id, string nickname, string email, double balance, DateTime dateOfBirth);

    Task DeleteUserAsync(int id);

    Task<Dictionary<int, IUser>> GetAllUsersAsync();

    Task<int> GetUsersCountAsync();

    #endregion User CRUD


    #region Movie CRUD

    Task AddMovieAsync(int id, string name, double price, int ageRestriction);

    Task<IMovie> GetMovieAsync(int id);

    Task UpdateMovieAsync(int id, string name, double price, int ageRestriction);

    Task DeleteMovieAsync(int id);

    Task<Dictionary<int, IMovie>> GetAllMoviesAsync();

    Task<int> GetMoviesCountAsync();

    #endregion


    #region State CRUD

    Task AddStateAsync(int id, int movieId, int movieQuantity);

    Task<IState> GetStateAsync(int id);

    Task UpdateStateAsync(int id, int movieId, int movieQuantity);

    Task DeleteStateAsync(int id);

    Task<Dictionary<int, IState>> GetAllStatesAsync();

    Task<int> GetStatesCountAsync();

    #endregion


    #region Event CRUD

    Task AddEventAsync(int id, int stateId, int userId, string type, int quantity = 0);

    Task<IEvent> GetEventAsync(int id);

    Task UpdateEventAsync(int id, int stateId, int userId, DateTime occurenceDate, string type, int? quantity);

    Task DeleteEventAsync(int id);

    Task<Dictionary<int, IEvent>> GetAllEventsAsync();

    Task<int> GetEventsCountAsync();

    #endregion
}
