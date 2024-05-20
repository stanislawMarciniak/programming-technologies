using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Presentation.Model.API;

namespace Presentation.ViewModel;

internal class EventMasterViewModel : IViewModel, IEventMasterViewModel
{
    public ICommand SwitchToUserMasterPage { get; set; }

    public ICommand SwitchToProductMasterPage { get; set; }

    public ICommand SwitchToStateMasterPage { get; set; }

    public ICommand PurchaseEvent { get; set; }

    public ICommand ReturnEvent { get; set; }

    public ICommand SupplyEvent { get; set; }

    public ICommand RemoveEvent { get; set; }

    private readonly IEventModelOperation _modelOperation;

    private readonly IErrorInformer _informer;

    private ObservableCollection<IEventDetailViewModel> _events;

    public ObservableCollection<IEventDetailViewModel> Events
    {
        get => _events;
        set
        {
            _events = value;
            OnPropertyChanged(nameof(Events));
        }
    }

    private int _stateId;

    public int StateId
    {
        get => _stateId;
        set
        {
            _stateId = value;
            OnPropertyChanged(nameof(StateId));
        }
    }

    private int _userId;

    public int UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            OnPropertyChanged(nameof(UserId));
        }
    }

    private int _quantity;

    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            OnPropertyChanged(nameof(Quantity));
        }
    }

    private bool _isEventSelected;

    public bool IsEventSelected
    {
        get => _isEventSelected;
        set
        {
            this.IsEventDetailVisible = value ? Visibility.Visible : Visibility.Hidden;

            _isEventSelected = value;
            OnPropertyChanged(nameof(IsEventSelected));
        }
    }

    private Visibility _isEventDetailVisible;

    public Visibility IsEventDetailVisible
    {
        get => _isEventDetailVisible;
        set
        {
            _isEventDetailVisible = value;
            OnPropertyChanged(nameof(IsEventDetailVisible));
        }
    }

    private IEventDetailViewModel _selectedDetailViewModel;

    public IEventDetailViewModel SelectedDetailViewModel
    {
        get => _selectedDetailViewModel;
        set
        {
            _selectedDetailViewModel = value;
            this.IsEventSelected = true;

            OnPropertyChanged(nameof(SelectedDetailViewModel));
        }
    }

    public EventMasterViewModel(IEventModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.SwitchToUserMasterPage = new SwitchViewCommand("UserMasterView");
        this.SwitchToStateMasterPage = new SwitchViewCommand("StateMasterView");
        this.SwitchToProductMasterPage = new SwitchViewCommand("ProductMasterView");

        this.PurchaseEvent = new OnClickCommand(e => this.StorePurchaseEvent(), c => this.CanPurchaseEvent());
        this.ReturnEvent = new OnClickCommand(e => this.StoreReturnEvent(), c => this.CanReturnEvent());
        this.SupplyEvent = new OnClickCommand(e => this.StoreSupplyEvent(), c => this.CanSupplyEvent());
        this.RemoveEvent = new OnClickCommand(e => this.DeleteEvent());

        this.Events = new ObservableCollection<IEventDetailViewModel>();

        this._modelOperation = IEventModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();

        this.IsEventSelected = false;

        Task.Run(this.LoadEvents);
    }

    private bool CanPurchaseEvent()
    {
        return !(
            string.IsNullOrWhiteSpace(this.StateId.ToString()) ||
            string.IsNullOrWhiteSpace(this.UserId.ToString())
        );
    }

    private bool CanReturnEvent()
    {
        return !(
            string.IsNullOrWhiteSpace(this.StateId.ToString()) ||
            string.IsNullOrWhiteSpace(this.UserId.ToString())
        );
    }

    private bool CanSupplyEvent()
    {
        return !(
            string.IsNullOrWhiteSpace(this.StateId.ToString()) ||
            string.IsNullOrWhiteSpace(this.UserId.ToString()) ||
            string.IsNullOrEmpty(this.Quantity.ToString()) ||
            this.Quantity < 1
        );
    }

    private void StorePurchaseEvent()
    {
        Task.Run(async () =>
        {
            try
            {
                int lastId = await this._modelOperation.GetCountAsync() + 1;

                await this._modelOperation.AddAsync(lastId, this.StateId, this.UserId, "PurchaseEvent");

                this.LoadEvents();

                this._informer.InformSuccess("Event successfully created!");
            }
            catch (Exception e)
            {
                this._informer.InformError(e.Message);
            }
        });
    }

    private void StoreReturnEvent()
    {
        Task.Run(async () =>
        {
            int lastId = await this._modelOperation.GetCountAsync() + 1;

            await this._modelOperation.AddAsync(lastId, this.StateId, this.UserId, "ReturnEvent");

            this.LoadEvents();

            this._informer.InformSuccess("Event successfully created!");
        });
    }

    private void StoreSupplyEvent()
    {
        Task.Run(async () =>
        {
            int lastId = await this._modelOperation.GetCountAsync() + 1;

            await this._modelOperation.AddAsync(lastId, this.StateId, this.UserId, "SupplyEvent", this.Quantity);

            this.LoadEvents();

            this._informer.InformSuccess("Event successfully created!");
        });
    }

    private void DeleteEvent()
    {
        Task.Run(async () =>
        {
            await this._modelOperation.DeleteAsync(this.SelectedDetailViewModel.Id);

            this.LoadEvents();

            this._informer.InformSuccess("Event successfully deleted!");
        });
    }

    private async void LoadEvents()
    {
        Dictionary<int, IEventModel> Events = (await this._modelOperation.GetAllAsync());

        Application.Current.Dispatcher.Invoke(() =>
        {
            this._events.Clear();

            foreach (IEventModel e in Events.Values)
            {
                this._events.Add(new EventDetailViewModel(e.Id, e.stateId, e.userId, e.occurrenceDate, e.Type, e.Quantity));
            }
        });

        OnPropertyChanged(nameof(Events));
    }
}
