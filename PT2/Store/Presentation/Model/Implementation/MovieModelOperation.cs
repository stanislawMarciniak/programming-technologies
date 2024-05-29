using Presentation.Model.API;
using Service.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.Model.Implementation;

internal class MovieModelOperation : IMovieModelOperation
{
    private IMovieCRUD _movieCRUD;

    public MovieModelOperation(IMovieCRUD? movieCrud = null)
    {
        this._movieCRUD = movieCrud ?? IMovieCRUD.CreateMovieCRUD();
    }

    private IMovieModel Map(IMovieDTO movie)
    {
        return new MovieModel(movie.Id, movie.Name, movie.Price, movie.AgeRestriction);
    }

    public async Task AddAsync(int id, string name, double price, int ageRestriction)
    {
        await this._movieCRUD.AddMovieAsync(id, name, price, ageRestriction);
    }

    public async Task<IMovieModel> GetAsync(int id)
    {
        return this.Map(await this._movieCRUD.GetMovieAsync(id));
    }

    public async Task UpdateAsync(int id, string name, double price, int ageRestriction)
    {
        await this._movieCRUD.UpdateMovieAsync(id, name, price, ageRestriction);
    }

    public async Task DeleteAsync(int id)
    {
        await this._movieCRUD.DeleteMovieAsync(id);
    }

    public async Task<Dictionary<int, IMovieModel>> GetAllAsync()
    {
        Dictionary<int, IMovieModel> result = new Dictionary<int, IMovieModel>();

        foreach (IMovieDTO movie in (await this._movieCRUD.GetAllMoviesAsync()).Values)
        {
            result.Add(movie.Id, this.Map(movie));
        }

        return result;
    }

    public async Task<int> GetCountAsync()
    {
        return await this._movieCRUD.GetMoviesCountAsync();
    }
}
