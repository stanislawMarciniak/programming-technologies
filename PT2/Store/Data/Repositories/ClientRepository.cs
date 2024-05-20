using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Data
{
    public class ClientRepository
    {
        public List<Client> GetAllClients()
        {
            using (var db = new StoreDataContext())
            {
                return db.Clients.Select(client => client).ToList();
            }
        }

        public int GetNumberOfClients()
        {
            using (var db = new StoreDataContext())
            {
                return db.Clients.Select(client => client).ToList().Count;
            }
        }

        public Client GetClientById(int id)
        {
            using (var db = new StoreDataContext())
            {
                return db.Clients.FirstOrDefault(client => client.Id.Equals(id));
            }
        }

        public Client GetClientByCredentials(string firstName, string lastName)
        {
            using (var db = new StoreDataContext())
            {
                return db.Clients
                    .FirstOrDefault(client => (client.FirstName.Equals(firstName) &&
                                               (client.LastName.Equals(lastName))));
            }
        }

        public Client GetLastClient()
        {
            using (var db = new StoreDataContext())
            {
                return db.Clients.Select(client => client).ToList().LastOrDefault();
            }
        }

        public void AddClient(Client client)
        {
            using (var db = new StoreDataContext())
            {
                db.Clients.InsertOnSubmit(client);
                db.SubmitChanges();
            }
        }

        public void DeleteClient(int id)
        {
            using (var db = new StoreDataContext())
            {
                Client clientToDelete = db.Clients.FirstOrDefault(client => client.Id.Equals(id));

                if (clientToDelete != null)
                {
                    db.Clients.DeleteOnSubmit(clientToDelete);
                    db.SubmitChanges();
                }
            }
        }

        public void UpdateClient(Client client)
        {
            using (var db = new StoreDataContext())
            {
                Client clientToUpdate = db.Clients.FirstOrDefault(c => c.Id.Equals(client.ClientID));

                if (clientToUpdate != null)
                {
                    clientToUpdate.FirstName = client.FirstName;
                    clientToUpdate.LastName = client.LastName;
                    db.SubmitChanges();
                }
            }
        }
    }
}
