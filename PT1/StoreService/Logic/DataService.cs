using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using StoreService.Data;

namespace StoreService.Logic
{
    public class DataService
    {
        private IRepository repository;

        public DataService(IRepository repository)
        {
            this.repository = repository;
        }


        // --------------- Item -----------------  

        public void AddItem(int id, double price, Category category)
        {
            Item newItem = new Item(id, price, category);
            repository.AddItem(new Item(id, price, category));
        }

        public void DeleteItem(Item item)
        {
            if (!repository.GetAllItems().Any(i => i.ItemID == item.ItemID))
            {
                throw new KeyNotFoundException("Item not found.");
            }
            repository.DeleteItem(item);
        }

        public IEnumerable<Item> GetAllItems()
        {
            return repository.GetAllItems();
        }

        public Item GetItemByID(int itemID)
        {
            return repository.GetItemByID(itemID);
        }

        public List<EventBase> GetAllItemEvents(Item item)
        {
            List<EventBase> events = new List<EventBase>();

            foreach (EventBase e in repository.GetAllEvents())
            {
                if (e.State.Item.Equals(item))
                {
                    events.Add(e);
                }
            }
            return events;
        }


        // --------------- Client -----------------  

        public void AddClient(int id, String name, String surname, String email)
        {
            if (repository.GetClientByID(id) != null)
            {
                throw new Exception("Client ID must be unique.");
            }

            Client newClient = new Client(id, name, surname, email);
            repository.AddClient(newClient);
        }

        public void DeleteClient(Client client)
        {
            if (!repository.GetAllClients().Any(i => i.ClientID == client.ClientID))
            {
                throw new KeyNotFoundException("Item not found.");
            }
            repository.DeleteClient(client);
        }

        public List<Client> GetAllClients()
        {
            return repository.GetAllClients();
        }

        public Client GetClient(int id)
        {
            return repository.GetClientByID(id);
        }

        public List<EventBase> GetAllClientEvents(int id)
        {
            List<EventBase> events = new List<EventBase>();
            Client client = repository.GetClientByID(id);

            foreach (EventBase ev in repository.GetAllEvents())
            {
                if (ev.Client.Equals(client))
                {
                    events.Add(ev);
                }
            }
            return events;
        }





        // --------------- Events -----------------  

        public void PurchaseItem(int productId, int clientId, int quantity)
        {
            Item item = repository.GetItemByID(productId);
            Client client = repository.GetClientByID(clientId);
            State state = new State(item, quantity);

            repository.DeleteItem(item);
            repository.AddEvent(new EventPurchase(state, client, quantity));
            repository.AddState(state);
        }


        public void ReturnItem(Item product, Client client, int quantity, int quantityReturned, String returnReason)
        {

            List<EventBase> productEvents = GetAllItemEvents(product);

            if (productEvents.Last<EventBase>() is EventReturn)
            {
                throw new Exception("Item is either not in the shop or wasn't yet purchased");
            }

            State state = new State(product, quantity);

            repository.AddItem(product);
            repository.AddEvent(new EventReturn(state, client, quantityReturned, returnReason));
            repository.AddState(state);
        }
    }
}