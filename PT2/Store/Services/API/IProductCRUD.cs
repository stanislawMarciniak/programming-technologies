using Data.API;
using Service.Implementation;

namespace Service.API;

public interface IMovieCRUD
{
    static IMovieCRUD CreateMovieCRUD(IDataRepository? dataRepository = null)
    {
        return new MovieCRUD(dataRepository ?? IDataRepository.CreateDatabase());
    }

    Task AddMovieAsync(int id, string name, double price, int ageRestriction);

    Task<IMovieDTO> GetMovieAsync(int id);

    Task UpdateMovieAsync(int id, string name, double price, int ageRestriction);

    Task DeleteMovieAsync(int id);

    Task<Dictionary<int, IMovieDTO>> GetAllMoviesAsync();

    Task<int> GetMoviesCountAsync();
}
