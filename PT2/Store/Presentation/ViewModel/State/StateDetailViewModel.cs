using System.Threading.Tasks;
using System.Windows.Input;
using Presentation.Model.API;

namespace Presentation.ViewModel;

internal class StateDetailViewModel : IViewModel, IStateDetailViewModel
{
    public ICommand UpdateState { get; set; }

    private readonly IStateModelOperation _modelOperation;

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

    public StateDetailViewModel(IStateModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.UpdateState = new OnClickCommand(e => this.Update(), c => this.CanUpdate());

        this._modelOperation = IStateModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();
    }

    public StateDetailViewModel(int id, int movieId, int movieQuantity, IStateModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.Id = id;
        this.MovieId = movieId;
        this.MovieQuantity = movieQuantity;

        this.UpdateState = new OnClickCommand(e => this.Update(), c => this.CanUpdate());

        this._modelOperation = IStateModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();
    }

    private void Update()
    {
        Task.Run(() =>
        {
            this._modelOperation.UpdateAsync(this.Id, this.MovieId, this.MovieQuantity);

            this._informer.InformSuccess("State successfully updated!");
        });
    }

    private bool CanUpdate()
    {
        return !(
            string.IsNullOrWhiteSpace(this.MovieQuantity.ToString()) ||
            this.MovieQuantity < 0
        );
    }
}
