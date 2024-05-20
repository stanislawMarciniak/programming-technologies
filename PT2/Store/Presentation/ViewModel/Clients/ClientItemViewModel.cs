using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using Presentation.Command;
using Presentation.Common;
using Presentation.Model;
using Service;

namespace Presentation.ViewModel
{
    public class ClientItemViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region InitialSetup

        public ClientItemViewModel() { }

        public ClientItemViewModel(ClientModel clientModel)
        {
            id = clientModel._id; 
            firstName = clientModel._firstName;
            lastName = clientModel._lastName;

            service = new ClientService();

            ConfigureCommands();
        }

        private void ConfigureCommands()
        {
            updateCommand = new RelayCommand(e => {UpdateClient();}, c => CanUpdate);
        }

        #endregion


        #region API

        public int Id
        {
            get => id;

            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

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

        public ICommand UpdateCommand
        {
            get => updateCommand;
        }

        public bool CanUpdate => !(HasErrors || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName));

        #endregion


        #region PrivateAttributes

        private string firstName;
        private string lastName;
        private int id;

        private ClientService service;

        private ErrorValidator errorValidator = new ErrorValidator();

        private ICommand updateCommand;

        #endregion


        #region PrivateMethods


        private void UpdateClient()
        {
            service.UpdateClient(
                new ClientModel()
                {
                    _id = Id,
                    _firstName = FirstName,
                    _lastName = LastName
                });
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

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanUpdate));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return errorValidator.GetErrors(propertyName);
        }

        public bool HasErrors => errorValidator.HasErrors;
    }

    #endregion
}
