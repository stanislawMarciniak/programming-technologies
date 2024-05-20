using Data;
using System.Collections.Generic;
using System.Data;

namespace Service
{
    public class EventPurchaseService
    {
        private EventPurchaseRepository eventPurchaseRepository = new EventPurchaseRepository();
        private ClientRepository clientRepository = new ClientRepository();
        private ItemRepository itemRepository = new ItemRepository();

        public bool AddPurchaseEvent(EventPurchase ev)
        {
            if (ev == null || InvalidEventData(ev))
            {
                return false;
            }

            eventPurchaseRepository.AddEventPurchase(ev);
            return true;
        }

        public bool DeletePurchaseEvent(int id)
        {
            if (PurchaseExists(id))
            {
                eventPurchaseRepository.DeletePurchaseEvent(id);
                return true;
            }

            return false;
        }

        public List<EventPurchase> GetAllPurchases()
        {
            return eventPurchaseRepository.GetAllPurchaseEvents();
        }

        public EventPurchase GetPurchaseEventById(int id)
        {
            return eventPurchaseRepository.GetEventsPurchaseById(id);
        }

        public List<EventPurchase> GetAllClientPurchases(int id)
        {
            return eventPurchaseRepository.GetEventsPurchaseByClientId(id);
        }

        public List<EventPurchase> GetAllItemPurchases(int id)
        {
            return eventPurchaseRepository.GetEventsPurchaseByItemId(id);
        }

        public EventPurchase GetMostRecentPurchase()
        {
            return eventPurchaseRepository.GetMostRecentPurchase();
        }

        public EventPurchase GetLastClientPurchaseOfItem(int clientId, int ItemId)
        {
            return eventPurchaseRepository.GetMostRecentByClientIdAndItemId(clientId, ItemId);
        }

        private bool InvalidEventData(EventPurchase ev)
        {
            return PurchaseExists(ev.EventID) || !ClientExists(ev.ClientID) || !ItemExists(ev.ItemID);
        }

        public bool ClientExists(int id)
        {
            return clientRepository.GetClientById(id) != null;
        }

        public bool ItemExists(int id)
        {
            return itemRepository.GetItemById(id) != null;
        }

        public bool PurchaseExists(int id)
        {
            return eventPurchaseRepository.GetEventsPurchaseById(id) != null;
        }
    }
}
