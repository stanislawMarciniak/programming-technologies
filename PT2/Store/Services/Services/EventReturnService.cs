using System.Data;
using System.Collections.Generic;

namespace Service
{
    public class EventReturnService
    {
        private ClientRepository clientRepository = new ClientRepository();
        private ItemRepository ItemRepository = new ItemRepository();
        private ReturnEventRepository returnRepository = new ReturnEventRepository();
        private PurchaseEventRepository purchaseRepository = new PurchaseEventRepository();

        public bool AddReturnEvent(ReturnEvent ev)
        {
            if (ev == null || InvalidEventData(ev))
            {
                return false;
            }

            PurchaseEvent recentPurchase = GetClientRecentPurchaseOfSuchItem(ev);

            if (recentPurchase == null)
            {
                return false;
            }

            returnRepository.AddReturnEvent(ev);
            purchaseRepository.DeletePurchaseEvent(recentPurchase.Id);
            return true;
        }

        public List<ReturnEvent> GetAllReturns()
        {
            return returnRepository.GetAllReturnEvents();
        }

        public List<ReturnEvent> GetAllClientReturns(int id)
        {
            return returnRepository.GetReturnEventsByClientId(id);
        }

        public List<ReturnEvent> GetAllItemReturns(int id)
        {
            return returnRepository.GetReturnEventsByItemId(id);
        }

        private PurchaseEvent GetClientRecentPurchaseOfSuchItem(ReturnEvent ev)
        {
            return purchaseRepository.GetMostRecentByClientIdAndItemId(ev.ClientId, ev.ItemId);
        }

        private bool InvalidEventData(ReturnEvent ev)
        {
            return !ClientExists(ev.ClientId) || !ItemExists(ev.ItemId);
        }

        private bool ClientExists(int id)
        {
            return clientRepository.GetClientById(id) != null;
        }

        private bool ItemExists(int id)
        {
            return ItemRepository.GetItemById(id) != null;
        }
    }
}
