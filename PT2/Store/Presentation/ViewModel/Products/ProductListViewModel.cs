using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Presentation.Model;
using Service;
using Presentation.Command;
using Presentation.Common;
using System.ComponentModel;
using System.Collections;

namespace Presentation.ViewModel
{
    public class ItemListViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region InitialSetup
        public ItemListViewModel()
        {
            Init();
            ConfigureCommands();
        }

        private void Init()
        {
            service = new ItemService();

            ItemViewModels = new ObservableCollection<ItemItemViewModel>();

            FetchItems();
        }

        private void ConfigureCommands()
        {
            addCommand = new RelayCommand(e => { AddItem(); },
                c => NonEmptyInputs());

            deleteCommand = new RelayCommand(e => { DeleteItem(); },
                c => ItemViewModelIsSelected());
        }

        #endregion


        #region API

        public string ItemName
        {
            get => newItemName;
            set
            {
                newItemName = value;
                ValidateStringInput(newItemName, nameof(ItemName));
                OnPropertyChanged(nameof(ItemName));
            }
        }

        public double Price
        {
            get => newItemPrice;
            set
            {
                newItemPrice = value;
                ValidatePriceInput(newItemPrice, nameof(Price));
                OnPropertyChanged(nameof(Price));
            }
        }

        public string Category
        {
            get => newItemCategory;
            set
            {
                newItemCategory = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public ObservableCollection<ItemItemViewModel> ItemViewModels
        {
            get => ItemViewModels;

            set
            {
                ItemViewModels = value;
                OnPropertyChanged(nameof(ItemViewModels));
            }
        }

        public ItemItemViewModel SelectedViewModel
        {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
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

        public bool CanAdd => !HasErrors;


        public Action<string> MessageBoxShowDelegate { get; set; }
            = x => throw new ArgumentOutOfRangeException(
                $"The delegate {nameof(MessageBoxShowDelegate)} must be assigned by the view layer");

        #endregion


        #region PrivateAttributes

        private string newItemName;
        private double newItemPrice;
        private string newItemCategory;

        private ICommand addCommand;
        private ICommand deleteCommand;

        private ItemService service;
        private ItemItemViewModel selectedViewModel;
        private ObservableCollection<ItemItemViewModel> ItemViewModels;

        #endregion


        #region PrivateMethods

        private void FetchItems()
        {
            ItemViewModels.Clear();

            foreach (var c in service.GetAllItems())
            {
                ItemViewModels.Add(new ItemItemViewModel(c));
            }

            OnPropertyChanged(nameof(ItemViewModels));
        }

        private void AddItem()
        {
            ItemModel newItem = new ItemModel()
            {
                _id = 0,
                _ItemName = ItemName,
                _price = Price,
                _category = Category
            };

            service.AddItem(newItem);
            FetchItems();
        }

        private void DeleteItem()
        {
            if (service.DeleteItem(SelectedViewModel.Id))
            {
                ShowPopupWindow("Successfully deleted a Item");
                FetchItems();
            }
            else
            {
                ShowPopupWindow("Cannot delete a Item, since it is included in some purchase events in the system");
            }
        }

        private bool ItemHasNoPurchases()
        {
            return service.HasNoPurchases(SelectedViewModel.Id);
        }

        public bool ItemViewModelIsSelected()
        {
            return !(selectedViewModel is null);
        }

        private bool NonEmptyInputs()
        {
            return !string.IsNullOrEmpty(ItemName) && Price > 0 && !string.IsNullOrEmpty(Category);
        }

        private void ShowPopupWindow(string message)
        {
            MessageBoxShowDelegate(message);
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
