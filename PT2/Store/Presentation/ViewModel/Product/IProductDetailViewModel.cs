using System.Windows.Input;
using Presentation.Model.API;

namespace Presentation.ViewModel;

public interface IMovieDetailViewModel
{
    static IMovieDetailViewModel CreateViewModel(int id, string name, double price, int ageRestriction, 
        IMovieModelOperation model, IErrorInformer informer)
    {
        return new MovieDetailViewModel(id, name, price, ageRestriction, model, informer);
    }

    ICommand UpdateMovie { get; set; }

    int Id { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int AgeRestriction { get; set; }
}
