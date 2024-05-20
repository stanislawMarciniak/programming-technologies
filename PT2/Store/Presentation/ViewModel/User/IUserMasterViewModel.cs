using Presentation.Model.API;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Presentation.ViewModel;

public interface IUserMasterViewModel
{
    static IUserMasterViewModel CreateViewModel(IUserModelOperation operation, IErrorInformer informer)
    {
        return new UserMasterViewModel(operation, informer);
    }

    ICommand CreateUser { get; set; }

    ICommand RemoveUser { get; set; }

    ObservableCollection<IUserDetailViewModel> Users { get; set; }

    string Nickname { get; set; }

    string Email { get; set; }

    double Balance { get; set; }

    DateTime DateOfBirth { get; set; }

    bool IsUserSelected { get; set; }

    Visibility IsUserDetailVisible { get; set; }

    IUserDetailViewModel SelectedDetailViewModel { get; set; }
}
