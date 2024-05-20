using Presentation.Model.API;
using System.Windows.Input;

namespace Presentation.ViewModel;

public interface IStateDetailViewModel
{
    static IStateDetailViewModel CreateViewModel(int id, int productId, int productQuantity,
        IStateModelOperation model, IErrorInformer informer)
    {
        return new StateDetailViewModel(id, productId, productQuantity, model, informer);
    }

    ICommand UpdateState { get; set; }

    int Id { get; set; }

    int ProductId { get; set; }

    int ProductQuantity { get; set; }
}
