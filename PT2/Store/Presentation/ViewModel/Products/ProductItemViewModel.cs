using System;
using Service;
using System.Windows.Input;
using Presentation.Command;
using Presentation.Model;
using System.Collections;
using Presentation.Common;
using System.ComponentModel;

namespace Presentation.ViewModel
{
    public class ItemItemViewModel : ViewModelBase, INotifyDataErrorInfo
    {

        #region InitialSetup

        public ItemItemViewModel() { }

        public ItemItemViewModel(ItemModel Item)
        {
            Id = Item._id;
            ItemName = Item._ItemName;
            Price = Item._price;
            Category = Item._category;

            service = new ItemService();

            ConfigureCommands();
        }

        private void ConfigureCommands()
        {
            updateCommand = new RelayCommand(e => { UpdateItem();}, c => CanUpdate);
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

        public string ItemName
        {
            get => ItemName;
            set
            {
                ItemName = value;
                ValidateStringInput(ItemName, nameof(ItemName));
                OnPropertyChanged(nameof(ItemName));
            }
        }
        public double Price
        {
            get => price;
            set
            {
                price = value;
                ValidatePriceInput(price, nameof(Price));
                OnPropertyChanged(nameof(Price));
            }
        }

        public string Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged(nameof(category));
            }
        }

        public ICommand UpdateCommand
        {
            get => updateCommand;
        }

        public bool CanUpdate => !HasErrors;

        #endregion


        #region PrivateAttributes

        private int id;
        private string ItemName;
        private double price;
        private string category;

        private ItemService service;
        private ErrorValidator errorValidator = new ErrorValidator();
        private ICommand updateCommand;

        #endregion


        #region PrivateMethods

        private void UpdateItem()
        {
            service.UpdateSelectedItem(
                new ItemModel()
                {
                    _id = Id,
                    _ItemName = ItemName,
                    _price = Price,
                    _category = Category
                });
        }


        private void ValidateStringInput(string field, string propertyName)
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

        private void ValidatePriceInput(double field, string propertyName)
        {
            errorValidator.ClearErrors(propertyName);

            if (field <= 0)
            {
                errorValidator.AddError(propertyName, $"{propertyName} has to be more than zero. It's a shop, not a charity!");
            }
            else if (field.ToString().Length > 10)
            {
                errorValidator.AddError(propertyName, $"Maximum length of {propertyName} is 10!");
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

