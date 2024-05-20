using System.Collections.Generic;
using System.Data;

namespace Service
{
    public class EventPurchaseService
    {
        private PurchaseEventRepository purchaseRepository = new PurchaseEventRepository();
        private ClientRepository clientRepository = new ClientRepository();
        private ItemRepository ItemRepository = new ItemRepository();

        public bool AddPurchaseEvent(PurchaseEvent ev)
        {
            if (ev == null || InvalidEventData(ev))
            {
                return false;
            }

            purchaseRepository.AddPurchaseEvent(ev);
            return true;
        }

        public bool DeletePurchaseEvent(int id)
        {
            if (PurchaseExists(id))
            {
                purchaseRepository.DeletePurchaseEvent(id);
                return true;
            }

            return false;
        }

        public List<PurchaseEvent> GetAllPurchases()
        {
            return purchaseRepository.GetAllPurchaseEvents();
        }

        public PurchaseEvent GetPurchaseById(int id)
        {
            return purchaseRepository.GetPurchaseEventById(id);
        }

        public List<PurchaseEvent> GetAllClientPurchases(int id)
        {
            return purchaseRepository.GetPurchaseEventsByClientId(id);
        }

        public List<PurchaseEvent> GetAllItemPurchases(int id)
        {
            return purchaseRepository.GetPurchaseEventsByItemId(id);
        }

        public PurchaseEvent GetMostRecentPurchase()
        {
            return purchaseRepository.GetMostRecentPurchase();
        }

        public PurchaseEvent GetLastClientPurchaseOfItem(int clientId, int ItemId)
        {
            return purchaseRepository.GetMostRecentByClientIdAndItemId(clientId, ItemId);
        }

        private bool InvalidEventData(PurchaseEvent ev)
        {
            return PurchaseExists(ev.Id) || !ClientExists(ev.ClientId) || !ItemExists(ev.ItemId);
        }

        public bool ClientExists(int id)
        {
            return clientRepository.GetClientById(id) != null;
        }

        public bool ItemExists(int id)
        {
            return ItemRepository.GetItemById(id) != null;
        }

        public bool PurchaseExists(int id)
        {
            return purchaseRepository.GetPurchaseEventById(id) != null;
        }
    }
}
