using System;
using System.Windows.Input;
using Presentation.Model;
using Service;
using Presentation.Command;
using Presentation.Common;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using Data;

namespace Presentation.ViewModel
{
    public class EventListViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        #region InitialSetup
        public EventListViewModel()
        {
            Init();
            ConfigureCommands();
        }

        private void Init()
        {
            purchaseService = new EventPurchaseService();
            returnService = new EventReturnService();

            purchaseViewModels = new ObservableCollection<PurchaseViewModel>();
            returnViewModels = new ObservableCollection<ReturnViewModel>();

            FetchPurchases();
            FetchReturns();
        }

        private void ConfigureCommands()
        {
            purchaseCommand = new RelayCommand(e => { AddPurchaseEvent(); },
                c => ProperInputs());

            returnCommand = new RelayCommand(e => { AddReturnEvent(); },
                c => ProperInputs());
        }

        #endregion


        #region API


        public int EventId
        {
            get => selectedEventId;
            set
            {
                selectedEventId = value;
                ValidateItemIdInput(selectedEventId, nameof(EventId));
                OnPropertyChanged(nameof(EventId));
            }
        }


        public int ItemId
        {
            get => selectedItemId;
            set
            {
                selectedItemId = value;
                ValidateItemIdInput(selectedItemId, nameof(ItemId));
                OnPropertyChanged(nameof(ItemId));
            }
        }

        public int ClientId
        {
            get => selectedClientId;
            set
            {
                selectedClientId = value;
                ValidateClientIdInput(selectedClientId, nameof(ClientId));
                OnPropertyChanged(nameof(ClientId));
            }
        }

        public ObservableCollection<PurchaseViewModel> PurchaseViewModels
        {
            get => purchaseViewModels;

            set
            {
                purchaseViewModels = value;
                OnPropertyChanged(nameof(PurchaseViewModels));
            }
        }

        public ObservableCollection<ReturnViewModel> ReturnViewModels
        {
            get => returnViewModels;
            set
            {
                returnViewModels = value;
                OnPropertyChanged(nameof(ReturnViewModels));
            }
        }

        public PurchaseViewModel SelectedPurchaseViewModel
        {
            get => selectedPurchaseViewModel;
            set
            {
                selectedPurchaseViewModel = value;
                OnPropertyChanged(nameof(SelectedPurchaseViewModel));
            }
        }

        public ReturnViewModel SelectedReturnViewModel
        {
            get => selectedReturnViewModel;
            set
            {
                selectedReturnViewModel = value;
                OnPropertyChanged(nameof(SelectedReturnViewModel));
            }
        }

        public ICommand PurchaseCommand
        {
            get => purchaseCommand;
        }

        public ICommand ReturnCommand
        {
            get => returnCommand;
        }

        public bool CanAddEvent => !HasErrors;


        public Action<string> MessageBoxShowDelegate { get; set; }
            = x => throw new ArgumentOutOfRangeException(
                $"The delegate {nameof(MessageBoxShowDelegate)} must be assigned by the view layer");

        #endregion


        #region PrivateAttributes
        private int selectedEventId;
        private int selectedClientId;
        private int selectedItemId;

        private ICommand purchaseCommand;
        private ICommand returnCommand;

        private EventPurchaseService purchaseService;
        private EventReturnService returnService;
        private PurchaseViewModel selectedPurchaseViewModel;
        private ReturnViewModel selectedReturnViewModel;
        private ObservableCollection<PurchaseViewModel> purchaseViewModels;
        private ObservableCollection<ReturnViewModel> returnViewModels;


        #endregion


        #region PrivateMethods

        private void FetchPurchases()
        {
            purchaseViewModels.Clear();

            foreach (var c in purchaseService.GetAllPurchases())
            {
                purchaseViewModels.Add(new PurchaseViewModel(c));
            }

            OnPropertyChanged("PurchaseViewModels");
        }

        private void FetchReturns()
        {
            returnViewModels.Clear();

            foreach (var c in returnService.GetAllReturns())
            {
                returnViewModels.Add(new ReturnViewModel(c));
            }

            OnPropertyChanged("ReturnViewModels");
        }

        private void AddPurchaseEvent()
        {

            PurchaseEvent purchaseEvent = new PurchaseEvent()
            {
                Id = selectedEventId,
                ClientId = selectedClientId,
                ItemId = selectedItemId,
                EventDate = DateTime.Now.ToString("dd/MM/yy")
            };
            purchaseService.AddPurchaseEvent(purchaseEvent);
            FetchPurchases();
        }

        private void AddReturnEvent()
        {
            if (ReturnPossible())
            {
                ReturnEvent returnEvent = new ReturnEvent()
                {
                    Id = selectedEventId,
                    ClientId = selectedClientId,
                    ItemId = selectedItemId,
                    EventDate = DateTime.Now.ToString("dd/MM/yy")
                };
                returnService.AddReturnEvent(returnEvent);
                FetchReturns();
                FetchPurchases();
            }
            else ShowPopupWindow("There's no such purchase to return.");
        }

        private bool ReturnPossible()
        {
            foreach (var c in purchaseService.GetAllClientPurchases(selectedClientId))
            {
                if (c.ItemId == selectedItemId) return true;
            }
            return false;
        }


        private bool ProperInputs()
        {
            return ItemId > 0 && ClientId > 0;
        }

        private void ShowPopupWindow(string message)
        {
            MessageBoxShowDelegate(message);
        }

        private void ValidateClientIdInput(int id, string propertyName)
        {
            errorValidator.ClearErrors(propertyName);

            if (purchaseService.ClientExists(id) == false)
            {
                errorValidator.AddError(propertyName, $"{propertyName} doesn't match any client!");
            }
        }

        private void ValidateItemIdInput(int id, string propertyName)
        {
            errorValidator.ClearErrors(propertyName);

            if (purchaseService.ItemExists(id) == false)
            {
                errorValidator.AddError(propertyName, $"{propertyName} doesn't match any Item!");
            }
        }

        #endregion


        #region Validation

        private ErrorValidator errorValidator = new ErrorValidator();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanAddEvent));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return errorValidator.GetErrors(propertyName);
        }

        public bool HasErrors => errorValidator.HasErrors;

        #endregion
    }
}

