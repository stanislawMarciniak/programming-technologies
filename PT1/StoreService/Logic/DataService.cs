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

        public void DeleteItem(int id)
        {
            repository.DeleteItem(id);
        }

        public IEnumerable<Item> GetAllItems()
        {
            return repository.GetAllItems();
        }

        public Item GetItemById(int id)
        {
            return repository.GetItemById(id);
        }

        public List<IEvent> GetAllItemEvents(Item item)
        {
            List<IEvent> events = new List<IEvent>();

            foreach (IEvent e in repository.GetAllEvents())
            {
                if (e.State.Item.Equals(item))
                {
                    events.Add(e);
                }
            }
            return events;
        }





        // --------------- Client -----------------  

        public void AddClient(int id, String name, String surname)
        {
            repository.AddClient(new Client(id, name, surname));
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
            return repository.GetClientById(id);
        }

        public List<IEvent> GetAllClientEvents(int id)
        {
            List<IEvent> events = new List<IEvent>();
            Client client = repository.GetClientById(id);

            foreach (IEvent ev in repository.GetAllEvents())
            {
                if (ev.Client.Equals(client))
                {
                    events.Add(ev);
                }
            }
            return events;
        }





        // --------------- Actions -----------------  

        public void PurchaseItem(int productId, int clientId)
        {
            Item product = repository.GetItemById(productId);
            Client client = repository.GetClientById(clientId);
            State state = new State(product);

            repository.DeleteItem(productId);
            repository.AddEvent(new EventPurchase(state, client));
            repository.AddState(state);
        }


        public void ReturnItem(Item product, int clientId)
        {
            Client client = repository.GetClientById(clientId);

            List<IEvent> productEvents = GetAllItemEvents(product);

            if (productEvents.Last<IEvent>() is EventReturn)
            {
                throw new Exception("Item is either not in the shop or wasn't yet purchased");
            }

            State state = new State(product);

            repository.AddItem(product);
            repository.AddEvent(new EventReturn(state, client));
            repository.AddState(state);
        }
    }
}