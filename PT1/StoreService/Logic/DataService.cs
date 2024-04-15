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
            repository.AddItem(new Item(id, price, category));
        }

        public void DeleteItem(Item item)
        {
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
            repository.AddClient(new Client(id, name, surname, email));
        }

        public void DeleteClient(Client client)
        {
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


        public void ReturnItem(Item product, int clientId, int quantity, int quantityReturned, String returnReason)
        {
            Client client = repository.GetClientByID(clientId);

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