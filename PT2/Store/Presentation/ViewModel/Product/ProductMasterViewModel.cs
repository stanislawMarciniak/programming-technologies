using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Presentation.Model.API;

namespace Presentation.ViewModel;

internal class MovieMasterViewModel : IViewModel, IMovieMasterViewModel
{
    public ICommand SwitchToUserMasterPage { get; set; }

    public ICommand SwitchToStateMasterPage { get; set; }

    public ICommand SwitchToEventMasterPage { get; set; }

    public ICommand CreateMovie { get; set; }

    public ICommand RemoveMovie { get; set; }

    private readonly IMovieModelOperation _modelOperation;

    private readonly IErrorInformer _informer;

    private ObservableCollection<IMovieDetailViewModel> _movies;

    public ObservableCollection<IMovieDetailViewModel> Movies
    {
        get => _movies;
        set
        {
            _movies = value;
            OnPropertyChanged(nameof(Movies));
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

    public int AgeRestriction
    {
        get => _pegi;
        set
        {
            _pegi = value;
            OnPropertyChanged(nameof(AgeRestriction));
        }
    }

    private bool _isMovieSelected;

    public bool IsMovieSelected
    {
        get => _isMovieSelected;
        set
        {
            this.IsMovieDetailVisible = value ? Visibility.Visible : Visibility.Hidden;

            _isMovieSelected = value;
            OnPropertyChanged(nameof(IsMovieSelected));
        }
    }

    private Visibility _isMovieDetailVisible;

    public Visibility IsMovieDetailVisible
    {
        get => _isMovieDetailVisible;
        set
        {
            _isMovieDetailVisible = value;
            OnPropertyChanged(nameof(IsMovieDetailVisible));
        }
    }

    private IMovieDetailViewModel _selectedDetailViewModel;

    public IMovieDetailViewModel SelectedDetailViewModel
    {
        get => _selectedDetailViewModel;
        set
        {
            _selectedDetailViewModel = value;
            this.IsMovieSelected = true;

            OnPropertyChanged(nameof(SelectedDetailViewModel));
        }
    }

    public MovieMasterViewModel(IMovieModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.SwitchToUserMasterPage = new SwitchViewCommand("UserMasterView");
        this.SwitchToStateMasterPage = new SwitchViewCommand("StateMasterView");
        this.SwitchToEventMasterPage = new SwitchViewCommand("EventMasterView");

        this.CreateMovie = new OnClickCommand(e => this.StoreMovie(), c => this.CanStoreMovie());
        this.RemoveMovie = new OnClickCommand(e => this.DeleteMovie());

        this.Movies = new ObservableCollection<IMovieDetailViewModel>();

        this._modelOperation = model ?? IMovieModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();

        this.IsMovieSelected = false;

        Task.Run(this.LoadMovies);
    }

    private bool CanStoreMovie()
    {
        return !(
            string.IsNullOrWhiteSpace(this.Name) ||
            string.IsNullOrWhiteSpace(this.Price.ToString()) ||
            string.IsNullOrEmpty(this.AgeRestriction.ToString()) ||
            this.Price <= 0 ||
            this.AgeRestriction <= 0
        );
    }

    private void StoreMovie()
    {
        Task.Run(async () =>
        {
            int lastId = await this._modelOperation.GetCountAsync() + 1;

            await this._modelOperation.AddAsync(lastId, this.Name, this.Price, this.AgeRestriction);

            this.LoadMovies();

            this._informer.InformSuccess("Movie added successfully!");

        });
    }

    private void DeleteMovie()
    {
        Task.Run(async () =>
        {
            try
            {
                await this._modelOperation.DeleteAsync(this.SelectedDetailViewModel.Id);

                this.LoadMovies();

                this._informer.InformSuccess("Movie deleted successfully!");
            }
            catch (Exception e)
            {
                this._informer.InformError("Error while deleting movie! Remember to remove all associated states!");
            }
        });
    }

    private async void LoadMovies()
    {
        Dictionary<int, IMovieModel> Movies = await this._modelOperation.GetAllAsync();

        Application.Current.Dispatcher.Invoke(() =>
        {
            this._movies.Clear();
            
            foreach (IMovieModel p in Movies.Values)
            {
                this._movies.Add(new MovieDetailViewModel(p.Id, p.Name, p.Price, p.AgeRestriction));
            }
        });

        OnPropertyChanged(nameof(Movies));
    }
}
