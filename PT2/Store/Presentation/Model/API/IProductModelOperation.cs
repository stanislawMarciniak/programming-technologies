using System.Collections.Generic;
using System.Threading.Tasks;
using Presentation.Model.Implementation;
using Service.API;

namespace Presentation.Model.API;

public interface IMovieModelOperation
{
    static IMovieModelOperation CreateModelOperation(IMovieCRUD? movieCrud = null)
    {
        return new MovieModelOperation(movieCrud ?? IMovieCRUD.CreateMovieCRUD());
    }

    Task AddAsync(int id, string name, double price, int ageRestriction);

    Task<IMovieModel> GetAsync(int id);

    Task UpdateAsync(int id, string name, double price, int ageRestriction);

    Task DeleteAsync(int id);

    Task<Dictionary<int, IMovieModel>> GetAllAsync();

    Task<int> GetCountAsync();
}
