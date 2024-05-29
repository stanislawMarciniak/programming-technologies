using Data.API;
using Service.API;

namespace Service.Implementation;

internal class MovieCRUD : IMovieCRUD
{
    private IDataRepository _repository;

    public MovieCRUD(IDataRepository repository)
    {
        this._repository = repository;
    }

    private IMovieDTO Map(IMovie movie)
    {
        return new MovieDTO(movie.Id, movie.MovieName, movie.Price, movie.AgeRestriction);
    }

    public async Task AddMovieAsync(int id, string name, double price, int ageRestriction)
    {
        await this._repository.AddMovieAsync(id, name, price, ageRestriction);
    }

    public async Task<IMovieDTO> GetMovieAsync(int id)
    {
        return this.Map(await this._repository.GetMovieAsync(id));
    }

    public async Task UpdateMovieAsync(int id, string name, double price, int ageRestriction)
    {
        await this._repository.UpdateMovieAsync(id, name, price, ageRestriction);
    }

    public async Task DeleteMovieAsync(int id)
    {
        await this._repository.DeleteMovieAsync(id);
    }

    public async Task<Dictionary<int, IMovieDTO>> GetAllMoviesAsync()
    {
        Dictionary<int, IMovieDTO> result = new Dictionary<int, IMovieDTO>();

        foreach (IMovie movie in (await this._repository.GetAllMoviesAsync()).Values)
        {
            result.Add(movie.Id, this.Map(movie));
        }

        return result;
    }

    public async Task<int> GetMoviesCountAsync()
    {
        return await this._repository.GetMoviesCountAsync();
    }
}
