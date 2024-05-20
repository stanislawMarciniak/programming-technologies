using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Presentation.Command;
using Presentation.Common;
using Presentation.Model;
using Service;

namespace Presentation.ViewModel
{
    public class ClientListViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region InitialSetup
        public ClientListViewModel()
        {
            Init();
            ConfigureCommands();
        }

        private void Init()
        {
            service = new ClientService();

            clientViewModels = new ObservableCollection<ClientItemViewModel>();


            FetchClients();
        }

        private void ConfigureCommands()
        {
            addCommand = new RelayCommand(e => { AddClient(); },
                condition => CanAdd);

            deleteCommand = new RelayCommand(e => { DeleteClient(); },
                condition => ClientViewModelIsSelected());
        }

        #endregion


        #region API

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;

                ValidateInput(firstName, nameof(FirstName));
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;

                ValidateInput(lastName, nameof(LastName));
                OnPropertyChanged(nameof(LastName));
            }
        }

        public ObservableCollection<ClientItemViewModel> ClientViewModels
        {
            get => clientViewModels;

            set
            {
                clientViewModels = value;
                OnPropertyChanged(nameof(ClientViewModels));
            }
        }

        public ClientItemViewModel SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                IsClientViewModelSelected = true;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public bool IsClientViewModelSelected
        {
            get => isClientViewModelSelected;
            set
            {
                isClientViewModelSelected = value;
                OnPropertyChanged(nameof(IsClientViewModelSelected));
            }
        }

        public ICommand AddCommand
        {
            get => addCommand;
        }

        public ICommand DeleteCommand
        {
            get => deleteCommand;
        }

        public bool CanAdd => !(HasErrors || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName));

        public Action<string> MessageBoxShowDelegate { get; set; }
            = x => throw new ArgumentOutOfRangeException(
                $"The delegate {nameof(MessageBoxShowDelegate)} must be assigned by the view layer");

        #endregion


        #region PrivateAttributes

        private string firstName;
        private string lastName;

        private ICommand addCommand;
        private ICommand deleteCommand;

        private ClientService service;
        private ClientItemViewModel selectedViewModel;
        private ObservableCollection<ClientItemViewModel> clientViewModels;

        private bool isClientViewModelSelected;

        #endregion


        #region PrivateMethods

        private void AddClient()
        {
            ClientModel newClient = new ClientModel()
            {
                _id = 0,
                _firstName = FirstName,
                _lastName = LastName
            };

            service.AddClient(newClient);
            FetchClients();
        }

        private void DeleteClient()
        {
            string message = service.DeleteClient(SelectedViewModel.Id) 
                ? "Client was deleted!" : "Can't delete a client, since he has registered events";

            ShowPopupWindow(message);
            FetchClients();
            OnPropertyChanged(nameof(ClientViewModels));
        }

        private bool ClientViewModelIsSelected()
        {
            return !(selectedViewModel is null);
        }

        private void ShowPopupWindow(string message)
        {
            MessageBoxShowDelegate(message);
        }

        private void FetchClients()
        {
            clientViewModels.Clear();

            foreach (var c in service.GetAllClients())
            {
                clientViewModels.Add(new ClientItemViewModel(c));
            }

            OnPropertyChanged(nameof(ClientViewModels));
        }

        private void ValidateInput(string field, string propertyName)
        {
            errorValidator.ClearErrors(propertyName);

            if (string.IsNullOrWhiteSpace(field))
            {
                errorValidator.AddError(propertyName, $"{propertyName} cannot be empty!");
            }
            else if (field.Length > 20)
            {
                errorValidator.AddError(propertyName, $"Maximum length of {propertyName} is 20!");
            }
        }

        #endregion


        #region Validation

        private ErrorValidator errorValidator = new ErrorValidator();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanAdd));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return errorValidator.GetErrors(propertyName);
        }

        public bool HasErrors => errorValidator.HasErrors;

        #endregion
    }
}
