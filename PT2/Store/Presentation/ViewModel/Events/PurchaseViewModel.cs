using Presentation.Model;
using Data;

namespace Presentation.ViewModel
{
    public class PurchaseViewModel : ViewModelBase
    {

        #region InitialSetup

        public PurchaseViewModel() { }

        public PurchaseViewModel(PurchaseEvent purchaseEvent)
        {
            Id = purchaseEvent.Id;
            ItemId = purchaseEvent.ItemId;
            ClientId = purchaseEvent.ClientId;
            Date = purchaseEvent.EventDate;

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

        public int ItemId
        {
            get => ItemId;
            set
            {
                ItemId = value;
                OnPropertyChanged(nameof(ItemId));
            }
        }

        public int ClientId
        {
            get => clientId;
            set
            {
                clientId = value;
                OnPropertyChanged(nameof(ClientId));
            }
        }

        public string Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        #endregion


        #region PrivateAttributes

        private int id;
        private int ItemId;
        private int clientId;
        private string date;

        #endregion

    }
}
