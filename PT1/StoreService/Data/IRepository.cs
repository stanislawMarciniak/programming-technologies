using System;
using System.Collections.Generic;
using System.Text;

namespace StoreService.Data
{
    public interface IRepository
    {
        // Client
        void AddClient(Client client);
        void DeleteClient(Client client);
        Client? GetClientByID(int clientID);
        List<Client> GetAllClients();
        List<int> GetAllClientsIDs();

        // Item
        void AddItem(Item item);
        void DeleteItem(Item item);
        Item GetItemByID(int itemID);
        IEnumerable<Item> GetAllItems();
        IEnumerable<int> GetAllItemsIDs();

        // Event
        void AddEvent(EventBase eventBase);
        void DeleteEvent(EventBase eventBase);
        List<EventBase> GetAllEvents();

        // State
        void AddState(State state);
        void DeleteState(State state);
        List<State> GetAllStates();
    }
}
