﻿using Data.API;
using Data.Database;

namespace Data.Implementation;

internal class DataContext : IDataContext
{
    public DataContext(string? connectionString = null)
    {
        if (connectionString is null)
        {
            string _projectRootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string _DBRelativePath = @"Data\Database\Shop.mdf";
            string _DBPath = Path.Combine(_projectRootDir, _DBRelativePath);
            this.ConnectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={_DBPath};Integrated Security = True; Connect Timeout = 30;";
        }
        else
        {
            this.ConnectionString = connectionString;
        }
    }

    private readonly string ConnectionString;

    #region User CRUD

    public async Task AddUserAsync(IUser user)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.User entity = new Database.User()
            {
                id = user.Id,
                nickname = user.Nickname,
                email = user.Email,
                balance = (decimal)user.Balance,
                dateOfBirth = user.DateOfBirth,
            };

            context.Users.InsertOnSubmit(entity);

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task<IUser?> GetUserAsync(int id)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.User? user = await Task.Run(() =>
            {
                
                IQueryable<Database.User> query = context.Users.Where(u => u.id == id);

                return query.FirstOrDefault();
            });
        
            return user is not null ? new User(user.id, user.nickname, user.email, (double)user.balance, user.dateOfBirth) : null;
        }
    }

    public async Task UpdateUserAsync(IUser user)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.User toUpdate = (from u in context.Users where u.id == user.Id select u).FirstOrDefault()!;

            toUpdate.nickname = user.Nickname;
            toUpdate.email = user.Email;
            toUpdate.balance = (decimal)user.Balance;
            toUpdate.dateOfBirth = user.DateOfBirth;

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.User toDelete = (from u in context.Users where u.id == id select u).FirstOrDefault()!;

            context.Users.DeleteOnSubmit(toDelete);

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task<Dictionary<int, IUser>> GetAllUsersAsync()
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            IQueryable<IUser> usersQuery = from u in context.Users
                select
                    new User(u.id, u.nickname, u.email, (double)u.balance, u.dateOfBirth) as IUser;

            return await Task.Run(() => usersQuery.ToDictionary(k => k.Id));
        }
    }

    public async Task<int> GetUsersCountAsync()
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            return await Task.Run(() => context.Users.Count());
        }
    }

    #endregion


    #region Movie CRUD

    public async Task AddMovieAsync(IMovie movie)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.Movie entity = new Database.Movie()
            {
                id = movie.Id,
                name = movie.MovieName,
                price = movie.Price,
                ageRestriction = movie.AgeRestriction,
            };

            context.Movies.InsertOnSubmit(entity);

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task<IMovie?> GetMovieAsync(int id)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.Movie? movie = await Task.Run(() =>
            {
                IQueryable<Database.Movie> query =
                    from p in context.Movies
                    where p.id == id
                    select p;

                return query.FirstOrDefault();
            });

            return movie is not null ? new Movie(movie.id, movie.name, (double)movie.price, movie.ageRestriction) : null;
        }
    }

    public async Task UpdateMovieAsync(IMovie movie)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.Movie toUpdate = (from p in context.Movies where p.id == movie.Id select p).FirstOrDefault()!;

            toUpdate.name = movie.MovieName;
            toUpdate.price = movie.Price;
            toUpdate.ageRestriction = movie.AgeRestriction;

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task DeleteMovieAsync(int id)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.Movie toDelete = (from p in context.Movies where p.id == id select p).FirstOrDefault()!;

            context.Movies.DeleteOnSubmit(toDelete);

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task<Dictionary<int, IMovie>> GetAllMoviesAsync()
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            IQueryable<IMovie> movieQuery = from p in context.Movies
                select
                    new Movie(p.id, p.name, (double)p.price, p.ageRestriction) as IMovie;

            return await Task.Run(() => movieQuery.ToDictionary(k => k.Id));
        }
    }

    public async Task<int> GetMoviesCountAsync()
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            return await Task.Run(() => context.Movies.Count());
        }
    }

    #endregion


    #region State CRUD

    public async Task AddStateAsync(IState state)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.State entity = new Database.State()
            {
                id = state.Id,
                movieId = state.movieId,
                movieQuantity = state.movieQuantity
            };

            context.States.InsertOnSubmit(entity);

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task<IState?> GetStateAsync(int id)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.State? state = await Task.Run(() =>
            {
                IQueryable<Database.State> query =
                    from s in context.States
                    where s.id == id
                    select s;

                return query.FirstOrDefault();
            });

            return state is not null ? new State(state.id, state.movieId, state.movieQuantity) : null;
        }
    }

    public async Task UpdateStateAsync(IState state)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.State toUpdate = (from s in context.States where s.id == state.Id select s).FirstOrDefault()!;

            toUpdate.movieId = state.movieId;
            toUpdate.movieQuantity = state.movieQuantity;

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task DeleteStateAsync(int id)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.State toDelete = (from s in context.States where s.id == id select s).FirstOrDefault()!;

            context.States.DeleteOnSubmit(toDelete);

            await Task.Run(() => context.SubmitChanges());
        }
    }

    public async Task<Dictionary<int, IState>> GetAllStatesAsync()
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            IQueryable<IState> stateQuery = from s in context.States
                select
                    new State(s.id, s.movieId, s.movieQuantity) as IState;

            return await Task.Run(() => stateQuery.ToDictionary(k => k.Id));
        }
    }

    public async Task<int> GetStatesCountAsync()
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            return await Task.Run(() => context.States.Count());
        }
    }

    #endregion


    #region Event CRUD

    public async Task AddEventAsync(IEvent even)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.Event entity = new Database.Event()
            {
                id = even.Id,
                stateId = even.stateId,
                userId = even.userId,
                occurrenceDate = even.occurrenceDate,
                type = even.Type,
                quantity = even.Quantity
            };

            context.Events.InsertOnSubmit(entity);

            await Task.Run(() => context.SubmitChanges());
        }
    }    

    public async Task<IEvent?> GetEventAsync(int id)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.Event? even = await Task.Run(() =>
            {
                IQueryable<Database.Event> query =
                    from e in context.Events
                    where e.id == id
                    select e;

                return query.FirstOrDefault();
            });

            return even is not null ? new Event(even.id, even.stateId, even.userId, even.occurrenceDate, even.type, even.quantity) : null;
        }
        
    }    

    public async Task UpdateEventAsync(IEvent even)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.Event toUpdate = (from e in context.Events where e.id == even.Id select e).FirstOrDefault()!;

            toUpdate.stateId = even.stateId;
            toUpdate.userId = even.userId;
            toUpdate.occurrenceDate = even.occurrenceDate;
            toUpdate.type = even.Type;
            toUpdate.quantity = even.Quantity;

            await Task.Run(() => context.SubmitChanges());
        }
    }    

    public async Task DeleteEventAsync(int id)
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            Database.Event toDelete = (from e in context.Events where e.id == id select e).FirstOrDefault()!;

            context.Events.DeleteOnSubmit(toDelete);

            await Task.Run(() => context.SubmitChanges());
        }
    }    

    public async Task<Dictionary<int, IEvent>> GetAllEventsAsync()
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            IQueryable<IEvent> eventQuery = from e in context.Events
                select
                    new Event(e.id, e.stateId, e.userId, e.occurrenceDate, e.type, e.quantity) as IEvent;

            return await Task.Run(() => eventQuery.ToDictionary(k => k.Id));
        }
    }    

    public async Task<int> GetEventsCountAsync()
    {
        using (ShopDataContext context = new ShopDataContext(this.ConnectionString))
        {
            return await Task.Run(() => context.Events.Count());
        }
    }    

    #endregion


    #region Utils

    public async Task<bool> CheckIfUserExists(int id)
    {
        return (await this.GetUserAsync(id)) != null;
    }

    public async Task<bool> CheckIfMovieExists(int id)
    {
        return (await this.GetMovieAsync(id)) != null;
    }

    public async Task<bool> CheckIfStateExists(int id)
    {
        return (await this.GetStateAsync(id)) != null;
    }

    public async Task<bool> CheckIfEventExists(int id, string type)
    {
        return (await this.GetEventAsync(id)) != null;
    }

    #endregion
}

