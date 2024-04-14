using System;
using System.Collections.Generic;

namespace StoreService.Data
{
    public class Repository : IRepository
    {
        private DataContext dataContext;

        public Repository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }


        // Client

        public void AddClient(Client client)
        {
            if (!NoSuchClientID(client.ClientID))
            {
                throw new Exception("Client with ID: " + client.ClientID + " already exists.");
            }

            dataContext.clients.Add(client);
        }

        public void DeleteClient(Client client)
        {
            if (NoSuchClientID(client.ClientID))
            {
                throw new KeyNotFoundException("Could not find the client with ID: " + client.ClientID + " .");
            }

            dataContext.clients.Remove(client);
        }

        public Client? GetClientByID(int clientID)
        {
            if (NoSuchClientID(clientID))
            {
                throw new KeyNotFoundException("Client with an id " + clientID + " is not found");
            }
            return dataContext.clients.Find(client => client.ClientID == clientID);
        }

        public List<Client> GetAllClients()
        {
            return dataContext.clients;
        }

        public List<int> GetAllClientsIDs()
        {
            List<int> IDs = new List<int>();

            foreach (Client client in dataContext.clients)
            {
                IDs.Add(client.ClientID);
            }
            return IDs;
        }

        public bool NoSuchClientID(int clientID)
        {
            return !dataContext.clients.Exists(c => c.ClientID == clientID);
        }


        // Item   

        public void AddItem(Item item)
        {
            if (!NoSuchItemID(item.ItemID))
            {
                throw new Exception("Item with ID: " + item.ItemID + " already exists.");
            }
            dataContext.items.Add(item.ItemID, item);
        }

        public void DeleteItem(Item item)
        {
            if (NoSuchItemID(item.ItemID))
            {
                throw new KeyNotFoundException("Could not find the item with ID: " + item.ItemID + ".");

            }
            dataContext.items.Remove(item.ItemID);
        }

        public Item GetItemByID(int itemID)
        {
            if (NoSuchItemID(itemID))
            {
                throw new KeyNotFoundException("Could not find the item with ID: " + itemID + " .");
            }

            return dataContext.items[itemID];
        }

        public IEnumerable<Item> GetAllItems()
        {
            return dataContext.items.Values;
        }

        public IEnumerable<int> GetAllItemsIDs()
        {
            return dataContext.items.Keys;
        }

        public bool NoSuchItemID(int itemID)
        {
            return !dataContext.items.ContainsKey(itemID);
        }


        // Event 

        public void AddEvent(EventBase eventBase)
        {
            dataContext.events.Add(eventBase);
        }

        public void DeleteEvent(EventBase eventBase)
        {
            dataContext.events.Remove(eventBase);
        }

        public List<EventBase> GetAllEvents()
        {
            return dataContext.events;
        }


        // State 

        public void AddState(State state)
        {
            dataContext.states.Add(state);
        }

        public void DeleteState(State state)
        {
            if (NoSuchState(state))
            {
                throw new Exception();
            }

            dataContext.states.Remove(state);
        }

        public List<State> GetAllStates()
        {
            return dataContext.states;
        }

        public bool NoSuchState(State state)
        {
            return !dataContext.states.Exists(s => s.Equals(state));
        }
    }
}