using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Presentation.Model.API;

namespace Presentation.ViewModel;

internal class UserDetailViewModel : IViewModel, IUserDetailViewModel
{
    public ICommand UpdateUser { get; set; }

    private readonly IUserModelOperation _modelOperation;

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

    private string _nickname;

    public string Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
            OnPropertyChanged(nameof(Nickname));
        }
    }

    private string _email;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    private double _balance;

    public double Balance
    {
        get => _balance;
        set
        {
            _balance = value;
            OnPropertyChanged(nameof(Balance));
        }
    }

    private DateTime _dateOfBirth;

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            _dateOfBirth = value;
            OnPropertyChanged(nameof(DateOfBirth));
        }
    }

    public UserDetailViewModel(IUserModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.UpdateUser = new OnClickCommand(e => this.Update(), c => this.CanUpdate());

        this._modelOperation = model ?? IUserModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();
    }

    public UserDetailViewModel(int id, string nickname, string email, double balance, DateTime dateOfBirth, IUserModelOperation? model = null, IErrorInformer? informer = null)
    {
        this.Id = id;
        this.Nickname = nickname;
        this.Email = email;
        this.Balance = balance;
        this.DateOfBirth = dateOfBirth;

        this.UpdateUser = new OnClickCommand(e => this.Update(), c => this.CanUpdate());

        this._modelOperation = model ?? IUserModelOperation.CreateModelOperation();
        this._informer = informer ?? new PopupErrorInformer();
    }

    private void Update()
    {
        Task.Run(() =>
        {
            this._modelOperation.UpdateAsync(this.Id, this.Nickname, this.Email, this.Balance, this.DateOfBirth);

            this._informer.InformSuccess("User successfully updated!");
        });
    }

    private bool CanUpdate()
    {
        return !(
            string.IsNullOrWhiteSpace(this.Nickname) ||
            string.IsNullOrWhiteSpace(this.Email) ||
            string.IsNullOrWhiteSpace(this.Balance.ToString()) ||
            string.IsNullOrWhiteSpace(this.DateOfBirth.ToString()) ||
            this.Balance == 0
        );
    }
}
