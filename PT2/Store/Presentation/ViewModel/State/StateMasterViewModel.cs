using Presentation.Model.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModel;

internal class StateMasterViewModel : IViewModel, IStateMasterViewModel
{
    public ICommand SwitchToUserMasterPage { get; set; }

    public ICommand SwitchToMovieMasterPage { get; set; }

    public ICommand SwitchToEventMasterPage { get; set; }

    public ICommand CreateState { get; set; }

    public ICommand RemoveState { get; set; }

    private readonly IStateModelOperation _modelOperation;

    private readonly IErrorInformer _informer;

    private ObservableCollection<IStateDetailViewModel> _states;

    public ObservableCollection<IStateDetailViewModel> States
    {
        get => _states;
        set
        {
            _states = value;
            OnPropertyChanged(nameof(States));
        }
    }

    private int _id;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    private int _movieId;

    public int MovieId
    {
        get => _movieId;
        set
        {
            _movieId = value;
            OnPropertyChanged(nameof(MovieId));
        }
    }

    private int _movieQuantity;

    public int MovieQuantity
    {
        get => _movieQuantity;
        set
        {
            _movieQuantity = value;
            OnPropertyChanged(nameof(MovieQuantity));
        }
    }

    private bool _isStateSelected;

    public bool IsStateSelected
    {
        get => _isStateSelected;
        set
        {
            this.IsStateDetailVisible = value ? Visibility.Visible : Visibility.Hidden;

            _isStateSelected = value;
            OnPropertyChanged(nameof(IsStateSelected));
        }
    }

    private Visibility _isStateDetailVisible;

    public Visibility IsStateDetailVisible
    {
        get => _isStateDetailVisible;
        set
        {
            _isStateDetailVisible = value;
            OnPropertyChanged(nameof(IsStateDetailVisible));
        }
    }

    private IStateDetailViewModel _selectedDetailViewModel;

    public IStateDetailViewModel SelectedDetailViewModel
    {
        get => _selectedDetailViewModel;
        set
        {
            _selectedDetailViewModel = value;
            this.IsStateSelected = true;

            OnPropertyChanged(nameof(SelectedDetailViewModel));
        }
    }

    public StateMasterViewModel(IStateModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.SwitchToUserMasterPage = new SwitchViewCommand("UserMasterView");
        this.SwitchToMovieMasterPage = new SwitchViewCommand("MovieMasterView");
        this.SwitchToEventMasterPage = new SwitchViewCommand("EventMasterView");

        this.CreateState = new OnClickCommand(e => this.StoreState(), c => this.CanStoreState());
        this.RemoveState = new OnClickCommand(e => this.DeleteState());

        this.States = new ObservableCollection<IStateDetailViewModel>();

        this._modelOperation = IStateModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();

        this.IsStateSelected = false;

        Task.Run(this.LoadStates);
    }

    private bool CanStoreState()
    {
        return !(
            string.IsNullOrWhiteSpace(this.MovieId.ToString()) ||
            string.IsNullOrWhiteSpace(this.MovieQuantity.ToString()) ||
            this.MovieQuantity < 0
        );
    }

    private void StoreState()
    {
        Task.Run(async () =>
        {
            try
            {
                int lastId = await this._modelOperation.GetCountAsync() + 1;

                await this._modelOperation.AddAsync(lastId, this.MovieId, this.MovieQuantity);

                this.LoadStates();

                this._informer.InformSuccess("State successfully created!");
            }
            catch (Exception e)
            {
                this._informer.InformError(e.Message);
            }
        });
    }

    private void DeleteState()
    {
        Task.Run(async () =>
        {
            try
            {
                await this._modelOperation.DeleteAsync(this.SelectedDetailViewModel.Id);

                this.LoadStates();

                this._informer.InformSuccess("State successfully deleted!");
            }
            catch (Exception e)
            {
                this._informer.InformError("Error while deleting state! Remember to remove all associated events!");
            }
        });
    }

    private async void LoadStates()
    {
        Dictionary<int, IStateModel> States = await this._modelOperation.GetAllAsync();

        Application.Current.Dispatcher.Invoke(() =>
        {
            this._states.Clear();

            foreach (IStateModel s in States.Values)
            {
                this._states.Add(new StateDetailViewModel(s.Id, s.movieId, s.movieQuantity));
            }
        });

        OnPropertyChanged(nameof(States));
    }
}
