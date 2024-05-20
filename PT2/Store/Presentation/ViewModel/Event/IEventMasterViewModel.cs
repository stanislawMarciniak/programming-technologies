using Presentation.Model.API;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModel;

public interface IEventMasterViewModel
{
    static IEventMasterViewModel CreateViewModel(IEventModelOperation operation, IErrorInformer informer)
    {
        return new EventMasterViewModel(operation, informer);
    }

    ICommand PurchaseEvent { get; set; }

    ICommand ReturnEvent { get; set; }

    ICommand SupplyEvent { get; set; }

    ICommand RemoveEvent { get; set; }

    ObservableCollection<IEventDetailViewModel> Events { get; set; }

    int StateId { get; set; }

    int UserId { get; set; }

    int Quantity { get; set; }

    bool IsEventSelected { get; set; }

    Visibility IsEventDetailVisible { get; set; }

    IEventDetailViewModel SelectedDetailViewModel { get; set; }
}
