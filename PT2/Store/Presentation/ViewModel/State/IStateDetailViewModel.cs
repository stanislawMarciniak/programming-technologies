using Presentation.Model.API;
using System.Windows.Input;

namespace Presentation.ViewModel;

public interface IStateDetailViewModel
{
    static IStateDetailViewModel CreateViewModel(int id, int movieId, int movieQuantity,
        IStateModelOperation model, IErrorInformer informer)
    {
        return new StateDetailViewModel(id, movieId, movieQuantity, model, informer);
    }

    ICommand UpdateState { get; set; }

    int Id { get; set; }

    int MovieId { get; set; }

    int MovieQuantity { get; set; }
}
