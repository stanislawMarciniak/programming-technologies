using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Presentation.Model.API;

namespace Presentation.ViewModel;

internal class ProductMasterViewModel : IViewModel, IProductMasterViewModel
{
    public ICommand SwitchToUserMasterPage { get; set; }

    public ICommand SwitchToStateMasterPage { get; set; }

    public ICommand SwitchToEventMasterPage { get; set; }

    public ICommand CreateProduct { get; set; }

    public ICommand RemoveProduct { get; set; }

    private readonly IProductModelOperation _modelOperation;

    private readonly IErrorInformer _informer;

    private ObservableCollection<IProductDetailViewModel> _products;

    public ObservableCollection<IProductDetailViewModel> Products
    {
        get => _products;
        set
        {
            _products = value;
            OnPropertyChanged(nameof(Products));
        }
    }

    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private double _price;

    public double Price
    {
        get => _price;
        set
        {
            _price = value;
            OnPropertyChanged(nameof(Price));
        }
    }

    private int _pegi;

    public int Pegi
    {
        get => _pegi;
        set
        {
            _pegi = value;
            OnPropertyChanged(nameof(Pegi));
        }
    }

    private bool _isProductSelected;

    public bool IsProductSelected
    {
        get => _isProductSelected;
        set
        {
            this.IsProductDetailVisible = value ? Visibility.Visible : Visibility.Hidden;

            _isProductSelected = value;
            OnPropertyChanged(nameof(IsProductSelected));
        }
    }

    private Visibility _isProductDetailVisible;

    public Visibility IsProductDetailVisible
    {
        get => _isProductDetailVisible;
        set
        {
            _isProductDetailVisible = value;
            OnPropertyChanged(nameof(IsProductDetailVisible));
        }
    }

    private IProductDetailViewModel _selectedDetailViewModel;

    public IProductDetailViewModel SelectedDetailViewModel
    {
        get => _selectedDetailViewModel;
        set
        {
            _selectedDetailViewModel = value;
            this.IsProductSelected = true;

            OnPropertyChanged(nameof(SelectedDetailViewModel));
        }
    }

    public ProductMasterViewModel(IProductModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.SwitchToUserMasterPage = new SwitchViewCommand("UserMasterView");
        this.SwitchToStateMasterPage = new SwitchViewCommand("StateMasterView");
        this.SwitchToEventMasterPage = new SwitchViewCommand("EventMasterView");

        this.CreateProduct = new OnClickCommand(e => this.StoreProduct(), c => this.CanStoreProduct());
        this.RemoveProduct = new OnClickCommand(e => this.DeleteProduct());

        this.Products = new ObservableCollection<IProductDetailViewModel>();

        this._modelOperation = model ?? IProductModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();

        this.IsProductSelected = false;

        Task.Run(this.LoadProducts);
    }

    private bool CanStoreProduct()
    {
        return !(
            string.IsNullOrWhiteSpace(this.Name) ||
            string.IsNullOrWhiteSpace(this.Price.ToString()) ||
            string.IsNullOrEmpty(this.Pegi.ToString()) ||
            this.Price <= 0 ||
            this.Pegi <= 0
        );
    }

    private void StoreProduct()
    {
        Task.Run(async () =>
        {
            int lastId = await this._modelOperation.GetCountAsync() + 1;

            await this._modelOperation.AddAsync(lastId, this.Name, this.Price, this.Pegi);

            this.LoadProducts();

            this._informer.InformSuccess("Product added successfully!");

        });
    }

    private void DeleteProduct()
    {
        Task.Run(async () =>
        {
            try
            {
                await this._modelOperation.DeleteAsync(this.SelectedDetailViewModel.Id);

                this.LoadProducts();

                this._informer.InformSuccess("Product deleted successfully!");
            }
            catch (Exception e)
            {
                this._informer.InformError("Error while deleting product! Remember to remove all associated states!");
            }
        });
    }

    private async void LoadProducts()
    {
        Dictionary<int, IProductModel> Products = await this._modelOperation.GetAllAsync();

        Application.Current.Dispatcher.Invoke(() =>
        {
            this._products.Clear();
            
            foreach (IProductModel p in Products.Values)
            {
                this._products.Add(new ProductDetailViewModel(p.Id, p.Name, p.Price, p.Pegi));
            }
        });

        OnPropertyChanged(nameof(Products));
    }
}
