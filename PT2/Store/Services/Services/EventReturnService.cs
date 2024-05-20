using System.Data;
using System.Collections.Generic;
using Data;

namespace Service
{
    public class EventReturnService
    {
        private ClientRepository clientRepository = new ClientRepository();
        private ItemRepository ItemRepository = new ItemRepository();
        private ReturnEventRepository returnRepository = new ReturnEventRepository();
        private EventPurchaseRepository purchaseRepository = new EventPurchaseRepository();

        public bool AddReturnEvent(EventReturn ev)
        {
            if (ev == null || InvalidEventData(ev))
            {
                return false;
            }

            EventPurchase recentPurchase = GetClientRecentPurchaseOfSuchItem(ev);

            if (recentPurchase == null)
            {
                return false;
            }

            returnRepository.AddReturnEvent(ev);
            purchaseRepository.DeletePurchaseEvent(recentPurchase.EventID);
            return true;
        }

        public List<EventReturn> GetAllReturns()
        {
            return returnRepository.GetAllReturnEvents();
        }

        public List<EventReturn> GetAllClientReturns(int id)
        {
            return returnRepository.GetReturnEventsByClientId(id);
        }

        public List<EventReturn> GetAllItemReturns(int id)
        {
            return returnRepository.GetReturnEventsByItemId(id);
        }

        private EventReturn GetClientRecentPurchaseOfSuchItem(EventReturn ev)
        {
            return purchaseRepository.GetMostRecentByClientIdAndItemId(ev.ClientID, ev.ItemID);
        }

        private bool InvalidEventData(EventReturn ev)
        {
            return !ClientExists(ev.ClientID) || !ItemExists(ev.ItemID);
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
