using System.Threading.Tasks;
using System.Windows.Input;
using Presentation.Model.API;

namespace Presentation.ViewModel;

internal class MovieDetailViewModel : IViewModel, IMovieDetailViewModel
{
    public ICommand UpdateMovie { get; set; }

    private readonly IMovieModelOperation _modelOperation;

    private readonly IErrorInformer _informer;

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

    public MovieDetailViewModel(IMovieModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.UpdateMovie = new OnClickCommand(e => this.Update(), c => this.CanUpdate());

        this._modelOperation = model ?? IMovieModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();
    }

    public MovieDetailViewModel(int id, string name, double price, int ageRestriction, IMovieModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
        this.AgeRestriction = ageRestriction;

        this.UpdateMovie = new OnClickCommand(e => this.Update(), c => this.CanUpdate());

        this._modelOperation = model ?? IMovieModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();
    }

    private void Update()
    {
        Task.Run(() =>
        {
            this._modelOperation.UpdateAsync(this.Id, this.Name, this.Price, this.AgeRestriction);

            this._informer.InformSuccess("Movie successfully updated!");
        });
    }

    private bool CanUpdate()
    {
        return !(
            string.IsNullOrWhiteSpace(this.Name) ||
            string.IsNullOrWhiteSpace(this.Price.ToString()) ||
            string.IsNullOrWhiteSpace(this.AgeRestriction.ToString()) ||
            this.Price == 0 ||
            this.AgeRestriction == 0
        );
    }
}
