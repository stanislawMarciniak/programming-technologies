using Presentation.Model.API;
using System;
using System.Windows.Input;

namespace Presentation.ViewModel;

public interface IUserDetailViewModel
{
    static IUserDetailViewModel CreateViewModel(int id, string nickname, string email, double balance,
        DateTime dateOfBirth, IUserModelOperation model, IErrorInformer informer)
    {
        return new UserDetailViewModel(id, nickname, email, balance, dateOfBirth, model, informer);
    }

    ICommand UpdateUser { get; set; }

    int Id { get; set; }

    string Nickname { get; set; }

    string Email { get; set; }

    double Balance { get; set; }

    DateTime DateOfBirth { get; set; }
}

