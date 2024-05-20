using System.Windows.Input;
using Presentation.Model.API;

namespace Presentation.ViewModel;

public interface IProductDetailViewModel
{
    static IProductDetailViewModel CreateViewModel(int id, string name, double price, int pegi, 
        IProductModelOperation model, IErrorInformer informer)
    {
        return new ProductDetailViewModel(id, name, price, pegi, model, informer);
    }

    ICommand UpdateProduct { get; set; }

    int Id { get; set; }

    string Name { get; set; }

    double Price { get; set; }

    int Pegi { get; set; }
}
