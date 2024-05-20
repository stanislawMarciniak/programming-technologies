using Presentation.Model.API;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModel;

public interface IProductMasterViewModel
{
    static IProductMasterViewModel CreateViewModel(IProductModelOperation operation, IErrorInformer informer)
    {
        return new ProductMasterViewModel(operation, informer);
    }

    ICommand CreateProduct { get; set; }

    ICommand RemoveProduct { get; set; }

    ObservableCollection<IProductDetailViewModel> Products { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int Pegi { get; set; }

    bool IsProductSelected { get; set; }

    Visibility IsProductDetailVisible { get; set; }

    IProductDetailViewModel SelectedDetailViewModel { get; set; }
}
