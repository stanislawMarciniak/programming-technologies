using Presentation.Model.API;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModel;

public interface IMovieMasterViewModel
{
    static IMovieMasterViewModel CreateViewModel(IMovieModelOperation operation, IErrorInformer informer)
    {
        return new MovieMasterViewModel(operation, informer);
    }

    ICommand CreateMovie { get; set; }

    ICommand RemoveMovie { get; set; }

    ObservableCollection<IMovieDetailViewModel> Movies { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int AgeRestriction { get; set; }

    bool IsMovieSelected { get; set; }

    Visibility IsMovieDetailVisible { get; set; }

    IMovieDetailViewModel SelectedDetailViewModel { get; set; }
}
